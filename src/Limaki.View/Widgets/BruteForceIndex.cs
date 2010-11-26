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


using System;
using System.Collections.Generic;
using Limaki.Drawing;

namespace Limaki.Widgets {
    public class BruteForceIndex : SpatialIndex {
        IEnumerable<IWidget> items = null;
        public override void Fill(IEnumerable<IWidget> items) {
            this.items = items;
        }

        protected override RectangleI CalculateBounds() {
            int l = 0;int t = 0;
            int r = 0;int b = 0;
            
            
            foreach (IWidget widget in items) {
                RectangleI env = widget.Shape.BoundsRect;
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
            return RectangleI.FromLTRB(l, t, r, b);
        }

        public override IEnumerable<IWidget> Query(RectangleS clipBounds) {
            return Query(clipBounds, ZOrder.NodesFirst);

        }

        public override IEnumerable<IWidget> Query( RectangleS clipBounds, ZOrder zOrder ) {
            if (zOrder==ZOrder.EdgesFirst)
                foreach (IWidget widget in items) {
                    if (widget is IEdgeWidget) {
                        RectangleI bounds = widget.Shape.BoundsRect;
                        bounds.Inflate(1, 1);
                        if (clipBounds.IntersectsWith(bounds))
                            yield return widget;
                    }
                }
            foreach (IWidget widget in items) {
                if (!(widget is IEdgeWidget)) {
                    RectangleI bounds = widget.Shape.BoundsRect;
                    bounds.Inflate(1, 1);
                    if (clipBounds.IntersectsWith(bounds))
                        yield return widget;
                }
            }
            if (zOrder==ZOrder.NodesFirst)
                foreach (IWidget widget in items) {
                    if (widget is IEdgeWidget) {
                        RectangleI bounds = widget.Shape.BoundsRect;
                        bounds.Inflate(1, 1);
                        if (clipBounds.IntersectsWith(bounds))
                            yield return widget;
                    }
                }
        }
        public override IEnumerable<IWidget> Query() {
            foreach (IWidget widget in items) {
                if (!(widget is IEdgeWidget)) {
                    yield return widget;
                }
            }
            foreach (IWidget widget in items) {
                if (widget is IEdgeWidget) {
                    yield return widget;
                }
            }
        }

        public override void Clear() {
            BoundsDirty = true;
            Bounds = RectangleI.Empty;
            items = null;
        }
        protected override void Add(RectangleI bounds, IWidget item) {}

        protected override void Remove(RectangleI bounds, IWidget item) { }
    }
}