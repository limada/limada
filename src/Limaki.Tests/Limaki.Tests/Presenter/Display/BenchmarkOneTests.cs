/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Presenter.UI;
using Limaki.Presenter.UI.GraphScene;
using Limaki.Presenter.Visuals.UI;
using Limaki.Tests.Graph.Model;
using Limaki.Visuals;
using NUnit.Framework;
using Xwt;

namespace Limaki.Tests.Presenter.Display {
    public class UITests : VisualsDisplayTest {
        [Test]
        public void MultiSelectorAgainstMoveTest() {
            

        }

    }
    public class BenchmarkOneTests:VisualsDisplayTest {

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
        IGraphLayout<IVisual,IVisualEdge> oldlayout = null;
        public override void Setup() {
            
            if (Display != null) {
                oldlayout = Display.Layout;
                Display.Layout = new BenchmarkOneSceneFactory.LongtermPerformanceLayout(
                                    () => { return Display.Data; }, factory.styleSheet);
                Display.StyleSheet = factory.styleSheet;
            }

            base.Setup();
            
            factory.Arrange (Display.Data as Scene);
            Display.Invoke ();

            var editAction = Display.EventControler.GetAction<IEditAction> ();
            editorEnabled = editAction.Enabled;
            var dragDropAction = Display.EventControler.GetAction<IDragDopActionPresenter>();
            dragDropEnabled = dragDropAction.Enabled;

            IAction action = Display.EventControler.GetAction<GraphItemMoveResizeAction<IVisual, IVisualEdge>>();
            action.Enabled = true;
            action = Display.EventControler.GetAction<GraphEdgeChangeAction<IVisual, IVisualEdge>>();
            action.Enabled = true;
            
            // this is neccessary as the mouse cursor returns after a long time
            // back to its position and activates VisualsTextEditor

            editAction.Enabled = false;
            dragDropAction.Enabled = false;

            action = Display.EventControler.GetAction<GraphItemAddAction<IVisual,IVisualEdge>>();
            action.Enabled = false;
            action = Display.EventControler.GetAction < AddEdgeAction>();
            action.Enabled = false;



        }

        public override void TearDown() {
            base.TearDown();
            IAction action = Display.EventControler.GetAction<IEditAction>();
            action.Enabled = editorEnabled;
            action = Display.EventControler.GetAction<IDragDopActionPresenter>();
            action.Enabled = dragDropEnabled;
            if (oldlayout != null)
                Display.Layout = oldlayout;
        }



        public void MoveLinks(RectangleD bounds) {
            MoveLink(factory.Edge[4],factory.Edge[1]);
            MoveLink(factory.Edge[5], factory.Edge[3]);
        }


        

        public void MoveNode1(RectangleD bounds) {
            NeutralPosition ();
            Point startposition = factory.Node[1].Shape[Anchor.LeftTop]+new Size(10,0);
            Point position = camera.FromSource(startposition);

            MouseActionEventArgs e = 
                new MouseActionEventArgs(
                    MouseActionButtons.Left, ModifierKeys.None, 
                    0, position.X, position.Y, 0);
            Display.EventControler.OnMouseDown (e);

            Assert.AreSame (Scene.Focused, factory.Node[1]);

            
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
            v.End = new Point(bounds.Width / 2, factory.distance.Height);
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
            var visualsCount = VisualsCount ();

            string testName = "MoveAlongSceneBoundsTest";
            this.ReportDetail (testName);
            
            ticker.Start();
            RectangleD bounds = Scene.Shape.BoundsRect;

            this.ReportDetail ("Scene:\t" + bounds+"\t Display: \t"+Display.Viewport.ClipSize);
            MoveNode1 (bounds);
            
            // this test does not work with zoom!
            Display.ZoomState = ZoomState.Custom;

            var viewport = Display.Viewport;
            float inc = 0.05f;
            while (viewport.ZoomFactor < 1.5f) {
                viewport.ZoomFactor = viewport.ZoomFactor + inc;
                viewport.UpdateZoom ();
                DoEvents();
            }

            MoveLinks(bounds);

            while (viewport.ZoomFactor < 2f) {
                viewport.ZoomFactor = viewport.ZoomFactor + inc;
                viewport.UpdateZoom();
                DoEvents();
            }

            MoveNode1(bounds);

            NeutralPosition();

            while (viewport.ZoomFactor > 1f) {
                viewport.ZoomFactor = viewport.ZoomFactor - inc;
                viewport.UpdateZoom();
                DoEvents();
            }

            ticker.Stop ();
            visualsCount = VisualsCount () - visualsCount;
            this.ReportDetail (
                testName + " \t" +
                ticker.ElapsedInSec () + " sec \t" +
                ticker.FramePerSecond () + " fps \t"+
                    visualsCount +" visuals \t"
                );
        }
    }
}
