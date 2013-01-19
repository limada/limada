using Limaki.Painting;
using Limaki.GDI.Painting;
using Xwt.Drawing;
using Xwt.Gdi.Backend;
using Xwt.Gdi;
using System;
using SD = System.Drawing;
using System.Diagnostics;
using Xwt.Tests;
using Limaki.Iconerias;
using System.Linq;
using Xwt.Engine;
using System.Linq.Expressions;

namespace Limaki.Tests.Sandbox {

    public enum PaintContextTestCase {
        XwtSample,
        MySample,
        SpeedTest,
        AwesomeIcons
    }

    public class PaintContextTestControl : System.Windows.Forms.UserControl {
        public PaintContextTestControl () {
            this.Stopwatch = new Stopwatch();
            System.Windows.Forms.ControlStyles controlStyle =
                    System.Windows.Forms.ControlStyles.UserPaint
                    | System.Windows.Forms.ControlStyles.AllPaintingInWmPaint
                    | System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer;

            //if (Opaque) {
            //    controlStyle = controlStyle | ControlStyles.Opaque;
            //}

            this.SetStyle(controlStyle, true);
            this.TestCase = PaintContextTestCase.AwesomeIcons;
        }

        PaintContextTestCase TestCase { get; set; }
        WidgetRegistry Registry { get; set; }
        protected override void OnPaint (System.Windows.Forms.PaintEventArgs e) {

            Registry = GdiEngine.Registry;

            base.OnPaint(e);

            this.BackColor = SD.Color.White;

            var graphics = new GdiContext(e.Graphics);

            var context = new Xwt.Drawing.Context(this.Registry, graphics);


            if (TestCase == PaintContextTestCase.XwtSample)
                XwtSample(context);
            else if (TestCase == PaintContextTestCase.SpeedTest) {
                Stopwatch.Start();
                SpeedTest(context);
                Stopwatch.Stop();
            } else if (TestCase == PaintContextTestCase.AwesomeIcons) {
                AwesomeIcons(context);
            }

            graphics.Dispose();
            Frames++;

        }

        private void AwesomeIcons (Context ctx) {
            ctx.Font = this.Registry.CreateFrontend<Font>(this.Font);

            var p = new ReferencePainter();
            p.Font = ctx.Font;

            p.Bounds = this.Bounds.ToXwt();
            var awesomeIcons = new AwesomeIconeria { Fill = true, FillColor = Colors.Black };

            var size = 24;
            var border = 24;
            var x = border * 2;
            var y = border * 2;
            var textSize = 8;
            var textLayout = new TextLayout(ctx);
            textLayout.Font = ctx.Font.WithSize(8);

            awesomeIcons.ForEach((icon, name, id) => {

                var img = awesomeIcons.AsImage(ctx.Registry,icon, size);
                ctx.DrawImage(img, x, y);

                textLayout.Text = name.Remove(0, 5);
                ctx.DrawTextLayout(textLayout, x, y + size);

                x += size + border;
                if (x >= p.Bounds.Width - border) {
                    x = border * 2;
                    y += size + border + textSize;
                }
            });


        }

        //Action<Context> icon = c => iconMethod.Invoke(font, new object[] { c });
        //var ix = Expression.Constant(font);
        //var ic = Expression.Parameter(typeof(Context), "it");
        //icon = Expression.Lambda<Action<Context>>(
        //    Expression.Call(ix, iconMethod,ic), ic).Compile();

        protected virtual void XwtSample (Xwt.Drawing.Context ctx) {
            ctx.Font = GdiEngine.Registry.CreateFrontend<Font>(this.Font);

            var p = new ReferencePainter();
            p.Font = GdiEngine.Registry.CreateFrontend<Font>(this.Font);
            p.Bounds = this.Bounds.ToXwt();
            p.All(ctx);

        }

        protected virtual void SpeedTest (Xwt.Drawing.Context ctx) {
            var p = new ReferencePainter();
            p.Font = GdiEngine.Registry.CreateFrontend<Font>(this.Font);
            p.Bounds = this.Bounds.ToXwt();
            p.SpeedTest(ctx, 0, 0);
        }

        public int Iterations { get; set; }
        public int Frames { get; set; }
        Stopwatch Stopwatch { get; set; }

        public virtual void SpeedTest () {
            Iterations = 200;
            this.TestCase = PaintContextTestCase.XwtSample;
            int i = 0;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            System.Windows.Forms.Application.DoEvents();
            while (i++ < Iterations) {
                this.Invalidate();
                System.Windows.Forms.Application.DoEvents();
            }
            System.Windows.Forms.Application.DoEvents();
            this.Cursor = System.Windows.Forms.Cursors.Default;
            var ms = Stopwatch.ElapsedMilliseconds;
            if (ms == 0)
                ms = 1;
            Trace.WriteLine(string.Format("Time in sec {0:#0.00}\tFrames per sec {1:#0.00}\tFrames {2}",
                                 ms / 1000d,
                                 Frames / (ms / 1000d),
                                 Frames));

        }
    }


}