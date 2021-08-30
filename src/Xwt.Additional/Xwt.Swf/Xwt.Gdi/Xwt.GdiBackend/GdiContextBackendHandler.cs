// 
// GdiContextBackendHandler.cs
//  
// Author:
//       Lytico 
// 
// Copyright (c) 2012 Lytico (www.limada.org)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;

using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Xwt.Backends;
using Xwt.Drawing;
using SD = System.Drawing;

namespace Xwt.GdiBackend {

    public class GdiContextBackendHandler : ContextBackendHandler {

        /// <summary>
        /// Makes a copy of the current state of the Context and saves it on an internal stack of saved states.
        /// When Restore() is called, it will be restored to the saved state. 
        /// Multiple calls to Save() and Restore() can be nested; 
        /// each call to Restore() restores the state from the matching paired save().
        /// </summary>
        public override void Save (object backend) {
            var gc = (GdiContext) backend;
            gc.Save ();
        }

        public override void Restore (object backend) {
            var gc = (GdiContext) backend;
            gc.Restore ();
        }

        // http://cairographics.org/documentation/cairomm/reference/classCairo_1_1Context.html

        // mono-libgdiplus\src\graphics-cairo.c

        /// <summary>
        /// Adds a circular arc of the given radius to the current path.
        /// The arc is centered at (xc, yc), 
        /// begins at angle1 and proceeds in the direction of increasing angles to end at angle2. 
        /// If angle2 is less than angle1 
        /// it will be progressively increased by 2*M_PI until it is greater than angle1.
        /// If there is a current point, an initial line segment will be added to the path 
        /// to connect the current point to the beginning of the arc. 
        /// If this initial line is undesired, 
        /// it can be avoided by calling begin_new_sub_path() before calling arc().
        /// </summary>
        /// <param name="backend"></param>
        /// <param name="xc"></param>
        /// <param name="yc"></param>
        /// <param name="radius"></param>
        /// <param name="angle1"></param>
        /// <param name="angle2"></param>
        public override void Arc (object backend, double xc, double yc, double radius, double angle1, double angle2) {
            var c = (GdiContext) backend;
            if (radius == 0)
                return;
            if (angle1 > 0 && angle2 == 0)
                angle2 = 360;
            // GraphicsPath.AddArc sweepAngle:The angle between startAngle and the end of the arc. 
            c.Path.AddArc ((float) (xc - radius), (float) (yc - radius),
                           (float) radius * 2, (float) radius * 2,
                           (float) angle1, (float) (angle2 - angle1));
            if (c.Path.PointCount != 0)
                c.Current = c.Path.GetLastPoint ();

        }

        /// <summary>
        /// Establishes a new clip region by intersecting the current clip region with the current Path 
        /// as it would be filled by fill() and according to the current fill rule.
        /// After clip(), the current path will be cleared from the Context.
        /// The current clip region affects all drawing operations by effectively masking out any changes to the surface 
        /// that are outside the current clip region.
        /// Calling clip() can only make the clip region smaller, never larger. 
        /// But the current clip is part of the graphics state, 
        /// so a temporary restriction of the clip region can be achieved by calling clip() within a save()/restore() pair. 
        /// The only other means of increasing the size of the clip region is reset_clip().
        /// </summary>
        public override void Clip (object backend) {
            ClipPreserve (backend);
            var gc = (GdiContext) backend;
            gc.Path.Dispose ();
            gc.Path = new GraphicsPath ();
        }

        public override void ClipPreserve (object backend) {
            var gc = (GdiContext) backend;
            gc.Graphics.Clip = new SD.Region (gc.Path);
        }

        public void ResetClip (object backend) {
            var gc = (GdiContext) backend;
            gc.Graphics.ResetClip ();
        }

        public override void ClosePath (object backend) {
            var gc = (GdiContext) backend;
            if (gc.Path.PointCount != 0)
                gc.CloseFigure ();
        }

        /// <summary>
        /// Adds a cubic Bezier spline to the path from the current point to position (x3, y3) in user-space coordinates, 
        /// using (x1, y1) and (x2, y2) as the control points. 
        /// </summary>
        public override void CurveTo (object backend, double x1, double y1, double x2, double y2, double x3, double y3) {
            var gc = (GdiContext) backend;
            gc.Path.AddBezier (
                (float) gc.Current.X, (float) gc.Current.Y,
                (float) x1, (float) y1,
                (float) x2, (float) y2,
                (float) x3, (float) y3);
            gc.Current = new SD.PointF ((float) x3, (float) y3);
        }

