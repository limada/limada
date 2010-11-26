/*
 * Limaki 
 * Version 0.08
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


using System.Collections.Generic;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Drawing.UI;

namespace Limaki.Widgets.Layout {
    public class ShapeProxy<TData, TItem, TEdge> 
        where TEdge : IEdge<TItem>, TItem 
        where TItem : IWidget 
        where TData : Scene {
        public ShapeProxy (ILayout<TData, TItem> layout) {
            this.layout = layout;
        }

        private ILayout<TData, TItem> layout = null;
        public ICollection<TEdge> AffectedEdges = new List<TEdge>();
        private IDictionary<TItem, PointI> locations = new Dictionary<TItem, PointI>();
        private IDictionary<TItem, IShape> invokeList = new Dictionary<TItem, IShape>();

        public virtual void Commit(TData Data) {

            ICollection<TItem> invokeDone = new Set<TItem>();

            foreach (KeyValuePair<TItem, IShape> kvp in invokeList) {
                // if kvp.Value (Shape) == null: item has a valid shape
                if (kvp.Value != null) {
                    Data.Commands.Add(new LayoutCommand<IWidget>(kvp.Key, LayoutActionType.Invoke));
                    invokeDone.Add(kvp.Key);
                }
                if (!(kvp.Key is TEdge)) {
                    Data.Commands.Add(new LayoutCommand<IWidget>(kvp.Key, LayoutActionType.Justify));
                }
            }

            foreach (KeyValuePair<TItem, PointI> kvp in locations) {
                IShape shape = kvp.Key.Shape;
                if (shape != null && shape.Location.Equals(kvp.Value) && !invokeDone.Contains(kvp.Key)) {
                    Data.Commands.Add(new LayoutCommand<IWidget>(kvp.Key, LayoutActionType.AddBounds));
                } else {
                    Data.Commands.Add(new MoveCommand(kvp.Key, kvp.Value));
                }
            }


            foreach (TEdge edge in AffectedEdges) {
                if (edge.Shape == null) {
                    if (!invokeDone.Contains(edge)) {
                        Data.Commands.Add(
                            new LayoutCommand<IWidget, IShape>(
                                edge, GetShape(edge), LayoutActionType.Invoke));
                        invokeDone.Add(edge);
                    }
                }
            }

            foreach (TEdge edge in AffectedEdges) {
                if (invokeDone.Contains(edge))
                    Data.Commands.Add(new LayoutCommand<IWidget>(edge, LayoutActionType.Justify));
                else {
                    Data.Commands.Add(new LayoutCommand<IWidget>(edge, LayoutActionType.AddBounds));
                    invokeDone.Add(edge);
                }
            }
        }

        

        public virtual void SetLocation(TItem widget, PointI location) {
            locations[widget] = location;
        }

        public virtual PointI GetLocation(TItem widget) {
            PointI result;
            if (!locations.TryGetValue(widget, out result)) {
                result = widget.Location;
            }
            return result;
        }

        public virtual SizeI GetSize(TItem item) {
            IShape shape = GetShape(item);
            return shape.Size;
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
            IShape shape = null;
            if (item.Shape == null) {
                if (!invokeList.TryGetValue(item, out shape)) {
                    shape = this.layout.CreateShape(item);
                    if (!(item is TEdge))
                        this.layout.Justify(item, shape);
                    invokeList.Add(item, shape);
                }
            } else {
                shape = item.Shape;

                if (shape.Size.Equals(SizeI.Empty)) {
                    Justify (item);
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
            IShape shape = null;
            if (item.Shape == null) {
                shape = GetShape (item);
            } else {
                if (!invokeList.ContainsKey (item)) {
                    invokeList.Add (item, null);
                }
                shape = item.Shape;
            }
            return shape;
        }

        public virtual void Justify(TItem item) {
            IShape shape = EnsureInvoke (item);
            if (!(item is TEdge))
                layout.Justify(item, shape);
        }


    }
}