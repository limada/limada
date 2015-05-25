using System;
using Limaki.Drawing;
using Limaki.View;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.UI;
using Limaki.View.Viz.Visualizers;
using Limaki.View.Viz.Visuals;
using NUnit.Framework;
using Xwt;

namespace Limaki.Tests.View.Display {

    public class VisualsDisplayTest : DisplayTest<IGraphScene<IVisual, IVisualEdge>> {
        protected IGraphScene<IVisual, IVisualEdge> _scene = null;
        public virtual IGraphScene<IVisual, IVisualEdge> Scene {
            get {
                if (_scene == null) {
                    _scene = new Scene ();
                }
                return _scene;
            }
            set { _scene = value; }
        }

        public virtual new IGraphSceneDisplay<IVisual, IVisualEdge> Display {
            get { return base.Display as IGraphSceneDisplay<IVisual, IVisualEdge>; }
            set { base.Display = value as IGraphSceneDisplay<IVisual, IVisualEdge>; }
        }

        bool zoomEnabled = false;
        bool trackerEnabled = false;
        bool selectorEnabled = false;

        protected virtual void InitDisplay () {
            this.Display.ZoomState = ZoomState.Original;
            this.Display.Data = this.Scene;
            var zoomAction = Display.ActionDispatcher.GetAction<ZoomAction> ();
            zoomEnabled = zoomAction.Enabled;
            zoomAction.Enabled = false;
            trackerEnabled = Display.MouseScrollAction.Enabled;
            Display.MouseScrollAction.Enabled = false;
            selectorEnabled = Display.SelectAction.Enabled;
            Display.SelectAction.Enabled = false;
        }

        public override void TearDown () {
            var zoomAction = Display.ActionDispatcher.GetAction<ZoomAction> ();
            zoomAction.Enabled = zoomEnabled;
            Display.MouseScrollAction.Enabled = trackerEnabled;
            Display.SelectAction.Enabled = selectorEnabled;
            base.TearDown ();
        }

        public override void Setup () {
            if (CreateDisplay == null)
                CreateDisplay = () => new VisualsDisplay ();
            base.Setup ();
            InitDisplay ();
        }

        public void MoveLink (IVisual link, IVisual target) {
            NeutralPosition ();

            var size = new Size (Math.Abs (link.Shape.Size.Width) / 4, -Math.Abs (link.Shape.Size.Height) / 4);
            var position = link.Shape[Anchor.Center] + size;
            position = camera.FromSource (position);
            //new Size(5, (int)(m * 5));

            // select link
            var e =
                new MouseActionEventArgs (MouseActionButtons.Left,
                                          ModifierKeys.None, 0, position.X, position.Y, 0);
            Display.ActionDispatcher.OnMouseDown (e);
            DoEvents ();

            Assert.AreSame (Scene.Focused, link);

            var v = new Vector ();
            v.Start = link.Shape[Anchor.LeftTop];
            v.End = target.Shape[Anchor.Center];
            //m = M(v);
            size = new Size (Math.Abs (target.Shape.Size.Width) / 4, -Math.Abs (target.Shape.Size.Height) / 4);
            v.End = v.End + size;//new Size((int)(m*3),-3);

            // start move
            position = camera.FromSource (v.Start);
            e = new MouseActionEventArgs (MouseActionButtons.Left,
                                          ModifierKeys.None,
                                          0, position.X, position.Y, 0);
            Display.ActionDispatcher.OnMouseDown (e);
            DoEvents ();

            MoveAlongLine (v);

            Assert.AreSame (Scene.Hovered, target);

            // end move
            position = camera.FromSource (v.End);
            e = new MouseActionEventArgs (MouseActionButtons.Left, ModifierKeys.None,
                                          0, position.X, position.Y, 0);
            Display.ActionDispatcher.OnMouseUp (e);
            DoEvents ();



        }
    }
}