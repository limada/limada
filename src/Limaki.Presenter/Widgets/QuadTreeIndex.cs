/*
 * Limaki 
 * Version 0.081
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
using Limaki.Drawing;
using Limaki.Actions;
using Limaki.Drawing.Indexing.QuadTrees;
using Limaki.Drawing.Shapes;
using System;

namespace Limaki.Widgets {
    public class QuadTreeIndex:SpatialIndex {
        private Quadtree<IWidget> _geoIndex = null;
        public Quadtree<IWidget> GeoIndex {
            get {
                if (_geoIndex == null) {
                    _geoIndex = new Quadtree<IWidget>();
                }
                return _geoIndex;
            }
            set { _geoIndex = value; }
        }

        protected override void Add(RectangleI bounds, IWidget item) {
            if ( bounds != RectangleI.Empty )
                GeoIndex.Add (bounds, item);
        }
        protected override void Remove(RectangleI bounds, IWidget item) {
            if ( bounds != RectangleI.Empty )
                GeoIndex.Remove(bounds, item);
        }

        public override void AddRange(IEnumerable<IWidget> items) {
            base.AddRange (items);
        }

        public override IEnumerable<IWidget> Query( RectangleS clipBounds, ZOrder zOrder ) {
            IEnumerable<IWidget> search = GeoIndex.Query(clipBounds);

            if (zOrder==ZOrder.EdgesFirst) {
                foreach (IWidget widget in search) {
                    if (widget is IEdgeWidget) {
                        if (ShapeUtils.Intersects(clipBounds, widget.Shape.BoundsRect))
                            yield return widget;
                    }
                }
            }

            foreach (IWidget widget in search) {
                if (!(widget is IEdgeWidget)) {
                    if (ShapeUtils.Intersects(clipBounds, widget.Shape.BoundsRect))
                        yield return widget;
                }
            }

            if (zOrder==ZOrder.NodesFirst) {
                foreach (IWidget widget in search) {
                    if (widget is IEdgeWidget) {
                        if (ShapeUtils.Intersects (clipBounds, widget.Shape.BoundsRect))
                            yield return widget;
                    }
                }
            }
        }

        public override IEnumerable<IWidget> Query(RectangleS clipBounds) {
            return Query (clipBounds, ZOrder.NodesFirst);
        }

        public override IEnumerable<IWidget> Query() {
            IEnumerable<IWidget> search = GeoIndex.QueryAll ();

            foreach (IWidget widget in search) {
                if (!(widget is IEdgeWidget)) {
                        yield return widget;
                }
            }
            foreach (IWidget widget in search) {
                if (widget is IEdgeWidget) {
                        yield return widget;
                }
            }
        }

        class RightBottomCommand : Command<ICollection<IWidget>, PointS> {
            public RightBottomCommand(ICollection<IWidget> target, PointS parameter)
                : base(target, parameter) {
                Parameter = new PointS(float.MinValue, float.MinValue);
            }
            public override void Execute() {
                int h = 0;
                int w = 0;
                foreach (IWidget widget in Subject) {
                    RectangleI bounds = widget.Shape.BoundsRect;
                    int r = bounds.Right;
                    int b = bounds.Bottom;
                    if (r > w) w = r;
                    if (b > h) h = b;
                }
                Parameter = new PointS(w, h);
            }
        }

        class LeftTopCommand : Command<ICollection<IWidget>, PointS> {
            public LeftTopCommand(ICollection<IWidget> target, PointS parameter)
                : base(target, parameter) {
                Parameter = new PointS(float.MaxValue, float.MaxValue);
            }
            public override void Execute() {
                int x = 0;
                int y = 0;
                foreach (IWidget widget in Subject) {
                    RectangleI bounds = widget.Shape.BoundsRect;
                    int l = bounds.X;
                    int t = bounds.Y;
                    if (l < x)
                        x = l;
                    if (t < y)
                        y = t;
                }
                Parameter = new PointS(x, y);
            }
        }

        /// <summary>
        /// not used!
        /// </summary>
        class BoundsVisitor : IItemVisitor<IWidget> {
            public PointS Parameter;
            public void VisitItem(IWidget item) {
                int w = (int)Parameter.X;
                int h = (int)Parameter.Y;
                RectangleI bounds = item.Shape.BoundsRect;
                int r = bounds.Right;
                int b = bounds.Bottom;
                if (r > w)
                    Parameter.X = r;
                if (b > h)
                    Parameter.Y = b;
            }

            public RectangleS GetEnvelope(IWidget item) {
                return item.Shape.BoundsRect;
            }

            public bool ProvidesEnvelope { get { return true; } }

        }

        protected override RectangleI CalculateBounds() {
            // remark: the starting values could be used to 
            // opimize further if boundsDirty is more refined;
            // eg: leftDirty leads to : 
            // l = float.MaxValue, t = float.MinValue, r = float.maxvalue, b = float.maxValue

            var l = float.MaxValue; var t = float.MaxValue;
            var r = float.MinValue; var b = float.MinValue;
            
            GeoIndex.QueryBounds(
                ref l, ref t, ref r, ref b,
                GeoIndex.Root,
                (w) => { return w.Shape.BoundsRect; });

            if (l > 0) l = 0;
            if (t > 0) t = 0;
            if (r < 0) r = 0;
            if (b < 0) b = 0;
            return RectangleI.FromLTRB((int)l,(int)t, (int) r, (int) b);
        }

       

        public override void Clear() {
            BoundsDirty = true;
            Bounds = RectangleI.Empty;
            GeoIndex = null;
        }
    }
}