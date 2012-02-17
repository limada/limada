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
using Limaki.Drawing;
using Limaki.Actions;
using Limaki.Drawing.Indexing.QuadTrees;
using Limaki.Drawing.Shapes;
using System;

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

        protected override void Add(RectangleI bounds, IVisual item) {
            if ( bounds != RectangleI.Empty )
                GeoIndex.Add (bounds, item);
        }
        protected override void Remove(RectangleI bounds, IVisual item) {
            if ( bounds != RectangleI.Empty )
                GeoIndex.Remove(bounds, item);
        }

        public override void AddRange(IEnumerable<IVisual> items) {
            base.AddRange (items);
        }

        public override IEnumerable<IVisual> Query( RectangleS clipBounds, ZOrder zOrder ) {
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

        public override IEnumerable<IVisual> Query(RectangleS clipBounds) {
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

        class RightBottomCommand : Command<ICollection<IVisual>, PointS> {
            public RightBottomCommand(ICollection<IVisual> target, PointS parameter)
                : base(target, parameter) {
                Parameter = new PointS(float.MinValue, float.MinValue);
            }
            public override void Execute() {
                int h = 0;
                int w = 0;
                foreach (var visual in Subject) {
                    var bounds = visual.Shape.BoundsRect;
                    int r = bounds.Right;
                    int b = bounds.Bottom;
                    if (r > w) w = r;
                    if (b > h) h = b;
                }
                Parameter = new PointS(w, h);
            }
        }

        class LeftTopCommand : Command<ICollection<IVisual>, PointS> {
            public LeftTopCommand(ICollection<IVisual> target, PointS parameter)
                : base(target, parameter) {
                Parameter = new PointS(float.MaxValue, float.MaxValue);
            }
            public override void Execute() {
                int x = 0;
                int y = 0;
                foreach (IVisual visual in Subject) {
                    var bounds = visual.Shape.BoundsRect;
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
        class BoundsVisitor : IItemVisitor<IVisual> {
            public PointS Parameter;
            public void VisitItem(IVisual item) {
                int w = (int)Parameter.X;
                int h = (int)Parameter.Y;
                var bounds = item.Shape.BoundsRect;
                int r = bounds.Right;
                int b = bounds.Bottom;
                if (r > w)
                    Parameter.X = r;
                if (b > h)
                    Parameter.Y = b;
            }

            public RectangleS GetEnvelope(IVisual item) {
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
                (visual) => { return visual.Shape.BoundsRect; });

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