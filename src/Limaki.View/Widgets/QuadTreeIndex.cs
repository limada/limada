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
using Limaki.Drawing;
using Limaki.Actions;
using Limaki.Drawing.Indexing.QuadTrees;
using Limaki.Drawing.Shapes;

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

        public override void Fill(IEnumerable<IWidget> items) {
            base.Fill (items);
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

        class BoundsCommand : Command<ICollection<IWidget>, PointS> {
            public BoundsCommand(ICollection<IWidget> target, PointS parameter)
                : base(target, parameter) {
                Parameter = new PointS(float.MinValue, float.MinValue);
            }
            public override void Execute() {
                int h = 0;
                int w = 0;
                foreach (IWidget widget in Target) {
                    RectangleI bounds = widget.Shape.BoundsRect;
                    int r = bounds.Right;
                    int b = bounds.Bottom;
                    if (r > w) w = r;
                    if (b > h) h = b;
                }
                Parameter = new PointS(w, h);
            }
        }
        
        class BoundsVisitor:IItemVisitor<IWidget> {
            #region IItemVisitor<IWidget> Member

            public PointS Parameter;
            public void VisitItem(IWidget item) {
                int w = (int)Parameter.X;
                int h = (int)Parameter.Y;
                RectangleI bounds = item.Shape.BoundsRect;
                int r = bounds.Right;
                int b = bounds.Bottom;
                if (r > w) Parameter.X = r;
                if (b > h) Parameter.Y = b;
            }

            public RectangleS GetEnvelope(IWidget item) {
                return item.Shape.BoundsRect;
            }

            public bool ProvidesEnvelope { get { return true; } }

            #endregion
        }

        protected override RectangleI CalculateBounds() {
            PointS rightBottom = GeoIndex.QueryRightBottom(new BoundsCommand(null, default(PointS)));
            return new RectangleI(0, 0, (int)rightBottom.X, (int)rightBottom.Y);
        }

        public override void Clear() {
            BoundsDirty = true;
            Bounds = RectangleI.Empty;
            GeoIndex = null;
        }
    }
}