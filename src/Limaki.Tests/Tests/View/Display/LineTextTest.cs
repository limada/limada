using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.Tests.Graph.Model;
using Limaki.Drawing.UI;
using Limaki.Widgets;
using Limaki.Winform.Displays;
using NUnit.Framework;

namespace Limaki.Tests.Widget {
    public class LineTextTest : WidgetDisplayTest {
        public LineTextTest() : base() { }
        public LineTextTest(WidgetDisplay display) : base(display) { }
        ISceneFactory Data = null;
        public override Scene Scene {
            get {
                if (_scene == null) {
                    Data = new SceneFactory<SimpleGraphFactory>();
                    base.Scene = Data.Scene;
                }
                return base.Scene;
            }
            set {
                base.Scene = value;
            }
        }
        WidgetBoundsLayer widgetBoundsLayer = null;
        public override void Setup() {
            base.Setup();
            if (!Display.EventControler.Actions.ContainsKey(typeof(WidgetBoundsLayer))) {
                widgetBoundsLayer = new WidgetBoundsLayer(Display, Display);
                Display.EventControler.Add (widgetBoundsLayer);
            } else {
                widgetBoundsLayer = Display.EventControler.GetAction<WidgetBoundsLayer> ();
            }
            widgetBoundsLayer.Data = Display.Data;
            widgetBoundsLayer.Layout = (ILayout<Scene, IWidget>)Display.LayoutControler.Layout;
            
        }
        public override void TearDown() {
            base.TearDown();
            //Display.EventControler.Remove (widgetBoundsLayer);
        }
        [Test]
        public void LineTextHover() {
            this.ReportDetail ("***** LineTextHover start");
            Display.ZoomAction.ZoomIn();
            Display.ZoomAction.ZoomIn();
            Display.UpdateZoom();
            NeutralPosition();
            this.ReportDetail ("***** Zoomend and Neutral");
            PointI lineCenter = 
                Data.Edge[1].Shape[Anchor.Center];
            lineCenter = Display.DataLayer.Camera.FromSource (lineCenter);
            MouseActionEventArgs e =
                new MouseActionEventArgs(MouseActionButtons.Left, 0, lineCenter.X, lineCenter.Y, 0);
            //Display.EventControler.OnMouseDown(e);
            //Application.DoEvents();
            this.ReportDetail("***** MouseMove");
            Display.EventControler.OnMouseMove(e);
            Display.Invalidate ();
            Application.DoEvents();
            //Display.EventControler.OnMouseUp(e);
            //Application.DoEvents();
            //NeutralPosition();
            
        }

        void Display_Paint(object sender, PaintEventArgs e) {
            ILayout<Scene, IWidget> layout = (ILayout<Scene, IWidget>) Display.LayoutControler.Layout;
            PointI[] hull = 
                layout.GetDataHull (Data.Edge[1], Display.DataLayer.Camera.Matrice, 0, true);

        }
    }
}