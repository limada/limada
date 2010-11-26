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


using Limaki.Common;
using Limaki.Tests.Graph.Model;
using Limaki.Drawing.UI;
using Limaki.Widgets.Paint;
using Limaki.Winform.Displays;
using Limaki.Drawing;
using Limaki.Widgets;
using Limaki.Actions;
using NUnit.Framework;
using System.Windows.Forms;
using System;

namespace Limaki.Tests.Widget {
    public class BenchmarkOneTests:WidgetDisplayTest {
        public BenchmarkOneTests():base() {}
        public BenchmarkOneTests(WidgetDisplay display):base(display) {}

        BenchmarkOneSceneFactory _factory = null;
        BenchmarkOneSceneFactory factory {
            get {
                if (_factory == null) {
                    _factory = new BenchmarkOneSceneFactory();
                }
                return _factory;

            }
        }

        public override Scene Scene {
            get {
                if (_scene == null) {
                    base.Scene = factory.Scene;
                }
                return base.Scene;
            }
            set {
                base.Scene = value;
            }
        }

        bool editorEnabled = false;
        bool dragDropEnabled = false;
        ILayout<Scene, IWidget> oldlayout = null;
        public override void Setup() {
            
            if (Display != null) {
                oldlayout = Display.DataLayout;
                Display.DataLayout = new BenchmarkOneSceneFactory.LongtermPerformanceLayout(
                                    () => { return Display.Data; }, factory.styleSheet);
            }

            base.Setup();
            
            factory.Arrange (Display.Data);
            Display.CommandsInvoke ();
            editorEnabled = Display.WidgetTextEditor.Enabled;
            dragDropEnabled = Display.WidgetDragDrop.Enabled;
            Display.WidgetChanger.Enabled = true;
            Display.EdgeWidgetChanger.Enabled = true;
            // this is neccessary as the mouse cursor returns after a long time
            // back to its position and activates WidgetTextEditor
            Display.WidgetTextEditor.Enabled = false;
            Display.WidgetDragDrop.Enabled = false;
            Display.AddWidgetAction.Enabled = false;
            Display.AddEdgeAction.Enabled = false;


            ( (GDIWidgetLayer) Display.DataLayer ).sceneRenderer.iWidgets = 0;
        }

        public override void TearDown() {
            base.TearDown();
            Display.WidgetTextEditor.Enabled = editorEnabled;
            Display.WidgetDragDrop.Enabled = dragDropEnabled;
            if (oldlayout != null)
            ( (GDISceneControler<Scene, IWidget>) Display.LayoutControler ).Layout = oldlayout;
        }



        public void MoveLinks(RectangleI bounds) {
            MoveLink(factory.Edge[4],factory.Edge[1]);
            MoveLink(factory.Edge[5], factory.Edge[3]);
        }


        

        public void MoveNode1(RectangleI bounds) {
            NeutralPosition ();
            PointI startposition = factory.Node[1].Shape[Anchor.LeftTop]+new SizeI(10,0);
            PointI position = camera.FromSource(startposition);

            MouseActionEventArgs e = 
                new MouseActionEventArgs(
                    MouseActionButtons.Left, ModifierKeys.None, 
                    0, position.X, position.Y, 0);
            Display.EventControler.OnMouseDown (e);

            Assert.AreSame (Scene.Focused, factory.Node[1]);

            
            Vector v = new Vector ();
            // diagonal movement:
            v.Start = startposition;
            v.End = new PointI(bounds.Right, bounds.Bottom);
            MoveAlongLine(v);

            // horizontal movement:
            v.Start = v.End;
            v.End = new PointI (bounds.Right, startposition.Y);
            MoveAlongLine(v);

            // vertical movement:
            v.Start = v.End;
            v.End = new PointI(startposition.X, bounds.Bottom);
            MoveAlongLine (v);

            v.Start = v.End;
            v.End = new PointI(bounds.Width/2, bounds.Bottom);
            MoveAlongLine(v);

            v.Start = v.End;
            v.End = new PointI(bounds.Width / 2, factory.distance.Height);
            MoveAlongLine(v);

            v.Start = v.End;
            v.End = startposition;
            MoveAlongLine(v);

            position = camera.FromSource (v.End);
            e = new MouseActionEventArgs(
                MouseActionButtons.Left, ModifierKeys.None, 
                0, position.X, position.Y, 0);
            Display.EventControler.OnMouseUp(e);
        }

        [Test]
        public void MoveAlongSceneBoundsTest() {
            string testName = "MoveAlongSceneBoundsTest";
            this.ReportDetail (testName);
            
            ticker.Start();
            RectangleI bounds = Scene.Shape.BoundsRect;
            this.ReportDetail ("Scene:\t" + bounds+"\t Display: \t"+Display.Bounds);
            MoveNode1 (bounds);
            
            // this test does not work with zoom!
            Display.ZoomState = ZoomState.Custom;
            float inc = 0.05f;
            while (Display.ZoomFactor < 1.5f) {
                Display.ZoomFactor = Display.ZoomFactor+inc;
                Display.UpdateZoom ();
                Application.DoEvents();
            }

            MoveLinks(bounds);

            while (Display.ZoomFactor < 2f) {
                Display.ZoomFactor = Display.ZoomFactor + inc;
                Display.UpdateZoom();
                Application.DoEvents();
            }

            MoveNode1(bounds);

            NeutralPosition();

            while (Display.ZoomFactor > 1f) {
                Display.ZoomFactor = Display.ZoomFactor - inc;
                Display.UpdateZoom();
                Application.DoEvents();
            }

            ticker.Stop ();
            this.ReportDetail (
                testName + " \t" +
                ticker.ElapsedInSec () + " sec \t" +
                ticker.FramePerSecond () + " fps \t"+
                ( (GDIWidgetLayer) Display.DataLayer ).sceneRenderer.iWidgets +" widgets \t"
                );
        }
    }
}
