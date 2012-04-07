using Limaki.Painting;
using Limaki.GDI.Painting;
using Xwt.Drawing;
using Xwt.Gdi.Backend;
using Xwt.Gdi;
using System;
using SD = System.Drawing;
using System.Diagnostics;

namespace Limaki.Tests.Sandbox {

    public enum PaintContextTestCase {
        XwtSample,
        MySample,
        SpeedTest,

    }

    public class PaintContextTestControl : System.Windows.Forms.UserControl {
        public PaintContextTestControl() {
            this.Stopwatch = new Stopwatch();
            System.Windows.Forms.ControlStyles controlStyle =
                    System.Windows.Forms.ControlStyles.UserPaint
                    | System.Windows.Forms.ControlStyles.AllPaintingInWmPaint
                    | System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer;

            //if (Opaque) {
            //    controlStyle = controlStyle | ControlStyles.Opaque;
            //}

            this.SetStyle (controlStyle, true);
        }

        PaintContextTestCase TestCase { get; set; }
        protected override void OnPaint (System.Windows.Forms.PaintEventArgs e) {
            base.OnPaint (e);
            this.BackColor = SD.Color.White;

            var graphics = new GdiContext { Graphics = e.Graphics };
            var painter = new PaintContext (graphics);
            var context = new Xwt.Drawing.Context(graphics);
            if (TestCase == PaintContextTestCase.XwtSample)
                XwtSample (context);
            else if (TestCase == PaintContextTestCase.MySample)
                MySample (painter);
            else if (TestCase == PaintContextTestCase.SpeedTest) {
                Stopwatch.Start();
                SpeedTest(context);
                Stopwatch.Stop();
            }
            graphics.Dispose ();
            Frames++;

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
            ctx.Font = Xwt.Engine.WidgetRegistry.CreateFrontend<Font>(this.Font);

            var p = new Samples.ReferencePainter();
            p.Font = Xwt.Engine.WidgetRegistry.CreateFrontend<Font> (this.Font);
            p.Bounds = this.Bounds.ToXwt();
            p.All(ctx);
           
        }

        protected virtual void SpeedTest (Xwt.Drawing.Context ctx) {
            var p = new Samples.ReferencePainter ();
            p.Font = Xwt.Engine.WidgetRegistry.CreateFrontend<Font> (this.Font);
            p.Bounds = this.Bounds.ToXwt ();
            p.SpeedTest (ctx,0,0);
        }

        public int Iterations { get; set; }
        public int Frames { get; set; }
        Stopwatch Stopwatch { get; set; }

        public virtual void SpeedTest() {
            Iterations = 200;
            this.TestCase = PaintContextTestCase.SpeedTest;
            int i = 0;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            System.Windows.Forms.Application.DoEvents ();
            while (i++ < Iterations) {
                this.Invalidate();
                System.Windows.Forms.Application.DoEvents();
            }
            System.Windows.Forms.Application.DoEvents ();
            this.Cursor = System.Windows.Forms.Cursors.Default;
            var ms = Stopwatch.ElapsedMilliseconds;
            if (ms == 0)
                ms = 1;
            Trace.WriteLine(string.Format ("Time in sec {0:#0.00}\tFrames per sec {1:#0.00}\tFrames {2}",
                                 ms / 1000d,
                                 Frames / (ms / 1000d),
                                 Frames));

        }
    }


}