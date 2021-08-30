﻿// 
// ReferencePainter.cs
//  
// Authors:
//       Lluis Sanchez <lluis@xamarin.com>
//       Lytico (www.limada.org)
// 
// Copyright (c) 2012 Xamarin Inc
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
using Xwt.Drawing;
using Xwt;
using System.Diagnostics;

namespace Xwt.Tests {
    public class ReferencePainter {
        public virtual void Figures (Context ctx, double x, double y) {
            Rectangles(ctx, x, y);
            Curves1(ctx, x, y + 60);
            Curves2(ctx, x + 100, y + 60);

        }
        public virtual void SimpleRectangles (Context ctx, double x, double y) {
            ctx.Save();
            ctx.Translate(x, y);

            // Simple rectangles

            ctx.SetLineWidth(1);
            ctx.Rectangle(0, 0, 10, 10);
            ctx.SetColor(Colors.Black);
            ctx.Fill();

            ctx.Rectangle(15, 0, 10, 10);
            ctx.SetColor(Colors.Black);
            ctx.Stroke();

            ctx.SetLineWidth(3);
            ctx.Rectangle(0, 15, 10, 10);
            ctx.SetColor(Colors.Black);
            ctx.Fill();

            ctx.Rectangle(15, 15, 10, 10);
            ctx.SetColor(Colors.Black);
            ctx.Stroke();

            ctx.Restore();
        }

        public virtual void RectangleWithHole (Context ctx, double x, double y) {
            // Rectangle with hole
            ctx.Save();
            ctx.Translate(x, y);

            ctx.Rectangle(0, 0, 40, 40);
            ctx.MoveTo(35, 35);
            ctx.RelLineTo(0, -20);
            ctx.RelLineTo(-20, 0);
            ctx.RelLineTo(0, 20);
            ctx.ClosePath();
            ctx.SetColor(Colors.Black);
            ctx.Fill();

            ctx.Restore();
        }

        public virtual void RoundedRectangle (Context ctx, double x, double y) {
            // Rounded Rectangle with Arcs
            ctx.Save();
            ctx.Translate(x, y);

            var r = 5;
            var l = 0;
            var t = 0;
            var w = 50;
            var h = 30;

            // top left  
            ctx.Arc(l + r, t + r, r, 180, 270);
            // top right 
            ctx.Arc(l + w - r, t + r, r, 270, 0);
            // bottom right  
            ctx.Arc(l + w - r, t + h - r, r, 0, 90);
            // bottom left 
            ctx.Arc(l + r, t + h - r, r, 90, 180);

            ctx.ClosePath();
            ctx.SetColor(Colors.AntiqueWhite);
            ctx.FillPreserve();
            ctx.SetColor(Colors.Black);
            ctx.Stroke();

            ctx.Restore();
        }

        public virtual void Rectangles (Context ctx, double x, double y) {
            SimpleRectangles(ctx, x, y);

            RectangleWithHole(ctx, x + 50, y);
            RoundedRectangle(ctx, x + 120, y);

        }

        public virtual void Curves1 (Context ctx, double x, double y) {
            ctx.Save();
            ctx.Translate(x, y);

            ctx.SetLineWidth(1);
            Action curve1 = () => {
                ctx.MoveTo(0, 30);
                ctx.CurveTo(20, 0, 50, 0, 60, 25);
            };
            // curve2 with lineTo; curve1 is closed
            Action curve2 = () => {
                ctx.LineTo(0, 0);
                ctx.CurveTo(20, 30, 50, 30, 60, 5);
            };
            Action paint = () => {
                curve1();
                curve2();
                ctx.ClosePath();
                ctx.SetColor(new Color(0, 0, 0, .5));
                ctx.StrokePreserve();
                ctx.SetColor(new Color(1, 0, 1, .5));
                ctx.Fill();
            };
            paint();

            ctx.Translate(0, 40);
            // curve2 with moveTo; curve1 is open
            curve2 = () => {
                ctx.MoveTo(0, 0);
                ctx.CurveTo(20, 30, 50, 30, 60, 5);
            };
            paint();
            ctx.Restore();

            //Todo: same stuff with arc
        }