        public override void Fill (object backend) {
            FillPreserve (backend);
            var gc = (GdiContext) backend;
            gc.Path = null;
        }

        public override void FillPreserve (object backend) {
            var gc = (GdiContext) backend;
            gc.Transform ();
            gc.Graphics.FillPath (gc.Brush, gc.Path);
        }

        public override void LineTo (object backend, double x, double y) {
            var gc = (GdiContext) backend;
            var dest = new SD.PointF ((float) x, (float) y);
            gc.Path.AddLine (gc.Current, dest);

            gc.Current = dest;
        }

        /// <summary>
        /// If the current subpath is not empty, begin a new subpath.
        /// After this call the current point will be (x, y).
        /// </summary>
        /// <param name="backend"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public override void MoveTo (object backend, double x, double y) {
            var gc = (GdiContext) backend;

            if (gc.Current.X != x || gc.Current.Y != y) {
                gc.Path.StartFigure ();
                gc.Current = new SD.PointF ((float) x, (float) y);
            }

        }

        public override void NewPath (object backend) {
            var gc = (GdiContext) backend;
            gc.NewPath ();
            
        }

        public override void Rectangle (object backend, double x, double y, double width, double height) {
            var gc = (GdiContext) backend;
            if (gc.Current.X != x || gc.Current.Y != y) {
                gc.Path.StartFigure ();
                gc.Current = new SD.PointF ((float) x, (float) y);
            }
            if (width <= 0 && height <= 0)
                return;
            gc.Path.AddRectangle (new SD.RectangleF ((float) x, (float) y, (float) width, (float) height));

        }

        /// <summary>
        /// Relative-coordinate version of curve_to().
        /// All offsets are relative to the current point. 
        /// Adds a cubic Bezier spline to the path from the current point to a point offset 
        /// from the current point by (dx3, dy3), using points offset by (dx1, dy1) and (dx2, dy2) 
        /// as the control points. After this call the current point will be offset by (dx3, dy3).
        /// Given a current point of (x, y), RelCurveTo(dx1, dy1, dx2, dy2, dx3, dy3)
        /// is logically equivalent to CurveTo(x + dx1, y + dy1, x + dx2, y + dy2, x + dx3, y + dy3).
        /// </summary>
        public override void RelCurveTo (object backend, double dx1, double dy1, double dx2, double dy2, double dx3, double dy3) {
            var gc = (GdiContext) backend;
            RelCurveTo (backend,
                gc.Current.X + dx1, gc.Current.Y + dy1,
                gc.Current.X + dx2, gc.Current.Y + dy2,
                gc.Current.X + dx3, gc.Current.Y + dy3);
        }

        /// <summary>
        /// Adds a line to the path from the current point to a point that 
        /// is offset from the current point by (dx, dy) in user space. 
        /// After this call the current point will be offset by (dx, dy).
        /// Given a current point of (x, y), 
        /// RelLineTo(dx, dy) is logically equivalent to LineTo(x + dx, y + dy).
        /// </summary>
        public override void RelLineTo (object backend, double dx, double dy) {
            var gc = (GdiContext) backend;
            LineTo (backend, gc.Current.X + dx, gc.Current.Y + dy);
        }

        /// <summary>
        /// If the current subpath is not empty, begin a new subpath.
        /// After this call the current point will offset by (x, y).
        /// Given a current point of (x, y), 
        /// RelMoveTo(dx, dy) is logically equivalent to MoveTo(x + dx, y + dy).
        /// </summary>
        public override void RelMoveTo (object backend, double dx, double dy) {
            var gc = (GdiContext) backend;
            gc.Current = new SD.PointF ((float) (gc.Current.X + dx), (float) (gc.Current.Y + dy));
        }

        public override void Stroke (object backend) {
            var gc = (GdiContext) backend;
            gc.Transform ();
            gc.Graphics.DrawPath (gc.Pen, gc.Path);
            gc.Path = null;

        }

