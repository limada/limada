using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Tests.View.Visuals;
using Limaki.View;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz.UI;
using Limaki.View.Viz.UI.GraphScene;
using Limaki.View.Viz.Visuals;
using NUnit.Framework;
using Xwt;
using System.Linq;
using Limaki.Common.Linqish;

namespace Limaki.Tests.View.Display {

    public class BenchmarkOneTests : VisualsDisplayTest {

        BenchmarkOneSceneFactory _factory = null;
        BenchmarkOneSceneFactory Factory {
            get {
                if (_factory == null) {
                    _factory = new BenchmarkOneSceneFactory ();
                }
                return _factory;

            }
        }

        public override IGraphScene<IVisual, IVisualEdge> Scene {
            get {
                if (_scene == null) {
                    base.Scene = Factory.Scene;
                }
                return base.Scene;
            }
            set {
                base.Scene = value;
            }
        }

        bool editorEnabled = false;
        bool dragDropEnabled = false;
        IGraphSceneLayout<IVisual, IVisualEdge> oldlayout = null;
        public override void Setup () {

            if (Display != null) {
                oldlayout = Display.Layout;
                Display.Layout = new BenchmarkOneSceneFactory.LongtermPerformanceSceneLayout (
                    () => { return Display.Data; }, Factory.styleSheet);
                Display.StyleSheet = Factory.styleSheet;
            }

            base.Setup ();

            Factory.Arrange (Display.Data);
            Display.Data.ClearSpatialIndex ();
            Display.Reset ();

            IAction action = Display.ActionDispatcher.GetAction<IEditAction> ();
            if (action != null) {
                editorEnabled = action.Enabled;
                action.Enabled = false;
            }

            action = Display.ActionDispatcher.GetAction<GraphItemMoveResizeAction<IVisual, IVisualEdge>> ();
            if (action != null)
                action.Enabled = true;
            action = Display.ActionDispatcher.GetAction<GraphEdgeChangeAction<IVisual, IVisualEdge>> ();
            if (action != null)
                action.Enabled = true;

            // this is neccessary as the mouse cursor returns after a long time
            // back to its position and activates VisualsTextEditor

            action = Display.ActionDispatcher.GetAction<GraphItemAddAction<IVisual, IVisualEdge>> ();
            action.Enabled = false;
            action = Display.ActionDispatcher.GetAction<AddVisualEdgeAction> ();
            action.Enabled = false;

            Display.ActionDispatcher.Actions.Values
                .OfType<IDropAction> ()
                .ForEach (a => a.Enabled = false);

        }

        public override void TearDown () {
            base.TearDown ();
            IAction action = Display.ActionDispatcher.GetAction<IEditAction> ();
            if (action != null)
                action.Enabled = editorEnabled;
            Display.ActionDispatcher.Actions.Values
                .OfType<IDropAction> ()
                .ForEach (a => a.Enabled = true);
            if (oldlayout != null)
                Display.Layout = oldlayout;
        }



        public void MoveLinks (Rectangle bounds) {
            MoveLink (Factory.Edges[4], Factory.Edges[1]);
            MoveLink (Factory.Edges[5], Factory.Edges[3]);
        }

        public void MoveNode1 (Rectangle bounds) {
            NeutralPosition ();
            var startposition = Factory.Nodes[1].Shape[Anchor.LeftTop] + new Size (10, 0);
            var position = camera.FromSource (startposition);

            var e =
                new MouseActionEventArgs (
                    MouseActionButtons.Left, ModifierKeys.None,
                    0, position.X, position.Y, 0);
            Display.ActionDispatcher.OnMouseDown (e);

            Assert.AreSame (Scene.Focused, Factory.Nodes[1]);


            var v = new Vector ();
            // diagonal movement:
            v.Start = startposition;
            v.End = new Point (bounds.Right, bounds.Bottom);
            MoveAlongLine (v);

            // horizontal movement:
            v.Start = v.End;
            v.End = new Point (bounds.Right, startposition.Y);
            MoveAlongLine (v);

            // vertical movement:
            v.Start = v.End;
            v.End = new Point (startposition.X, bounds.Bottom);
            MoveAlongLine (v);

            v.Start = v.End;
            v.End = new Point (bounds.Width / 2, bounds.Bottom);
            MoveAlongLine (v);

            v.Start = v.End;
            v.End = new Point (bounds.Width / 2, Factory.distance.Height);
            MoveAlongLine (v);

            v.Start = v.End;
            v.End = startposition;
            MoveAlongLine (v);

            position = camera.FromSource (v.End);
            e = new MouseActionEventArgs (
                MouseActionButtons.Left, ModifierKeys.None,
                0, position.X, position.Y, 0);
            Display.ActionDispatcher.OnMouseUp (e);
        }

        [Test]
        public void MoveAlongSceneBoundsTest () {

            var visualsCount = VisualsCount ();

            string testName = "MoveAlongSceneBoundsTest";
            this.ReportDetail (testName);

            ticker.Start ();
            var bounds = Scene.Shape.BoundsRect;

            this.ReportDetail ("Scene:\t" + bounds + "\t Display: \t" + Display.Viewport.ClipSize);
            MoveNode1 (bounds);

            Display.ZoomState = ZoomState.Custom;

            var viewport = Display.Viewport;
            float inc = 0.05f;
            while (viewport.ZoomFactor < 1.5f) {
                viewport.ZoomFactor = viewport.ZoomFactor + inc;
                viewport.UpdateZoom ();
                DoEvents ();
            }

            MoveLinks (bounds);

            while (viewport.ZoomFactor < 2f) {
                viewport.ZoomFactor = viewport.ZoomFactor + inc;
                viewport.UpdateZoom ();
                DoEvents ();
            }

            MoveNode1 (bounds);

            NeutralPosition ();

            while (viewport.ZoomFactor > 1f) {
                viewport.ZoomFactor = viewport.ZoomFactor - inc;
                viewport.UpdateZoom ();
                DoEvents ();
            }

            ticker.Stop ();
            visualsCount = VisualsCount () - visualsCount;
            this.ReportDetail (
                testName + " \t" +
                ticker.ElapsedInSec () + " sec \t" +
                ticker.FramePerSecond () + " fps \t" +
                visualsCount + " visuals \t"
                );
        }
    }
}