        public virtual void Curves2 (Context ctx, double sx, double sy) {
            ctx.Save();
            ctx.Translate(sx, sy);
            ctx.SetColor(Colors.Black);

            double x = 0, y = 40;
            double x1 = y - x, y1 = x1 + y, x2 = x + y, y2 = x, x3 = y1, y3 = y;

            ctx.MoveTo(x, y);
            ctx.CurveTo(x1, y1, x2, y2, x3, y3);

            ctx.SetLineWidth(2.0);
            ctx.Stroke();

            ctx.SetColor(new Color(1, 0.2, 0.2, 0.6));
            ctx.SetLineWidth(1.0);
            ctx.MoveTo(x, y);
            ctx.LineTo(x1, y1);
            ctx.MoveTo(x2, y2);
            ctx.LineTo(x3, y3);
            ctx.Stroke();

            ctx.Restore();
        }

        public void PatternsAndImages (Context ctx, double x, double y) {
            ctx.Save();
            ctx.Translate(x, y);

            ctx.SetColor(Colors.Black);
            // Dashed lines

            ctx.SetLineDash(15, 10, 10, 5, 5);
            ctx.Rectangle(10, 10, 100, 100);
            ctx.Stroke();
            ctx.SetLineDash(0);

            // Image
            var arcColor = new Color(1, 0, 1);
            var ib = new ImageBuilder(30, 30);
            ib.Context.Arc(15, 15, 15, 0, 360);
            ib.Context.SetColor(arcColor);
            ib.Context.Fill();
            ib.Context.SetColor(Colors.DarkKhaki);
            ib.Context.Rectangle(0, 0, 5, 5);
            ib.Context.Fill();

            var img = ib.ToBitmap();
            ctx.DrawImage(img, 0, 0);
			ctx.DrawImage(img, 0, 50, 50, 10);

            ctx.Arc(100, 100, 15, 0, 360);
            arcColor.Alpha = 0.4;
            ctx.SetColor(arcColor);
            ctx.Fill();

            ctx.Restore();

            // ImagePattern

            ctx.Save();

            ctx.Translate(x + 130, y);
            ctx.Pattern = new ImagePattern(img);
            ctx.Rectangle(10, 10, 100, 100);
            ctx.Fill();

            ctx.Restore();

        }

