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
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Presenter;
using Limaki.Presenter.UI;
using Limaki.Widgets;
using NUnit.Framework;
using Limaki.Tests.Graph.Model;

namespace Limaki.Tests.Presenter.Display {
    public class DisplayTest<T>:DomainTest where T:class {
        
        public ITestDevice TestDevice { get; set; }
        public object TestForm { get; set; }

        public Get<IDisplay<T>> Factory { get; set; }
        private IDisplay<T> _display = null;
        public virtual IDisplay<T> Display {
            get {
                if (_display ==null) {
                    try {
                        _display = Factory();
                    } catch (Exception e) {
                        this.ReportDetail (e.Message);
                        throw e;
                    }
                }
                return _display;
            }
            set { _display = value; }
        }

        public DisplayTest() {}
        public DisplayTest(IDisplay<T> display) {
            this.Display = display;
            this.TestForm = TestDevice.FindForm(display);
        }

        protected FrameTicker ticker = new FrameTicker();
        
        public override void  Setup(){
            base.Setup();
            if (TestForm == null) {
                try {
                    TestForm = TestDevice.CreateForm (this.Display);
                } catch (Exception e) {
                    this.ReportDetail(e.GetType()+"\t"+e.Message);
                    throw e;
                }
                
 	        }
            if ( Display != null )
                ticker.Instrument(Display);
            
        }

        public override void TearDown () {
            if ( Display != null )
                ticker.Disinstrument(Display);
            base.TearDown();
        }
        public int secToTest = 5;

        
        public virtual void DoEvents() {
            TestDevice.DoEvents ();
        }


        public int WidgetCount() {
            ILayer<T> layer = Display.EventControler.GetAction<ILayer<T>>();
            if (layer != null) {
                var renderer = layer.Renderer() as GraphSceneRenderer<IWidget, IEdgeWidget>;
                if (renderer != null)
                    return renderer.iWidgets;
            }
            return 0;
        }

        [Test]
        public virtual void RunSelectorTest() {

            IAction zoomAction = Display.EventControler.GetAction<ZoomAction> ();

            bool zoomEnabled = zoomAction.Enabled;
            zoomAction.Enabled= false;

            bool trackerEnabled = Display.MouseScrollAction.Enabled;
            Display.MouseScrollAction.Enabled = false;

            IMoveResizeAction selection = Display.SelectAction;
            selection.Enabled = true;
            selection.Clear();

            PointI position = new PointI(0, 0);
            PointI max = new PointI(Display.Viewport.ClipSize.Width, Display.Viewport.ClipSize.Height);
            this.ReportDetail("Selector runs to " + max);

            var iWidgets = WidgetCount ();

            ticker.Start();

            MouseActionEventArgs e = 
                new MouseActionEventArgs(MouseActionButtons.Left, ModifierKeys.None,
                    0, position.X, position.Y, 0);

            selection.OnMouseHover(e);
            selection.OnMouseDown(e);
            DoEvents();

            while ((position.X < max.X) && (position.Y < max.Y)) {
                position.X += 1;
                position.Y += 1;
                e = new MouseActionEventArgs(
                    MouseActionButtons.Left, ModifierKeys.None, 
                    0, position.X, position.Y, 0);
                selection.OnMouseMove(e);
                DoEvents();
            }
            
            ticker.Stop();
            selection.OnMouseUp(e);
            selection.Clear ();

            
            iWidgets = WidgetCount() - iWidgets;

            this.ReportDetail(
                "Selector-Test" + " \t" +
                ticker.ElapsedInSec() + " sec \t" +
                ticker.FramePerSecond() + " fps \t" +
                iWidgets + " widgets \t"
                );
            
            ticker.Disinstrument(Display);
            
            zoomAction.Enabled = zoomEnabled;
            Display.MouseScrollAction.Enabled = trackerEnabled;
            Display.Execute ();
            DoEvents();

        }

        public virtual void RunFrameTest(Frame frame) {
            this.ReportDetail("Test run for " + secToTest + " seconds " + frame.ToString() + "...");

            ticker.Start();

            RectangleI rect = new RectangleI(new PointI(),Display.Viewport.ClipSize);
            int div = 2;
            if (frame == Frame.Quarter) {
                div = 4;
            }
            SizeI deflate = new SizeI(rect.Width / div, rect.Height / div);
            rect = new RectangleI(
                new PointI(rect.Location.X + deflate.Width, rect.Location.Y + deflate.Height), deflate);

            int time = (secToTest * 1000) + Environment.TickCount;
            while (ticker.Elapsed > Environment.TickCount) {
                if (frame == Frame.Full) {
                    ((IControl)Display).Invalidate();
                } else {
                    ((IControl)Display).Invalidate(rect);
                }
                ((IControl)Display).Update();
                DoEvents();
            }

            ticker.Stop();
            string fpsResult =
                (ticker.FramePerSecond()) +
                " fps (" + frame.ToString() + "),";

            this.ReportDetail(fpsResult);
        }


        #region helper-functions
        
        public void NeutralPosition() {
            MouseActionEventArgs e =
                new MouseActionEventArgs(MouseActionButtons.Left, ModifierKeys.None, 0, 1, 1, 0);
            Display.EventControler.OnMouseDown(e);
            DoEvents();
            Display.EventControler.OnMouseMove(e);
            DoEvents();
            Display.EventControler.OnMouseUp(e);
            DoEvents();
        }

        protected virtual PointS nextPoint(PointS n, SizeS d) {
            return n + d;
        }

        protected virtual void MoveAlongLine(Vector v) {
            PointS next = PointS.Empty;
            PointS end = v.End;

            int dx = v.End.X - v.Start.X;
            int dy = v.End.Y - v.Start.Y;
            double m1 = (double)dy / dx;
            double m = Math.Abs((double)dy / dx);


            SizeS d = new SizeS(1, (float)m);
            if (m == 1d) {
                // horizontal line:
                d = new SizeS(1, 0);
            } else if (double.IsInfinity(m)) {
                // vertical line:
                d = new SizeS(0, 1);
            } else if (m > 1d) {
                d = new SizeS((float)(1d / m), 1);
            }

            Func<bool> rangeX = delegate() { return next.X < end.X; };
            Func<bool> rangeY = delegate() { return next.Y < end.Y; };
            if (dx < 0) {
                d = new SizeS(-d.Width, d.Height);
                rangeX = delegate() { return next.X > end.X; };
            }
            if (dy < 0) {
                d = new SizeS(d.Width, -d.Height);
                rangeY = delegate() { return next.Y > end.Y; };
            }
            //double angle = Math.Atan(dx / dy) / Math.PI * 180 + ((dx < 0) ? 180 : 0);
            MouseActionEventArgs e = null;
            next = nextPoint(v.Start, d);
            end = v.End;
            while (rangeX() || rangeY()) {
                PointS tnext = camera.FromSource(next);
                e = new MouseActionEventArgs(MouseActionButtons.Left,
                        ModifierKeys.None, 0, (int)tnext.X, (int)tnext.Y, 0);
                Display.EventControler.OnMouseMove(e);
                DoEvents();
                next = nextPoint(next, d);
            }

        }

        protected ICamera camera {
            get { return Display.Viewport.Camera; }
        }

        #endregion
    }
}
