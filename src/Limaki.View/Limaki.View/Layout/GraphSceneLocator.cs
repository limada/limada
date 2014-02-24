/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */


using System;
using System.Linq;
using System.Collections.Generic;
using Limaki.Actions;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;
using Xwt;
using Limaki.Drawing.Indexing;

namespace Limaki.View.Layout {
    /// <summary>
    /// a Locator for Graph-Items
    /// assuming that an item has a Shape
    /// operations are resulting in commands
    /// and have to be commited to a receiver
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public class GraphSceneLocator<TItem, TEdge> : IGraphSceneLocator<TItem, TEdge>
        where TEdge : IEdge<TItem>, TItem {

        public GraphSceneLocator(IShaper<TItem> shaper) {

            // must be a list, cause order of adding is important, and can occur more than once
            this.AffectedEdges = new List<TEdge>(); 
            
            this.ShapeGetter = shaper.GetShape;
            this.ShapeCreator = shaper.CreateShape;
            this.ShapeJustifier = shaper.Justify;
        }
      
        public ICollection<TEdge> AffectedEdges { get; set; }

        private IDictionary<TItem, Point> changedLocations = new Dictionary<TItem, Point>();
        private IDictionary<TItem, IShape> itemsToInvoke = new Dictionary<TItem, IShape>();

        protected Func<TItem, IShape> ShapeGetter = null;
        protected Func<TItem, IShape> ShapeCreator = null;
        protected Action<TItem, IShape> ShapeJustifier = null;

        public virtual void SetLocation (TItem item, Point location) {
            changedLocations[item] = location;
            UpdateIndex(item);
        }

        public virtual Point GetLocation (TItem item) {
            var result = default(Point);
            if (!changedLocations.TryGetValue(item, out result)) {
                var shape = ShapeGetter(item);
                if (shape != null)
                    result = shape.Location;
            }
            return result;
        }

        public virtual Size GetSize (TItem item) {
            var shape = GetOrCreateShape (item);
            return shape.Size;
        }

        public virtual void SetSize (TItem item, Size value) {
            var shape = GetOrCreateShape (item);
            shape.Size = value;
        }

        public virtual bool HasLocation (TItem item) {
            return ShapeGetter(item) != null || changedLocations.ContainsKey(item);
        }

        public virtual bool HasSize (TItem item) {
            return ShapeGetter(item) != null || itemsToInvoke.ContainsKey(item);
        }

        public virtual void Commit (ICollection<ICommand<TItem>> requests) {

            var invokeDone = new Set<TItem>();

            foreach (var kvp in itemsToInvoke) {
                // if kvp.Value (Shape) == null: item has a valid shape
                if (kvp.Value != null) {
                    requests.Add(new LayoutCommand<TItem>(kvp.Key, LayoutActionType.Invoke));
                    invokeDone.Add(kvp.Key);
                }
                if (!(kvp.Key is TEdge)) {
                    requests.Add (new LayoutCommand<TItem> (kvp.Key, LayoutActionType.Justify));
                }
            }

            foreach (var kvp in changedLocations) {
                var shape = ShapeGetter(kvp.Key);
                if (shape != null && shape.Location.Equals(kvp.Value) && !invokeDone.Contains(kvp.Key)) {
                    requests.Add(new LayoutCommand<TItem>(kvp.Key, LayoutActionType.AddBounds));
                } else {
                    requests.Add(new MoveCommand<TItem>(kvp.Key, ShapeGetter, kvp.Value));
                }
            }

            Func<TEdge, bool> isAffected =
                ( edge ) => 
                    (ShapeGetter(edge.Root)!=null || changedLocations.ContainsKey (edge.Root))&& 
                    (ShapeGetter(edge.Leaf)!=null || changedLocations.ContainsKey (edge.Leaf));

            foreach (var edge in AffectedEdges) {
                if (ShapeGetter(edge) == null) {
                    if (!invokeDone.Contains(edge)) {
                        requests.Add(
                            new LayoutCommand<TItem, IShape>(
                                edge, this.GetOrCreateShape(edge), LayoutActionType.Invoke));
                        invokeDone.Add(edge);
                    }
                }
            }

            foreach (var edge in AffectedEdges) {
                if (invokeDone.Contains(edge))
                    requests.Add(new LayoutCommand<TItem>(edge, LayoutActionType.Justify));
                else {
                    requests.Add(new LayoutCommand<TItem>(edge, LayoutActionType.AddBounds));
                    invokeDone.Add(edge);
                }
            }
        }

        

        /// <summary>
        /// gives back a valid shape
        /// if item has no shape:
        /// Layout.CreateShape is called 
        /// item, shape is added to invokeList
        /// shape is Justified
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual IShape GetOrCreateShape(TItem item) {
            var shape = ShapeGetter(item);
            if (shape == null) {
                if (!itemsToInvoke.TryGetValue(item, out shape)) {
                    shape = ShapeCreator(item);
                    if (!(item is TEdge))
                        ShapeJustifier(item, shape);
                    itemsToInvoke.Add(item, shape);
                    UpdateIndex(item);
                }
            } else {
                if (shape.Size.Equals(Size.Zero)) {
                    Justify(item);
                }
            }
            return shape;
        }

       

        /// <summary>
        /// adds item to invokeList
        /// if item.Shape == null, Invoke(item) is called
        /// else item,null is added to invokeList
        /// </summary>
        /// <param name="item"></param>
        public virtual IShape EnsureInvoke(TItem item) {
            var shape = ShapeGetter(item);
            if (shape == null) {
                shape = GetOrCreateShape(item);
            } else {
                if (!itemsToInvoke.ContainsKey(item)) {
                    itemsToInvoke.Add(item, null);
                }
            }
            return shape;
        }

        public virtual void Justify(TItem item) {
            var shape = EnsureInvoke(item);
            if (!(item is TEdge))
                ShapeJustifier(item, shape);
        }

        ISpatialIndex<TItem> _index = null;
        public IEnumerable<TItem> ElementsIn (Rectangle bounds) {
            if(_index==null) {
                _index = new SpatialQuadTreeIndex<TItem> {
                                                             BoundsOf = item => new Rectangle(this.GetLocation(item), this.GetSize(item)),
                                                             HasBounds = item => this.HasLocation(item) && this.HasSize(item)
                                                         };
                _index.AddRange(this.changedLocations.Keys);
            }
            var result= _index.Query(bounds);
            return result;
        }
        
        private void UpdateIndex (TItem item) {
            if (_index != null)
                _index = null;
        }

    }
}