        public override void StrokePreserve (object backend) {
            var gc = (GdiContext) backend;
            gc.Transform ();
            gc.Graphics.DrawPath (gc.Pen, gc.Path);
        }

        public override void SetColor (object backend, Xwt.Drawing.Color color) {
            var gc = (GdiContext) backend;
            gc.Color = GdiConverter.ToGdi (color);
        }

        public override void SetLineWidth (object backend, double width) {
            var gc = (GdiContext) backend;
            gc.LineWidth = width;
        }
        /// <summary>
        /// Sets the dash pattern to be used by stroke().
        /// A dash pattern is specified by dashes, an array of positive values. 
        /// Each value provides the user-space length of altenate "on" and "off" portions of the stroke. 
        /// The offset specifies an offset into the pattern at which the stroke begins.
        /// If dashes is empty dashing is disabled. If the size of dashes is 1, 
        /// a symmetric pattern is assumed with alternating on and off portions of the size specified by the single value in dashes.
        /// It is invalid for any value in dashes to be negative, or for all values to be 0. 
        /// If this is the case, an exception will be thrown
        /// </summary>
        public override void SetLineDash (object backend, double offset, params double[] pattern) {
            var gc = (GdiContext) backend;
            if (pattern.Length != 0) {
                gc.Pen.DashOffset = (float) (offset / gc.LineWidth);
                var fp = new float[pattern.Length];
                for (int i = 0; i < fp.Length; ++i)
                    fp[i] = (float) (pattern[i] / gc.LineWidth);
                gc.Pen.DashStyle = DashStyle.Custom;
                gc.Pen.DashPattern = fp;
            } else {
                gc.Pen.DashStyle = DashStyle.Solid;
            }
        }

        public override void SetPattern (object backend, object p) {
            var gc = (GdiContext) backend;
            gc.Brush = (SD.TextureBrush) p;
        }

        //public void SetFont (object backend, Xwt.Drawing.Font font) {
        //    var gc = (GdiContext) backend;
        //    gc.Font = font.ToGdi ();
        //}

        public override void DrawTextLayout (object backend, Xwt.Drawing.TextLayout layout, double x, double y) {

            if (layout.Text == null)
                return;

            var gc = (GdiContext) backend;
            var tl = (GdiTextLayoutBackend)Toolkit.GetBackend(layout);
            var font = tl.Font.ToGdi ();
            var w = layout.Width;
            var h = layout.Height;
            SD.Drawing2D.GraphicsPath path = null;
            var onPath = (gc.ScaledRotated (gc.Graphics.Transform) || gc.ScaledOrRotated);

            if (w != -1 || h != -1) {
                var size = tl.Size;
                w = w == -1 ? size.Width : w;
                h = h == -1 ? size.Height : h;

                var rect = new System.Drawing.RectangleF ((float) x, (float) y, (float) w, (float) h);
                if (onPath) {
                    path = gc.TextLayoutPath (font, (p, fontSize) =>
                            p.AddString (tl.Text, font.FontFamily, (int) font.Style, fontSize, rect, tl.Format));

                } else {
                    gc.Graphics.DrawString (tl.Text, font, gc.Brush, rect, tl.Format);
                }
            } else {
                var at = new System.Drawing.PointF ((float) x, (float) y);
                if (onPath) {
                    path = gc.TextLayoutPath (font, (p, fontSize) =>
                            p.AddString (tl.Text, font.FontFamily, (int) font.Style, fontSize, at, tl.Format));
                } else {
                    gc.Graphics.DrawString (tl.Text, font, gc.Brush, at, tl.Format);
                }
            }
            if (path != null) {
                var s = gc.Graphics.SetQuality (GdiConverter.DrawTextHighQuality);

                var brush = new SD.SolidBrush (gc.Color);
                gc.Graphics.FillPath (brush, path);
                brush.Dispose ();

                path.Dispose ();
                gc.Graphics.SetQuality (s);
            }

        }

