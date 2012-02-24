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
 * http://limada.sourceforge.net
 * 
 */


using System;
using System.Collections.Generic;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Presenter.UI.GraphScene;
using Xwt;

namespace Limaki.Presenter.Layout {
    public class GraphItemShapeProxy<TItem, TEdge> : IShapeProxy<TItem, TEdge>
        where TEdge : IEdge<TItem>, TItem {

        public GraphItemShapeProxy(IGraphLayout<TItem, TEdge> layout) {
            this.layout = layout;
            this.AffectedEdges = new List<TEdge>();
            this.ShapeGetter = layout.GetShape;
        }

        private IGraphLayout<TItem, TEdge> layout = null;
        public ICollection<TEdge> AffectedEdges { get; set; }

        private IDictionary<TItem, Point> locations = new Dictionary<TItem, Point>();
        private IDictionary<TItem, IShape> invokeList = new Dictionary<TItem, IShape>();

        protected Func<TItem, IShape> ShapeGetter = null;

        public virtual void Commit(IGraphScene<TItem, TEdge> scene) {

            ICollection<TItem> invokeDone = new Set<TItem>();

            foreach (KeyValuePair<TItem, IShape> kvp in invokeList) {
                // if kvp.Value (Shape) == null: item has a valid shape
                if (kvp.Value != null) {
                    scene.Requests.Add(new LayoutCommand<TItem>(kvp.Key, LayoutActionType.Invoke));
                    invokeDone.Add(kvp.Key);
                }
                if (!(kvp.Key is TEdge)) {
                    scene.Requests.Add(new LayoutCommand<TItem>(kvp.Key, LayoutActionType.Justify));
                }
            }

            foreach (KeyValuePair<TItem, Point> kvp in locations) {
                IShape shape = ShapeGetter(kvp.Key);
                if (shape != null && shape.Location.Equals(kvp.Value) && !invokeDone.Contains(kvp.Key)) {
                    scene.Requests.Add(new LayoutCommand<TItem>(kvp.Key, LayoutActionType.AddBounds));
                } else {
                    scene.Requests.Add(new MoveCommand<TItem>(kvp.Key, ShapeGetter, kvp.Value));
                }
            }

            Func<TEdge, bool> isAffected =
                ( edge ) => 
                    (ShapeGetter(edge.Root)!=null || locations.ContainsKey (edge.Root))&& 
                    (ShapeGetter(edge.Leaf)!=null || locations.ContainsKey (edge.Leaf));

            foreach (TEdge edge in AffectedEdges) {
                if (ShapeGetter(edge) == null) {
                    if (!invokeDone.Contains(edge)) {
                        scene.Requests.Add(
                            new LayoutCommand<TItem, IShape>(
                                edge, this.GetShape(edge), LayoutActionType.Invoke));
                        invokeDone.Add(edge);
                    }
                }
            }

            foreach (TEdge edge in AffectedEdges) {
                if (invokeDone.Contains(edge))
                    scene.Requests.Add(new LayoutCommand<TItem>(edge, LayoutActionType.Justify));
                else {
                    scene.Requests.Add(new LayoutCommand<TItem>(edge, LayoutActionType.AddBounds));
                    invokeDone.Add(edge);
                }
            }
        }

        public virtual void SetLocation(TItem item, Point location) {
            locations[item] = location;
        }

        public virtual Point GetLocation(TItem item) {
            Point result;
            if (!locations.TryGetValue(item, out result)) {
                var shape = ShapeGetter(item);
                result = shape.Location;
            }
            return result;
        }

        public virtual Size GetSize(TItem item) {
            var shape = GetShape(item);
            return shape.Size;
        }
        public virtual void SetSize(TItem item, Size value) {
            var shape = GetShape(item);
            shape.Size = value;
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
        public virtual IShape GetShape(TItem item) {
            var shape = ShapeGetter(item);
            if (shape == null) {
                if (!invokeList.TryGetValue(item, out shape)) {
                    shape = this.layout.CreateShape(item);
                    if (!(item is TEdge))
                        this.layout.Justify(item, shape);
                    invokeList.Add(item, shape);
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
                shape = GetShape(item);
            } else {
                if (!invokeList.ContainsKey(item)) {
                    invokeList.Add(item, null);
                }
            }
            return shape;
        }

        public virtual void Justify(TItem item) {
            var shape = EnsureInvoke(item);
            if (!(item is TEdge))
                layout.Justify(item, shape);
        }


    }
}