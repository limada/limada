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
using Xwt;

namespace Limaki.Visuals {
    public class BruteForceIndex : SpatialIndex {
        protected IEnumerable<IVisual> Visuals { get; set; }
        public override void AddRange(IEnumerable<IVisual> items) {
            this.Visuals = items;
        }

        protected override RectangleD CalculateBounds() {
            var l = 0d;var t = 0d;
            var r = 0d;var b = 0d;
            
            
            foreach (var visual in Visuals) {
                var env = visual.Shape.BoundsRect;
                var envX = env.X;
                var envY = env.Y;
                var envR = env.Right;
                var envB = env.Bottom;
                if (envX < l) l = envX;
                if (envY < t) t = envY;
                if (envR > r) r = envR;
                if (envB > b) b = envB;
            }
            if (l > 0) l = 0;
            if (t > 0) t = 0;
            if (r < 0) r = 0;
            if (b < 0) b = 0;
            return RectangleD.FromLTRB(l, t, r, b);
        }

        public override IEnumerable<IVisual> Query(RectangleD clipBounds) {
            return Query(clipBounds, ZOrder.NodesFirst);

        }

        public override IEnumerable<IVisual> Query( RectangleD clipBounds, ZOrder zOrder ) {
            if (zOrder==ZOrder.EdgesFirst)
                foreach (var visual in Visuals) {
                    if (visual is IVisualEdge) {
                        RectangleD bounds = visual.Shape.BoundsRect;
                        bounds.Inflate(1, 1);
                        if (clipBounds.IntersectsWith(bounds))
                            yield return visual;
                    }
                }
            foreach (var visual in Visuals) {
                if (!(visual is IVisualEdge)) {
                    RectangleD bounds = visual.Shape.BoundsRect;
                    bounds.Inflate(1, 1);
                    if (clipBounds.IntersectsWith(bounds))
                        yield return visual;
                }
            }
            if (zOrder==ZOrder.NodesFirst)
                foreach (var visual in Visuals) {
                    if (visual is IVisualEdge) {
                        RectangleD bounds = visual.Shape.BoundsRect;
                        bounds.Inflate(1, 1);
                        if (clipBounds.IntersectsWith(bounds))
                            yield return visual;
                    }
                }
        }
        public override IEnumerable<IVisual> Query() {
            foreach (var visual in Visuals) {
                if (!(visual is IVisualEdge)) {
                    yield return visual;
                }
            }
            foreach (var visual in Visuals) {
                if (visual is IVisualEdge) {
                    yield return visual;
                }
            }
        }

        public override void Clear() {
            BoundsDirty = true;
            Bounds = RectangleD.Zero;
            Visuals = null;
        }
        protected override void Add(RectangleD bounds, IVisual item) {}

        protected override void Remove(RectangleD bounds, IVisual item) { }
    }
}