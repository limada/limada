/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.View;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Rendering;
using Limaki.View.Viz.UI;
using NUnit.Framework;
using Xwt;

namespace Limaki.Tests.View.Display {

    public class DisplayTest<T> : DomainTest where T : class {
        public Func<IDisplay<T>> CreateDisplay { get; set; }
        private IDisplay<T> _display = null;
        public virtual IDisplay<T> Display {
            get {
                if (_display == null) {
                    try {
                        if (CreateDisplay != null)
                            _display = CreateDisplay ();
                        else
                            _display = Registry.Factory.Create<IDisplay<T>>();
                    } catch (Exception e) {
                        this.ReportDetail (e.Message);
                        throw e;
                    }
                }
                return _display;
            }
            set { _display = value; }
        }

        private IVindow _testWindow = null;
        public virtual IVindow TestWindow {
            get {  return _testWindow ?? (_testWindow = new Vindow ()); }
            set { _testWindow = value; }
        }

        public override void Setup () {
            base.Setup ();
            if (_testWindow == null) {
                TestWindow.Content = this.Display;
            }
            if (Display != null)
                ticker.Instrument (Display);

        }

        protected FrameTicker ticker = new FrameTicker ();

        public override void TearDown () {
            if (Display != null)
                ticker.Disinstrument (Display);
            base.TearDown ();
        }
        
        public int secToTest = 5;

        public virtual void DoEvents () {
            Display.SetFocus ();
            Application.MainLoop.DispatchPendingEvents();
        }

        public int VisualsCount () {
            var layer = Display.ActionDispatcher.GetAction<ILayer<T>> ();
            if (layer != null) {
                var renderer = layer.Renderer () as GraphSceneRenderer<IVisual, IVisualEdge>;
                if (renderer != null)
                    return renderer.iItems;
            }
            return 0;
        }

        [Test]
        public virtual void RunSelectorTest () {

            var zoomAction = Display.ActionDispatcher.GetAction<ZoomAction> ();

            bool zoomEnabled = zoomAction.Enabled;
            zoomAction.Enabled = false;

            bool trackerEnabled = Display.MouseScrollAction.Enabled;
            Display.MouseScrollAction.Enabled = false;

            var selection = Display.SelectAction;
            selection.Enabled = true;
            selection.Clear ();

            var position = new Point (0, 0);
            var max = new Point (Display.Viewport.ClipSize.Width, Display.Viewport.ClipSize.Height);
            this.ReportDetail ("Selector runs to " + max);

            var visualsCount = VisualsCount ();

            ticker.Start ();

            var e = new MouseActionEventArgs (MouseActionButtons.Left, ModifierKeys.None, 0, position.X, position.Y, 0);

            selection.OnMouseHover (e);
            selection.OnMouseDown (e);
            DoEvents ();

            while ((position.X < max.X) && (position.Y < max.Y)) {
                position.X += 1;
                position.Y += 1;
                e = new MouseActionEventArgs (
                    MouseActionButtons.Left, ModifierKeys.None,
                    0, position.X, position.Y, 0);
                selection.OnMouseMove (e);
                DoEvents ();
            }

            ticker.Stop ();
            selection.OnMouseUp (e);
            selection.Clear ();


            visualsCount = VisualsCount () - visualsCount;

            this.ReportDetail (
                "Selector-Test" + " \t" +
                ticker.ElapsedInSec () + " sec \t" +
                ticker.FramePerSecond () + " fps \t" +
                visualsCount + " visuals \t"
                );

            ticker.Disinstrument (Display);

            zoomAction.Enabled = zoomEnabled;
            Display.MouseScrollAction.Enabled = trackerEnabled;
            Display.Perform ();
            DoEvents ();

        }

        public virtual void RunFrameTest (Frame frame) {
            this.ReportDetail ("Test run for " + secToTest + " seconds " + frame.ToString () + "...");

            ticker.Start ();

            var rect = new Rectangle (new Point (), Display.Viewport.ClipSize);
            int div = 2;
            if (frame == Frame.Quarter) {
                div = 4;
            }
            var deflate = new Size (rect.Width / div, rect.Height / div);
            rect = new Rectangle (
                new Point (rect.Location.X + deflate.Width, rect.Location.Y + deflate.Height), deflate);

            int time = (secToTest * 1000) + Environment.TickCount;
            var backend = Display.Backend;
            while (ticker.Elapsed > Environment.TickCount) {
                if (frame == Frame.Full) {
                    backend.Invalidate ();
                } else {
                    backend.Invalidate (rect);
                }
                backend.Update ();
                DoEvents ();
            }

            ticker.Stop ();
            string fpsResult =
                (ticker.FramePerSecond ()) +
                " fps (" + frame.ToString () + "),";

            this.ReportDetail (fpsResult);
        }


        #region helper-functions

        public void NeutralPosition () {
            var e = new MouseActionEventArgs (MouseActionButtons.Left, ModifierKeys.None, 0, 1, 1, 0);
            Display.ActionDispatcher.OnMouseDown (e);
            DoEvents ();
            Display.ActionDispatcher.OnMouseMove (e);
            DoEvents ();
            Display.ActionDispatcher.OnMouseUp (e);
            DoEvents ();
        }

        protected virtual Point nextPoint (Point n, Size d) {
            return n + d;
        }

        protected virtual void MoveAlongLine (Vector v) {
            var next = Point.Zero;
            var end = v.End;

            var dx = v.End.X - v.Start.X;
            var dy = v.End.Y - v.Start.Y;
            var m1 = (double)dy / dx;
            var m = Math.Abs ((double)dy / dx);


            var d = new Size (1, (float)m);
            if (m == 1d) {
                // horizontal line:
                d = new Size (1, 0);
            } else if (double.IsInfinity (m)) {
                // vertical line:
                d = new Size (0, 1);
            } else if (m > 1d) {
                d = new Size ((float)(1d / m), 1);
            }

            Func<bool> rangeX = () => next.X < end.X;
            Func<bool> rangeY = () => next.Y < end.Y;
            if (dx < 0) {
                d = new Size (-d.Width, d.Height);
                rangeX = () => next.X > end.X;
            }
            if (dy < 0) {
                d = new Size (d.Width, -d.Height);
                rangeY = () => next.Y > end.Y;
            }
            //double angle = Math.Atan(dx / dy) / Math.PI * 180 + ((dx < 0) ? 180 : 0);
            MouseActionEventArgs e = null;
            next = nextPoint (v.Start, d);
            end = v.End;
            while (rangeX () || rangeY ()) {
                var tnext = camera.FromSource (next);
                e = new MouseActionEventArgs (MouseActionButtons.Left,
                                              ModifierKeys.None, 0,
                                              tnext.X, tnext.Y,
                                              0);
                Display.ActionDispatcher.OnMouseMove (e);
                DoEvents ();
                next = nextPoint (next, d);
            }

        }

        protected ICamera camera {
            get { return Display.Viewport.Camera; }
        }

        #endregion
    }
}