/*
 * Limaki 
 * Version 0.071
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
using System.Windows.Forms;
using System.Drawing;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Actions;
using Limaki.Winform.Displays;
using Limaki.Tests.Display;
using Limaki.Widgets;
using NUnit.Framework;


namespace Limaki.Tests.Widget {
    public class WidgetDisplayTest : DisplayTest<WidgetDisplay,Scene> {
        public WidgetDisplayTest():base() {}
        public WidgetDisplayTest(WidgetDisplay display):base(display) {}

        protected Scene _scene = null;
        public virtual Scene Scene {
            get {
                if (_scene == null) {
                    _scene = new BenchmarkOneSceneFactory().Scene;
                }
                return _scene;
            }
            set { _scene = value; }
        }

        bool zoomEnabled = false;
        bool trackerEnabled = false;
        bool selectorEnabled = false;
        protected void InitDisplay() {
            this.Display.ZoomState = ZoomState.Original;
            this.Display.Data = this.Scene;
            zoomEnabled = Display.ZoomAction.Enabled;
            Display.ZoomAction.Enabled = false;
            trackerEnabled = Display.ScrollAction.Enabled;
            Display.ScrollAction.Enabled = false;
            selectorEnabled = Display.SelectAction.Enabled;
            Display.SelectAction.Enabled = false;
        }

        public override void TearDown() {
            Display.ZoomAction.Enabled = zoomEnabled;
            Display.ScrollAction.Enabled = trackerEnabled;
            Display.SelectAction.Enabled = selectorEnabled;
            base.TearDown();
        }
        public override void Setup() {
            base.Setup();
            InitDisplay ();
        }

        public void NeutralPosition() {
            MouseEventArgs e = new MouseEventArgs(MouseButtons.Left, 0, 1, 1, 0);
            Display.EventControler.OnMouseDown(e);
            Application.DoEvents();
            Display.EventControler.OnMouseMove(e);
            Application.DoEvents();
            Display.EventControler.OnMouseUp(e);
            Application.DoEvents();
        }

        #region helper-functions
        protected virtual PointF nextPoint(PointF n, SizeF d) {
            return n + d;
        }

        protected virtual void MoveAlongLine(Vector v) {
            PointF next = PointF.Empty;
            PointF end = v.End;

            int dx = v.End.X - v.Start.X;
            int dy = v.End.Y - v.Start.Y;
            double m1 = (double)dy / dx;
            double m = Math.Abs((double)dy / dx);


            SizeF d = new SizeF(1, (float)m);
            if (m == 1d) {
                // horizontal line:
                d = new SizeF(1, 0);
            } else if (double.IsInfinity(m)) {
                // vertical line:
                d = new SizeF(0, 1);
            } else if (m > 1d) {
                d = new SizeF((float)(1d / m), 1);
            }

            Fun<bool> rangeX = delegate() { return next.X < end.X; };
            Fun<bool> rangeY = delegate() { return next.Y < end.Y; };
            if (dx < 0) {
                d = new SizeF(-d.Width, d.Height);
                rangeX = delegate() { return next.X > end.X; };
            }
            if (dy < 0) {
                d = new SizeF(d.Width, -d.Height);
                rangeY = delegate() { return next.Y > end.Y; };
            }
            //double angle = Math.Atan(dx / dy) / Math.PI * 180 + ((dx < 0) ? 180 : 0);
            MouseEventArgs e = null;
            next = nextPoint(v.Start, d);
            end = v.End;
            while (rangeX() || rangeY()) {
                PointF tnext = camera.FromSource(next);
                e = new MouseEventArgs(MouseButtons.Left, 0, (int)tnext.X, (int)tnext.Y, 0);
                Display.EventControler.OnMouseMove(e);
                Application.DoEvents();
                next = nextPoint(next, d);
            }

        }

        protected ICamera camera {
            get { return Display.DataLayer.Camera; }
        }

        public void MoveLink(IWidget link, IWidget target) {
            NeutralPosition();

            Size size = new Size(Math.Abs(link.Shape.Size.Width) / 4, -Math.Abs(link.Shape.Size.Height) / 4);
            Point position = link.Shape[Anchor.Center] + size;
            position = camera.FromSource(position);
            //new Size(5, (int)(m * 5));

            // select link
            MouseEventArgs e = new MouseEventArgs(MouseButtons.Left, 0, position.X, position.Y, 0);
            Display.EventControler.OnMouseDown(e);
            Application.DoEvents();

            Assert.AreSame(Scene.Focused, link);

            Vector v = new Vector();
            v.Start = link.Shape[Anchor.LeftTop];
            v.End = target.Shape[Anchor.Center];
            //m = M(v);
            size = new Size(Math.Abs(target.Shape.Size.Width) / 4, -Math.Abs(target.Shape.Size.Height) / 4);
            v.End = v.End + size;//new Size((int)(m*3),-3);

            // start move
            position = camera.FromSource(v.Start);
            e = new MouseEventArgs(MouseButtons.Left, 0, position.X, position.Y, 0);
            Display.EventControler.OnMouseDown(e);
            Application.DoEvents();

            MoveAlongLine(v);

            // end move
            position = camera.FromSource(v.End);
            e = new MouseEventArgs(MouseButtons.Left, 0, position.X, position.Y, 0);
            Assert.AreSame(Scene.Hovered, target);
            Display.EventControler.OnMouseUp(e);
            Application.DoEvents();



        }
        #endregion

        [Test]
        public override void RunSelectorTest() {
            base.RunSelectorTest();
        }



    }
}
