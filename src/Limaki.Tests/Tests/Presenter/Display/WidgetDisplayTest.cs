using System;
using Limaki.Drawing;
using Limaki.Presenter.UI;
using Limaki.Tests.Graph.Model;
using Limaki.Widgets;
using NUnit.Framework;
using Limaki.Presenter.Display;

namespace Limaki.Tests.Presenter.Display {
    public class WidgetDisplayTest:DisplayTest<IGraphScene<IWidget,IEdgeWidget>> {
        protected Scene _scene = null;
        public virtual Scene Scene {
            get {
                if (_scene == null) {
                    _scene = new Scene();
                }
                return _scene;
            }
            set { _scene = value; }
        }

        public virtual new IGraphSceneDisplay<IWidget,IEdgeWidget> Display {
            get { return base.Display as IGraphSceneDisplay<IWidget, IEdgeWidget>; }
            set { base.Display  = value as IGraphSceneDisplay<IWidget, IEdgeWidget>; }
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
            DoEvents();

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