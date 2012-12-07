using System;
using Limaki.Drawing;
using Limaki.View.UI;
using Limaki.Tests.Graph.Model;
using Limaki.Visuals;
using NUnit.Framework;
using Limaki.View.Visualizers;
using Xwt;

namespace Limaki.Tests.View.Display {
    public class VisualsDisplayTest:DisplayTest<IGraphScene<IVisual,IVisualEdge>> {
        protected IGraphScene<IVisual, IVisualEdge> _scene = null;
        public virtual IGraphScene<IVisual, IVisualEdge> Scene {
            get {
                if (_scene == null) {
                    _scene = new Scene();
                }
                return _scene;
            }
            set { _scene = value; }
        }

        public virtual new IGraphSceneDisplay<IVisual,IVisualEdge> Display {
            get { return base.Display as IGraphSceneDisplay<IVisual, IVisualEdge>; }
            set { base.Display  = value as IGraphSceneDisplay<IVisual, IVisualEdge>; }
        }

        bool zoomEnabled = false;
        bool trackerEnabled = false;
        bool selectorEnabled = false;
        protected virtual void InitDisplay() {
            this.Display.ZoomState = ZoomState.Original;
            this.Display.Data = this.Scene;
            var zoomAction = Display.EventControler.GetAction<ZoomAction> ();
            zoomEnabled = zoomAction.Enabled;
            zoomAction.Enabled = false;
            trackerEnabled = Display.MouseScrollAction.Enabled;
            Display.MouseScrollAction.Enabled = false;
            selectorEnabled = Display.SelectAction.Enabled;
            Display.SelectAction.Enabled = false;
        }

        public override void TearDown() {
            var zoomAction = Display.EventControler.GetAction<ZoomAction> ();
            zoomAction.Enabled = zoomEnabled;
            Display.MouseScrollAction.Enabled = trackerEnabled;
            Display.SelectAction.Enabled = selectorEnabled;
            base.TearDown();
        }

        public override void Setup() {
            base.Setup();
            InitDisplay ();
        }



        public void MoveLink(IVisual link, IVisual target) {
            NeutralPosition();

            Size size = new Size(Math.Abs(link.Shape.Size.Width) / 4, -Math.Abs(link.Shape.Size.Height) / 4);
            Point position = link.Shape[Anchor.Center] + size;
            position = camera.FromSource(position);
            //new Size(5, (int)(m * 5));

            // select link
            MouseActionEventArgs e = 
                new MouseActionEventArgs(MouseActionButtons.Left,
                    ModifierKeys.None, 0, position.X, position.Y, 0);
            Display.EventControler.OnMouseDown(e);
            DoEvents();

            Assert.AreSame(Scene.Focused, link);

            Vector v = new Vector();
            v.Start = link.Shape[Anchor.LeftTop];
            v.End = target.Shape[Anchor.Center];
            //m = M(v);
            size = new Size(Math.Abs(target.Shape.Size.Width) / 4, -Math.Abs(target.Shape.Size.Height) / 4);
            v.End = v.End + size;//new Size((int)(m*3),-3);

            // start move
            position = camera.FromSource(v.Start);
            e = new MouseActionEventArgs(MouseActionButtons.Left, 
                ModifierKeys.None, 
                0, position.X, position.Y, 0);
            Display.EventControler.OnMouseDown(e);
            DoEvents();

            MoveAlongLine(v);

            // end move
            position = camera.FromSource(v.End);
            e = new MouseActionEventArgs(MouseActionButtons.Left, ModifierKeys.None, 
                0, position.X, position.Y, 0);
            Assert.AreSame(Scene.Hovered, target);
            Display.EventControler.OnMouseUp(e);
            DoEvents();



        }
    }
}