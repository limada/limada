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


using System.Collections.Generic;
using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Drawing.Indexing.QuadTrees;
using Xwt;

namespace Limaki.Visuals {
    public class QuadTreeIndex:SpatialIndex {
        private Quadtree<IVisual> _geoIndex = null;
        public Quadtree<IVisual> GeoIndex {
            get {
                if (_geoIndex == null) {
                    _geoIndex = new Quadtree<IVisual>();
                }
                return _geoIndex;
            }
            set { _geoIndex = value; }
        }

        protected override void Add(Rectangle bounds, IVisual item) {
            if ( bounds != Rectangle.Zero )
                GeoIndex.Add (bounds, item);
        }
        protected override void Remove(Rectangle bounds, IVisual item) {
            if ( bounds != Rectangle.Zero )
                GeoIndex.Remove(bounds, item);
        }

        public override void AddRange(IEnumerable<IVisual> items) {
            base.AddRange (items);
        }

        public override IEnumerable<IVisual> Query( Rectangle clipBounds, ZOrder zOrder ) {
            IEnumerable<IVisual> search = GeoIndex.Query(clipBounds);

            if (zOrder==ZOrder.EdgesFirst) {
                foreach (var visual in search) {
                    if (visual is IVisualEdge) {
                        if (DrawingExtensions.Intersects(clipBounds, visual.Shape.BoundsRect))
                            yield return visual;
                    }
                }
            }

            foreach (var visual in search) {
                if (!(visual is IVisualEdge)) {
                    if (DrawingExtensions.Intersects(clipBounds, visual.Shape.BoundsRect))
                        yield return visual;
                }
            }

            if (zOrder==ZOrder.NodesFirst) {
                foreach (var visual in search) {
                    if (visual is IVisualEdge) {
                        if (DrawingExtensions.Intersects(clipBounds, visual.Shape.BoundsRect))
                            yield return visual;
                    }
                }
            }
        }

        public override IEnumerable<IVisual> Query(Rectangle clipBounds) {
            return Query (clipBounds, ZOrder.NodesFirst);
        }

        public override IEnumerable<IVisual> Query() {
            var search = GeoIndex.QueryAll ();

            foreach (var visual in search) {
                if (!(visual is IVisualEdge)) {
                        yield return visual;
                }
            }
            foreach (var visual in search) {
                if (visual is IVisualEdge) {
                        yield return visual;
                }
            }
        }

        class RightBottomCommand : Command<ICollection<IVisual>, Point> {
            public RightBottomCommand(ICollection<IVisual> target, Point parameter)
                : base(target, parameter) {
                Parameter = new Point(float.MinValue, float.MinValue);
            }
            public override void Execute() {
                var h = 0d;
                var w = 0d;
                foreach (var visual in Subject) {
                    var bounds = visual.Shape.BoundsRect;
                    var r = bounds.Right;
                    var b = bounds.Bottom;
                    if (r > w) w = r;
                    if (b > h) h = b;
                }
                Parameter = new Point(w, h);
            }
        }

        class LeftTopCommand : Command<ICollection<IVisual>, Point> {
            public LeftTopCommand(ICollection<IVisual> target, Point parameter)
                : base(target, parameter) {
                Parameter = new Point(float.MaxValue, float.MaxValue);
            }
            public override void Execute() {
                var x = 0d;
                var y = 0d;
                foreach (IVisual visual in Subject) {
                    var bounds = visual.Shape.BoundsRect;
                    var l = bounds.X;
                    var t = bounds.Y;
                    if (l < x)
                        x = l;
                    if (t < y)
                        y = t;
                }
                Parameter = new Point(x, y);
            }
        }

        /// <summary>
        /// not used!
        /// </summary>
        class BoundsVisitor : IItemVisitor<IVisual> {
            public Point Parameter;
            public void VisitItem(IVisual item) {
                var w = Parameter.X;
                var h = Parameter.Y;
                var bounds = item.Shape.BoundsRect;
                var r = bounds.Right;
                var b = bounds.Bottom;
                if (r > w)
                    Parameter.X = r;
                if (b > h)
                    Parameter.Y = b;
            }

            public Rectangle GetEnvelope(IVisual item) {
                return item.Shape.BoundsRect;
            }

            public bool ProvidesEnvelope { get { return true; } }

        }

        protected override Rectangle CalculateBounds() {
            // remark: the starting values could be used to 
            // opimize further if boundsDirty is more refined;
            // eg: leftDirty leads to : 
            // l = float.MaxValue, t = float.MinValue, r = float.maxvalue, b = float.maxValue

            var l = double.MaxValue; var t = double.MaxValue;
            var r = double.MinValue; var b = double.MinValue;
            
            GeoIndex.QueryBounds(
                ref l, ref t, ref r, ref b,
                GeoIndex.Root,
                (visual) => { return visual.Shape.BoundsRect; });

            if (l > 0) l = 0;
            if (t > 0) t = 0;
            if (r < 0) r = 0;
            if (b < 0) b = 0;
            return Rectangle.FromLTRB((int)l,(int)t, (int) r, (int) b);
        }

       

        public override void Clear() {
            BoundsDirty = true;
            Bounds = Rectangle.Zero;
            GeoIndex = null;
        }
    }
}