        protected ImageAttributes ImgAttributes (ImageDescription img) {
            var alpha = img.Alpha * _globalAlpha;
            var clrMatrix = new ColorMatrix (new float[][] {
                            new float[] { 1.0f, 0.0f, 0.0f, 0.0f, 0.0f },
                            new float[] { 0.0f, 1.0f, 0.0f, 0.0f, 0.0f },
                            new float[] { 0.0f, 0.0f, 1.0f, 0.0f, 0.0f },
                            new float[] { 0.0f, 0.0f, 0.0f, (float) alpha, 0.0f },
                            new float[] { 0.0f, 0.0f, 0.0f, 0.0f, 1.0f }
                        });
            var imageAttributes = new ImageAttributes ();
            imageAttributes.SetColorMatrix (clrMatrix);
            return imageAttributes;
        }

        public override void DrawImage (object backend, ImageDescription img, double x, double y) {
            var context = (GdiContext)backend;
            var image = ((GdiImage) img.Backend).Image;
            var q = context.Graphics.SetQuality(GdiConverter.DrawHighQuality);
            if (img.Alpha >= 1)
                context.Graphics.DrawImage (image, (float) x, (float) y);
            else {
                var size = img.Size;
                context.Graphics.DrawImage (image, new SD.Rectangle ((int)x, (int)y, (int)size.Width, (int)size.Height),
                    0f, 0f, (float)size.Width, (float)size.Height, SD.GraphicsUnit.Pixel, ImgAttributes (img));
            }
            context.Graphics.SetQuality(q);
        }

        public override void DrawImage (object backend, ImageDescription img, Rectangle srcRect, Rectangle destRect) {
            var context = (GdiContext)backend;
            var image = ((GdiImage)img.Backend).Image;
            
            var q = context.Graphics.SetQuality(GdiConverter.DrawHighQuality);
            if (img.Alpha >= 1)
                context.Graphics.DrawImage (image, destRect.ToGdi (), srcRect.ToGdi (), SD.GraphicsUnit.Pixel);
            else 
                context.Graphics.DrawImage (image, destRect.ToGdi (),
                    (float)srcRect.X, (float)srcRect.Y, (float)srcRect.Width, (float)srcRect.Height, 
                    SD.GraphicsUnit.Pixel, ImgAttributes (img));

            context.Graphics.SetQuality(q);
        }

        public override void Rotate (object backend, double angle) {
            var gc = (GdiContext) backend;
            gc.Rotate ((float) angle);
            //if (gc.Path.PointCount != 0) {
            //    gc.Current = gc.Path.GetLastPoint ();
            //} 
        }

        public override void Scale (object backend, double scaleX, double scaleY) {
            var context = (GdiContext) backend;
            context.Scale ((float) scaleX, (float) scaleY);
        }

        public override double GetScaleFactor (object backend) {
            var gc = (GdiContext) backend;
            return gc.ScaleFactor;
        }

        public override void Translate (object backend, double tx, double ty) {
            var gc = (GdiContext) backend;
            gc.Translate ((float) tx, (float) ty);
        }

        //public void ResetTransform (object backend) {
        //    var gc = (GdiContext) backend;
        //    gc.ResetTransform ();
        //}

        public override void Dispose (object backend) {
            var gc = (GdiContext) backend;
            gc.Dispose ();
        }


        private double _globalAlpha = 1d;
        public override void SetGlobalAlpha (object backend, double globalAlpha) {
            _globalAlpha = globalAlpha;
        }

        public override void ModifyCTM (object backend, Drawing.Matrix transform) {
            var gc = (GdiContext)backend;
            gc.Matrix.Multiply(transform.ToGdi());
        }

        public override Drawing.Matrix GetCTM (object backend) {
            var gc = (GdiContext)backend;
            return gc.Matrix.ToXwt ();
        }

        public override bool IsPointInStroke (object backend, double x, double y) {
            throw new NotImplementedException();
        }

        public override bool IsPointInFill (object backend, double x, double y) {
            throw new NotImplementedException();
        }

        public override void ArcNegative (object backend, double xc, double yc, double radius, double angle1, double angle2) {
            throw new NotImplementedException();
        }

        public override object CreatePath () {
            throw new NotImplementedException();
        }

        public override object CopyPath (object backend) {
            throw new NotImplementedException();
        }

        public override void AppendPath (object backend, object otherBackend) {
            throw new NotImplementedException();
        }

        public override void SetStyles(object backend, StyleSet styles)
        {
            throw new NotImplementedException();
        }
    }
}