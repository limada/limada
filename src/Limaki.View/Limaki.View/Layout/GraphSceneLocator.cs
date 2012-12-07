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
using System.Collections.Generic;
using Limaki.Actions;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;
using Xwt;

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

        public bool HasLocation (TItem item) {
            return ShapeGetter(item) != null || changedLocations.ContainsKey(item);
        }

        public bool HasSize (TItem item) {
            return ShapeGetter(item) != null || itemsToInvoke.ContainsKey(item);
        }

        public virtual void Commit (ICollection<ICommand<TItem>> requests) {

            ICollection<TItem> invokeDone = new Set<TItem>();

            foreach (KeyValuePair<TItem, IShape> kvp in itemsToInvoke) {
                // if kvp.Value (Shape) == null: item has a valid shape
                if (kvp.Value != null) {
                    requests.Add(new LayoutCommand<TItem>(kvp.Key, LayoutActionType.Invoke));
                    invokeDone.Add(kvp.Key);
                }
                if (!(kvp.Key is TEdge)) {
                    requests.Add (new LayoutCommand<TItem> (kvp.Key, LayoutActionType.Justify));
                }
            }

            foreach (KeyValuePair<TItem, Point> kvp in changedLocations) {
                IShape shape = ShapeGetter(kvp.Key);
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

            foreach (TEdge edge in AffectedEdges) {
                if (ShapeGetter(edge) == null) {
                    if (!invokeDone.Contains(edge)) {
                        requests.Add(
                            new LayoutCommand<TItem, IShape>(
                                edge, this.GetOrCreateShape(edge), LayoutActionType.Invoke));
                        invokeDone.Add(edge);
                    }
                }
            }

            foreach (TEdge edge in AffectedEdges) {
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


    }
}