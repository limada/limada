/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2012 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing;
using Limaki.Drawing.Gdi;
using Limaki.View.Gdi.UI;
using Limaki.View.Rendering;
using Xwt;
using Xwt.Gdi.Backend;

namespace Limaki.View.Swf {

    public class MoveResizeRenderer : MoveResizeRendererBase {

        public override GripPainterBase GripPainter {
            get {
                if (_gripPainter == null) {
                    _gripPainter = new GripPainter();
                }
                return base.GripPainter;
            }
            set { _gripPainter = value; }
        }

        protected System.Drawing.Drawing2D.Matrix emptyMatrix =
            new System.Drawing.Drawing2D.Matrix();

        public override void OnPaint(IRenderEventArgs e) {
            IShape shape = this.Shape;
            if ((shape != null) && (ShowGrips)) {
                var g = ((GdiSurface)e.Surface).Graphics;
                //save
                System.Drawing.Drawing2D.Matrix transform = g.Transform;
                //reset
                g.Transform = emptyMatrix;

                GripPainter.Render(e.Surface);
                //restore
                g.Transform = transform;
            }
        }

        bool useRegionForClipping = true;
        private System.Drawing.Region clipRegion = new System.Drawing.Region();

        public override void InvalidateShapeOutline(IShape oldShape, IShape newShape) {
            if (oldShape != null) {
                int halfborder = GripSize + 1;
                
                var backend = this.Backend as IGdiBackend;

                if (useRegionForClipping) {
                    lock (clipRegion) {
                        clipRegion.MakeInfinite();
                        var a = oldShape.BoundsRect;
                        var b = newShape.BoundsRect;

                        var smaller = DrawingExtensions.Intersect(a, b);
                        var bigger = DrawingExtensions.Union(a, b);
                        smaller = Camera.FromSource(smaller);
                        bigger = Camera.FromSource(bigger);

                        smaller = smaller.Inflate(-halfborder, -halfborder);
                        bigger = bigger.Inflate(halfborder, halfborder);

                        // this is a mono workaround, as it don't like strange rectangles:
                        smaller = smaller.NormalizedRectangle();
                        bigger = bigger.NormalizedRectangle();

                        // this is a mono workaround, as it don't like strange rectangles:
                        if (smaller.Size == Size.Zero) {
                            clipRegion.Intersect(bigger.ToGdi ());
                        } else {
                            clipRegion.Intersect(
                                Rectangle.FromLTRB(bigger.Left, bigger.Top, bigger.Right, smaller.Top).ToGdi ());
                            clipRegion.Union(
                                Rectangle.FromLTRB(bigger.Left, smaller.Bottom, bigger.Right, bigger.Bottom).ToGdi ());
                            clipRegion.Union(
                                Rectangle.FromLTRB(bigger.Left, smaller.Top, smaller.Left, smaller.Bottom).ToGdi ());
                            clipRegion.Union(
                                Rectangle.FromLTRB(smaller.Right, smaller.Top, bigger.Right, smaller.Bottom).ToGdi ());

                            //clipRegion.Intersect(smaller);
                            //clipRegion.Complement(bigger);
                        }
                        backend.Invalidate(clipRegion);
                    }
                } else {
                    // the have do redraw the oldShape and newShape area
                    var invalidRect = DrawingExtensions.Union(oldShape.BoundsRect, newShape.BoundsRect);
                    // transform rectangle to control coordinates
                    invalidRect = Camera.FromSource(invalidRect);

                    invalidRect = invalidRect.Inflate(halfborder, halfborder);
                    backend.Invalidate(invalidRect);
                }
            }
        }

        protected override void Dispose(bool disposing) {
            base.Dispose (disposing);
            if (disposing) {
                emptyMatrix.Dispose();
                emptyMatrix = null;
            }
        }



    }
}