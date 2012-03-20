using Limaki.Painting;
using Limaki.GDI.Painting;
using Xwt.Drawing;
using Xwt.Gdi.Backend;
using System;
namespace Limaki.Tests.Sandbox {
   
    public class PaintContextTestControl:System.Windows.Forms.UserControl {

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
            base.OnPaint(e);
            var pen = new System.Drawing.Pen(System.Drawing.Color.WhiteSmoke);
            pen.Width = 2;

            var graphics = new GdiContext { Graphics = e.Graphics };
            var painter = new PaintContext (graphics);

            if (true)
                XwtSample (painter.CreateContext ());
            else
                MySample(painter);

            graphics.Dispose ();

        }

        protected virtual void MySample (PaintContext painter) {
            var bounds = new Xwt.Rectangle (this.Bounds.X, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height);
            var backColor = SystemColors.Window;
            painter.Rectangle (bounds);
            painter.SetColor (backColor);
            painter.Fill ();

            painter.Translate (10, 10);

            painter.MoveTo (50, 100);
            painter.LineTo (30, 30);
            painter.Arc (20, 20, 40, 90, 70);
            painter.ClosePath ();
            painter.SetColor (Colors.Wheat);
            painter.FillPreserve ();
            painter.SetColor (Colors.Red);
            painter.SetLineWidth (2);
            painter.Stroke ();

            painter.MoveTo (0, 0);
            painter.LineTo (0, 50);
            painter.SetLineWidth (1);
            painter.Translate (60, 60);
            Action<double> rotate = r => {
                painter.Rotate (r);

                painter.SetColor (Colors.GhostWhite);
                painter.FillPreserve ();
                painter.SetColor (Colors.Green);

                painter.StrokePreserve ();
                //if (r > 180)
                painter.ResetTransform (); //todo: split into resetPathTransform
                painter.Rotate (-r);

            };
            for (int i = 0; i <= 360; i += 3)
                rotate (i);

            painter.Stroke ();
            painter.Translate (0, 0);
            painter.Rotate (5);

            painter.SetColor (Colors.Blue);

            painter.MoveTo (50, 50);
            painter.Arc (70, 70, 40, 360, 180);
            painter.ClosePath ();

            painter.TranslatePath (150, 150);
            painter.Stroke ();
            painter.ResetTransform ();

            var textPos = new Xwt.Point (100, 100);
            var s = string.Format ("textPos {0}", textPos);
            var font = Xwt.Drawing.Font.FromName ("Tahoma", 10);

            var text = painter.CreateTextLayout ();
            text.Font = font;
            text.Text = s;
            text.Width = 60;
            painter.Rotate (5);
            painter.DrawTextLayout (text, textPos.X, textPos.Y);
            var size = text.GetSize ();

            painter.SetColor (Colors.Fuchsia);
            text.Text = string.Format ("has size {0}", size);
            text.Width = 200;
            text.Font = text.Font.WithWeight (FontWeight.Bold);
            textPos.Y += size.Height + 5;

            painter.DrawTextLayout (text, textPos.X, textPos.Y);

            size = text.GetSize ();
            text.Text = s;
            text.Width = 50;
            textPos.Y += size.Height + 5;

            painter.SetColor (Colors.Blue);
            painter.DrawTextLayout (text, textPos.X, textPos.Y, 20);
            size.Height = 20;
            textPos.Y += size.Height + 5;


            painter.TextLayout (text, textPos.X, textPos.Y, 20);
            painter.ResetTransform ();
            painter.SetColor (Colors.Blue);
            painter.Rotate (20);
            painter.FillPreserve ();
            painter.SetLineWidth (0);
            painter.SetColor (backColor);
            painter.Stroke ();
        }

        protected virtual void XwtSample (Xwt.Drawing.Context ctx) {

            // Simple rectangles

            ctx.SetLineWidth (1);
            ctx.Rectangle (100, 5, 10, 10);
            ctx.SetColor (Color.Black);
            ctx.Fill ();

            ctx.Rectangle (115, 5, 10, 10);
            ctx.SetColor (Color.Black);
            ctx.Stroke ();

            //

            ctx.SetLineWidth (3);
            ctx.Rectangle (100, 20, 10, 10);
            ctx.SetColor (Color.Black);
            ctx.Fill ();

            ctx.Rectangle (115, 20, 10, 10);
            ctx.SetColor (Color.Black);
            ctx.Stroke ();

            // Rectangle with hole

            ctx.Rectangle (10, 100, 40, 40);
            ctx.MoveTo (45, 135);
            ctx.RelLineTo (0, -20);
            ctx.RelLineTo (-20, 0);
            ctx.RelLineTo (0, 20);
            ctx.ClosePath ();
            ctx.SetColor (Color.Black);
            ctx.Fill ();

            // Dashed lines
			
            ctx.SetLineDash (15, 10, 10, 5, 5);
            ctx.Rectangle (100, 100, 100, 100);
            ctx.Stroke ();
            ctx.SetLineDash (0);
            Image img = null;
            if (true) {
                ImageBuilder ib = new ImageBuilder (30, 30, ImageFormat.ARGB32);
                ib.Context.Arc (15, 15, 15, 0, 360);
                ib.Context.SetColor (new Color (1, 0, 1));
                ib.Context.Rectangle (0, 0, 5, 5);
                ib.Context.Fill ();
                img = ib.ToImage ();
                ctx.DrawImage (img, 90, 90);
                ctx.DrawImage (img, 90, 140, 50, 10);
            } else {
                ctx.Arc (105, 105, 15, 0, 360);
                ctx.SetColor (new Color (1, 0, 1));
                ctx.Rectangle (90, 90, 5, 5);
                ctx.Fill ();
            }
            ctx.Arc (190, 190, 15, 0, 360);
            ctx.SetColor (new Color (1, 0, 1, 0.4));
            ctx.Fill ();

            ctx.Save ();
            ctx.Translate (90, 220);
            if (true) {
                ctx.Pattern = new ImagePattern(img);
            }
            ctx.Rectangle (0, 0, 100, 70);
            ctx.Fill ();
            ctx.Restore ();

            ctx.Translate (30, 30);
            double end = 270;

            for (double n = 0; n <= end; n += 5) {
                ctx.Save ();
                ctx.Rotate (n);
                ctx.MoveTo (0, 0);
                ctx.RelLineTo (30, 0);
                double c = n / end;
                ctx.SetColor (new Color (c, c, c));
                ctx.Stroke ();
                ctx.Restore ();
            }
        }
    }

    
}