        public virtual void Texts (Xwt.Drawing.Context ctx, double x, double y) {
            ctx.Save();

			var m = ctx.GetCTM ();
			var scaleX = m.M11;
			var scaleY = m.M22;

            ctx.Translate(x, y);

            ctx.SetColor(Colors.Black);

            var col1 = new Rectangle();
            var col2 = new Rectangle();

            var text = new TextLayout(ctx);
            text.Font = this.Font.WithSize(10);

            // first text
            text.Text = "Lorem ipsum dolor sit amet,";
            var size1 = text.GetSize();
            col1.Width = size1.Width;
            col1.Height += size1.Height + 10;
            ctx.DrawTextLayout(text, 0, 0);

            // proofing width; test should align with text above
            ctx.SetColor(Colors.DarkMagenta);
            text.Text = "consetetur sadipscing elitr, sed diam nonumy";
            text.Width = col1.Width;
            var size2 = text.GetSize();

            ctx.DrawTextLayout(text, 0, col1.Bottom);
            col1.Height += size2.Height + 10;

            ctx.SetColor(Colors.Black);

            // proofing scale, on second col
            ctx.Save();
            ctx.SetColor(Colors.Red);
            col2.Left = col1.Right + 10;

            text.Text = "eirmod tempor invidunt ut";

            var scale = 1.2;
            text.Width = text.Width / scale;
            var size3 = text.GetSize();
            col2.Height = size3.Height * scale;
            col2.Width = size3.Width * scale + 5;
			ctx.Scale(scale*scaleX, scale*scaleY);
            ctx.DrawTextLayout(text, col2.Left / scale, col2.Top / scale);
            ctx.Restore();

            // drawing col line
            ctx.SetLineWidth(1);

            ctx.SetColor(Colors.Black.WithAlpha(.5));
            ctx.MoveTo(col1.Right + 5, col1.Top);
            ctx.LineTo(col1.Right + 5, col1.Bottom);
            ctx.Stroke();
            ctx.MoveTo(col2.Right + 5, col2.Top);
            ctx.LineTo(col2.Right + 5, col2.Bottom);
            ctx.Stroke();
            ctx.SetColor(Colors.Black);

            // proofing heigth, on second col
            ctx.Save();
            ctx.SetColor(Colors.DarkCyan);
            text.Text = "Praesent ac lacus nec dolor pulvinar feugiat a id elit.";
            var size4 = text.GetSize();
            text.Height = size4.Height / 2;
            text.Trimming = TextTrimming.WordElipsis;
            ctx.DrawTextLayout(text, col2.Left, col2.Bottom + 5);

            ctx.SetLineWidth(1);
            ctx.SetColor(Colors.Blue);
            ctx.Rectangle(new Rectangle(col2.Left, col2.Bottom + 5, text.Width, text.Height));
            ctx.Stroke();

            // drawing col line
            ctx.SetLineWidth(1);

            ctx.SetColor(Colors.Black.WithAlpha(.5));
            ctx.MoveTo(col1.Right + 5, col1.Top);
            ctx.LineTo(col1.Right + 5, col1.Bottom);
            ctx.Stroke();
            ctx.MoveTo(col2.Right + 5, col2.Top);
            ctx.LineTo(col2.Right + 5, col2.Bottom);
            ctx.Stroke();
            ctx.SetColor(Colors.Black);

            ctx.Restore();

            // proofing rotate, and printing size to see the values
            ctx.Save();

            text.Font = this.Font.WithSize(10);
            text.Text = string.Format("Size 1 {0}\r\nSize 2 {1}\r\nSize 3 {2} Scale {3}",
                                       size1, size2, size3, scale);
            text.Width = -1; // this clears textsize
            text.Height = -1;
            ctx.Rotate(5);
            // maybe someone knows a formula with angle and textsize to calculyte ty
            var ty = 30;
            ctx.DrawTextLayout(text, ty, col1.Bottom + 10);

            ctx.Restore();

            ctx.Restore();
            
			ctx.Save ();

			// Text boces

            ctx.Translate(x, y);
            y = 180;

            // Without wrapping

            TextLayout tl = new TextLayout(ctx);
            tl.Text = "Stright text";
            DrawText(ctx, tl, ref y);

            // With wrapping

            tl = new TextLayout(ctx);
            tl.Text = "The quick brown fox jumps over the lazy dog";
            tl.Width = 100;
            DrawText(ctx, tl, ref y);

            // With blank lines

            tl = new TextLayout(ctx);
			tl.Text = "\nEmpty line above\nLine break above\n\nEmpty line above\n\n\nTwo empty lines above\nEmpty line below\n";
            tl.Width = 200;
            DrawText(ctx, tl, ref y);

			ctx.Restore ();
        }

        public void DrawText (Context ctx, TextLayout tl, ref double y) {
            double x = 10;
            var s = tl.GetSize();
            var rect = new Rectangle(x, y, s.Width, s.Height).Inflate(0.5, 0.5);
            ctx.SetLineWidth(1);
            ctx.SetColor(Colors.Blue);
            ctx.Rectangle(rect);
            ctx.Stroke();
            ctx.SetColor(Colors.Black);
            ctx.DrawTextLayout(tl, x, y);

            y += s.Height + 20;
        }

