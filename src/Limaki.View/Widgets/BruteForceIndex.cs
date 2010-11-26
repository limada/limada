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
            int h = 0;
            int w = 0;
            foreach (IWidget widget in items) {
                RectangleI bounds = widget.Shape.BoundsRect;
                int r = bounds.Right;
                int b = bounds.Bottom;
                if (r > w) w = r;
                if (b > h) h = b;
            }
            return new RectangleI(0, 0, w, h);
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