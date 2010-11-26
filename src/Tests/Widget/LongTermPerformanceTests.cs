/*
 * Limaki 
 * Version 0.063
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


using System.Drawing;
using Limaki.Common;
using Limaki.Displays;
using Limaki.Drawing;
using Limaki.Widgets;
using Limaki.Actions;
using Limaki.Drawing.Shapes;
using NUnit.Framework;
using System.Windows.Forms;
using System;

namespace Limaki.Tests.Widget {
    public class LongTermPerformanceTests:WidgetDisplayTest {
        public LongTermPerformanceTests():base() {}
        public LongTermPerformanceTests(WidgetDisplay display):base(display) {}

        bool editorEnabled = false;
        bool dragDropEnabled = false;
        ILayout<Scene, IWidget> oldlayout = null;
        public override void Setup() {
            oldlayout = ((SceneCommandAction<Scene, IWidget>)Display.CommandAction).Layout;
            ((SceneCommandAction<Scene, IWidget>)Display.CommandAction).Layout = null;
            base.Setup();
            // this is neccessary as the mouse cursor returns after a long time
            // back to its position and activates WidgetTextEditor
            ((SceneCommandAction<Scene, IWidget>)Display.CommandAction).Layout =
                new LongtermPerformanceData.LongtermPerformanceLayout(
                    Display.displayKit.dataHandler,Data.styleSheet);
            ( (WidgetLayer) Display.DataLayer ).Layout =
                ( (SceneCommandAction<Scene, IWidget>) Display.CommandAction ).Layout;
            Data.Arrange (Display.Data);
            Display.CommandsInvoke ();
            editorEnabled = Display.WidgetTextEditor.Enabled;
            dragDropEnabled = Display.WidgetDragDrop.Enabled;
            Display.WidgetChanger.Enabled = true;
            Display.LinkWidgetChanger.Enabled = true;
            Display.WidgetTextEditor.Enabled = false;
            Display.WidgetDragDrop.Enabled = false;
            Display.AddWidgetAction.Enabled = false;
            Display.AddLinkAction.Enabled = false;


            ( (WidgetLayer) Display.DataLayer ).sceneRenderer.iWidgets = 0;
        }

        public override void TearDown() {
            base.TearDown();
            Display.WidgetTextEditor.Enabled = editorEnabled;
            Display.WidgetDragDrop.Enabled = dragDropEnabled;
            ( (SceneCommandAction<Scene, IWidget>) Display.CommandAction ).Layout = oldlayout;
        }

        LongtermPerformanceData Data = null;
        public override Scene Scene {
            get {
                if (_scene == null) {
                    Data = new LongtermPerformanceData ();
                    base.Scene = Data.Scene;
                }
                return base.Scene;
            }
            set {
                base.Scene = value;
            }
        }

        public void MoveLinks(Rectangle bounds) {
            MoveLink(Data.Link4,Data.Link1);
            MoveLink(Data.Link5, Data.Link3);
        }


        

        public void MoveNode1(Rectangle bounds) {
            NeutralPosition ();
            Point startposition = Data.Node1.Shape[Anchor.LeftTop]+new Size(10,0);
            Point position = transformer.FromSource(startposition);

            MouseEventArgs e = new MouseEventArgs(MouseButtons.Left, 0, position.X, position.Y, 0);
            Display.ActionDispatcher.OnMouseDown (e);

            Assert.AreSame (Scene.Selected, Data.Node1);

            
            Vector v = new Vector ();
            // diagonal movement:
            v.Start = startposition;
            v.End = new Point(bounds.Right, bounds.Bottom);
            MoveAlongLine(v);

            // horizontal movement:
            v.Start = v.End;
            v.End = new Point (bounds.Right, startposition.Y);
            MoveAlongLine(v);

            // vertical movement:
            v.Start = v.End;
            v.End = new Point(startposition.X, bounds.Bottom);
            MoveAlongLine (v);

            v.Start = v.End;
            v.End = new Point(bounds.Width/2, bounds.Bottom);
            MoveAlongLine(v);

            v.Start = v.End;
            v.End = new Point(bounds.Width / 2, Data.distance.Height);
            MoveAlongLine(v);

            v.Start = v.End;
            v.End = startposition;
            MoveAlongLine(v);

            position = transformer.FromSource (v.End);
            e = new MouseEventArgs(MouseButtons.Left, 0, position.X, position.Y, 0);
            Display.ActionDispatcher.OnMouseUp(e);
        }

        [Test]
        public void MoveAlongSceneBoundsTest() {
            string testName = "MoveAlongSceneBoundsTest";
            this.ReportMessage (testName);
            
            ticker.Start();
            Rectangle bounds = Scene.Shape.BoundsRect;
            this.ReportMessage ("bounds:\t" + bounds);
            MoveNode1 (bounds);
            
            // this test does not work with zoom!
            Display.ZoomState = Limaki.Actions.ZoomState.Custom;
            float inc = 0.05f;
            while (Display.ZoomFactor < 1.5f) {
                Display.ZoomFactor = Display.ZoomFactor+inc;
                Display.UpdateZoom ();
                Application.DoEvents ();
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
            this.ReportMessage (
                testName + " \t" +
                ticker.ElapsedInSec () + " sec \t" +
                ticker.FramePerSecond () + " fps \t"+
                ( (WidgetLayer) Display.DataLayer ).sceneRenderer.iWidgets +" widgets \t"
                );
        }
    }
}
