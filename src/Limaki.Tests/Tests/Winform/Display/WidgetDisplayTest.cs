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
using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.Tests.Display;
using Limaki.Tests.Graph.Model;
using Limaki.Drawing.UI;
using Limaki.Widgets;
using Limaki.Winform.Displays;
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
            MouseActionEventArgs e = 
                new MouseActionEventArgs(MouseActionButtons.Left, ModifierKeys.None, 0, 1, 1, 0);
            Display.EventControler.OnMouseDown(e);
            Application.DoEvents();
            Display.EventControler.OnMouseMove(e);
            Application.DoEvents();
            Display.EventControler.OnMouseUp(e);
            Application.DoEvents();
        }

        #region helper-functions
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

            System.Func<bool> rangeX = delegate() { return next.X < end.X; };
            System.Func<bool> rangeY = delegate() { return next.Y < end.Y; };
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
                Application.DoEvents();
                next = nextPoint(next, d);
            }

        }

        protected ICamera camera {
            get { return Display.Camera; }
        }

        public void MoveLink(IWidget link, IWidget target) {
            NeutralPosition();

            SizeI size = new SizeI(Math.Abs(link.Shape.Size.Width) / 4, -Math.Abs(link.Shape.Size.Height) / 4);
            PointI position = link.Shape[Anchor.Center] + size;
            position = camera.FromSource(position);
            //new Size(5, (int)(m * 5));

            // select link
            MouseActionEventArgs e = 
                new MouseActionEventArgs(MouseActionButtons.Left,
                    ModifierKeys.None, 0, position.X, position.Y, 0);
            Display.EventControler.OnMouseDown(e);
            Application.DoEvents();

            Assert.AreSame(Scene.Focused, link);

            Vector v = new Vector();
            v.Start = link.Shape[Anchor.LeftTop];
            v.End = target.Shape[Anchor.Center];
            //m = M(v);
            size = new SizeI(Math.Abs(target.Shape.Size.Width) / 4, -Math.Abs(target.Shape.Size.Height) / 4);
            v.End = v.End + size;//new Size((int)(m*3),-3);

            // start move
            position = camera.FromSource(v.Start);
            e = new MouseActionEventArgs(MouseActionButtons.Left, 
                ModifierKeys.None, 
                0, position.X, position.Y, 0);
            Display.EventControler.OnMouseDown(e);
            Application.DoEvents();

            MoveAlongLine(v);

            // end move
            position = camera.FromSource(v.End);
            e = new MouseActionEventArgs(MouseActionButtons.Left, ModifierKeys.None, 
                0, position.X, position.Y, 0);
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