        public virtual void Transforms (Xwt.Drawing.Context ctx, double x, double y) {
            Rotate(ctx, x, y);
            Scale(ctx, x + 100, y);
        }

        public virtual void Rotate (Xwt.Drawing.Context ctx, double x, double y) {
            ctx.Save();
            ctx.Translate(x + 30, y + 30);
            ctx.SetLineWidth(3);
            // Rotation

            double end = 270;

            for (double n = 0; n <= end; n += 5) {
                ctx.Save();
                ctx.Rotate(n);
                ctx.MoveTo(0, 0);
                ctx.RelLineTo(30, 0);
                double c = n / end;
                ctx.SetColor(new Color(c, c, c));
                ctx.Stroke();
                ctx.Restore();
            }

            //ctx.ResetTransform();
            ctx.Restore();
        }

        public virtual void Scale (Context ctx, double ax, double ay) {
            ctx.Save();
            ctx.Translate(ax, ay);
            ctx.SetColor(Colors.Black);
            ctx.SetLineWidth(1);

			var mx = ctx.GetCTM ();
			var sx = mx.M11;
			var sy = mx.M22;

            var x = 0d;
            var y = 0d;
            var w = 10d;
            var inc = .1d;
			var doScale = true;
            for (var i = inc; i < 3.5d; i += inc) {
                ctx.Save();
				if (doScale) {
					ctx.Scale (i * sx, i * sy);
					mx = ctx.GetCTM ();
					Trace.WriteLine (string.Format ("{0} - {1} {2}", i * sx, mx.M11, mx.M22));
					ctx.Rectangle (x, y, w, w);
				} else {
					var z = 10;
					ctx.Rectangle (x* i*z, y* i*z, w* i*z, w* i*z);
				}
                ctx.SetColor(Colors.Yellow.WithAlpha(1 / i));
                ctx.FillPreserve();
                ctx.SetColor(Colors.Red.WithAlpha(1 / i));
                ctx.Stroke();
                //ctx.MoveTo(x += w * inc, y += w * inc / 3);
				x += w * inc; y += w * inc / 3;
                ctx.Restore();
				mx = ctx.GetCTM ();
				Trace.WriteLine (string.Format ("restore {0} {1}", mx.M11, mx.M22));
            }

            ctx.Restore();
        }

        public virtual void All (Context ctx) {
            Figures(ctx, 5, 5);
            Transforms(ctx, 5, 205);
            PatternsAndImages(ctx, 5, 305);
            Texts(ctx, 305, 5);
        }

        public Font Font { get; set; }
        public Rectangle Bounds { get; set; }

        public virtual void SpeedTest (Xwt.Drawing.Context ctx, double sx, double sy) {
            ctx.Save();

            ctx.Translate(sx, sy);

            var n = 1000;
            var ll = 80;
            var p = new Point(0, 0);
            for (double i = 1; i < n; i++) {

                ctx.MoveTo(p.X, p.Y);

                ctx.SetColor(new Color(i / n, i / n, i / n));

                ctx.LineTo(p.X + ll, p.Y + ll);
                ctx.Stroke();

                if (p.Y + ll > this.Bounds.Bottom) {
                    p.Y = 0;
                    p.X += ll + 5;

                } else {
                    p.Y++;
                }

            }

            ctx.Restore();
        }

        public void PaintIcon (Context c, double size,  double x, double y, Action<Context> draw) {
            c.Save();
            
            // settings needs to be adjusted from the SVG path values, those works well for FontAwesome
            c.Translate(4+x, size - 4+y);
            c.Scale(size / 2500, -size / 2500);

            draw(c);

            c.SetLineWidth(size/50);

            c.SetColor(Colors.Black);
            c.Fill();
            //c.SetColor(Colors.LightGray);
           // c.Stroke();

            c.Restore();

        }
    }


}
