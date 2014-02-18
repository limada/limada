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
 * http://www.limada.org
 * 
 */


using Limaki.Actions;
using Limaki.Drawing;
using Limaki.View.UI;
using Limaki.View.UI.GraphScene;
using Limaki.View.Visuals.UI;
using Limaki.Tests.Graph.Model;
using Limaki.Visuals;
using NUnit.Framework;
using Xwt;
using System;

namespace Limaki.Tests.View.Display {
    [Obsolete]
    public class BenchmarkOneTests008:VisualsDisplayTest008 {

        BenchmarkOneSceneFactory _factory = null;
        BenchmarkOneSceneFactory factory {
            get {
                if (_factory == null) {
                    _factory = new BenchmarkOneSceneFactory();
                }
                return _factory;

            }
        }

        public override IGraphScene<IVisual, IVisualEdge> Scene {
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
        IGraphSceneLayout<IVisual,IVisualEdge> oldlayout = null;
        public override void Setup() {
            
            if (Display != null) {
                oldlayout = Display.Layout;
                Display.Layout = new BenchmarkOneSceneFactory.LongtermPerformanceSceneLayout(
                                    () => { return Display.Data; }, factory.styleSheet);
                Display.StyleSheet = factory.styleSheet;
            }

            base.Setup();
            
            factory.Arrange (Display.Data);
            Display.Data.ClearSpatialIndex();
            Display.Reset ();

            IAction action = Display.EventControler.GetAction<IEditAction>();
            if (action != null) {
                editorEnabled = action.Enabled;
                action.Enabled = false;
            }
            action = Display.EventControler.GetAction<IDropAction>();
            if (action != null) {
                dragDropEnabled = action.Enabled;
                action.Enabled = false;
            }
            action = Display.EventControler.GetAction<GraphItemMoveResizeAction<IVisual, IVisualEdge>>();
            if (action != null)
            action.Enabled = true;
            action = Display.EventControler.GetAction<GraphEdgeChangeAction<IVisual, IVisualEdge>>();
            if (action != null)
            action.Enabled = true;
            
            // this is neccessary as the mouse cursor returns after a long time
            // back to its position and activates VisualsTextEditor

            action = Display.EventControler.GetAction<GraphItemAddAction<IVisual,IVisualEdge>>();
            action.Enabled = false;
            action = Display.EventControler.GetAction < AddEdgeAction>();
            action.Enabled = false;



        }

        public override void TearDown () {
            base.TearDown();
            IAction action = Display.EventControler.GetAction<IEditAction>();
            if (action != null)
                action.Enabled = editorEnabled;
            action = Display.EventControler.GetAction<IDropAction>();
            if (action != null)
                action.Enabled = dragDropEnabled;
            if (oldlayout != null)
                Display.Layout = oldlayout;
        }



        public void MoveLinks(Rectangle bounds) {
            MoveLink(factory.Edge[4],factory.Edge[1]);
            MoveLink(factory.Edge[5], factory.Edge[3]);
        }


        

        public void MoveNode1(Rectangle bounds) {
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
            Rectangle bounds = Scene.Shape.BoundsRect;

            this.ReportDetail ("Scene:\t" + bounds+"\t Display: \t"+Display.Viewport.ClipSize);
            MoveNode1 (bounds);
            
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
