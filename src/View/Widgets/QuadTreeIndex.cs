/*
 * Limaki 
 * Version 0.071
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
using System.Drawing;
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

        protected override void Add(Rectangle bounds, IWidget item) {
            if ( bounds != Rectangle.Empty )
                GeoIndex.Add (bounds, item);
        }
        protected override void Remove(Rectangle bounds, IWidget item) {
            if ( bounds != Rectangle.Empty )
                GeoIndex.Remove(bounds, item);
        }

        public override void Fill(IEnumerable<IWidget> items) {
            base.Fill (items);
        }

        public override IEnumerable<IWidget> Query(RectangleF clipBounds) {
            IEnumerable<IWidget> search = GeoIndex.Query(clipBounds);

            foreach (IWidget widget in search) {
                if (!(widget is IEdgeWidget)) {
                    //RectangleF bounds = widget.Shape.BoundsRect;
                    //bounds.Inflate(1, 1);
                    //if (clipBounds.IntersectsWith(bounds))
                    if (ShapeUtils.Intersects(clipBounds,widget.Shape.BoundsRect))
                        yield return widget;
                }
            }
            foreach (IWidget widget in search) {
                if (widget is IEdgeWidget) {
                    //Rectangle bounds = widget.Shape.BoundsRect;
                    //bounds.Inflate(1, 1);
                    //if (clipBounds.IntersectsWith(bounds))
                    if ( ShapeUtils.Intersects(clipBounds, widget.Shape.BoundsRect) )
                        yield return widget;
                }
            }
        }

        class BoundsCommand : Command<ICollection<IWidget>, PointF> {
            public BoundsCommand(ICollection<IWidget> target, PointF parameter)
                : base(target, parameter) {
                Parameter = new PointF(float.MinValue, float.MinValue);
            }
            public override void Execute() {
                int h = 0;
                int w = 0;
                foreach (IWidget widget in Target) {
                    Rectangle bounds = widget.Shape.BoundsRect;
                    int r = bounds.Right;
                    int b = bounds.Bottom;
                    if (r > w) w = r;
                    if (b > h) h = b;
                }
                Parameter = new PointF(w, h);
            }
        }
        
        class BoundsVisitor:IItemVisitor<IWidget> {
            #region IItemVisitor<IWidget> Member

            public PointF Parameter;
            public void VisitItem(IWidget item) {
                int w = (int)Parameter.X;
                int h = (int)Parameter.Y;
                Rectangle bounds = item.Shape.BoundsRect;
                int r = bounds.Right;
                int b = bounds.Bottom;
                if (r > w) Parameter.X = r;
                if (b > h) Parameter.Y = b;
            }

            public RectangleF GetEnvelope(IWidget item) {
                return item.Shape.BoundsRect;
            }

            public bool ProvidesEnvelope { get { return true; } }

            #endregion
        }

        protected override Rectangle CalculateBounds() {
            PointF rightBottom = GeoIndex.QueryRightBottom(new BoundsCommand(null, default(PointF)));
            return new Rectangle(0, 0, (int)rightBottom.X, (int)rightBottom.Y);
        }

        public override void Clear() {
            BoundsDirty = true;
            Bounds = Rectangle.Empty;
            GeoIndex = null;
        }
    }
}