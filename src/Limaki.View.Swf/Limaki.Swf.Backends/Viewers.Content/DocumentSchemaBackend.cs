using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.View.Visualizers;
using Limaki.Visuals;

namespace Limaki.View.Swf.Backends {
    // couldn't this replaced by WinformSplitView??
    public partial class DocumentSchemaBackend : UserControl, IZoomTarget {

        public SwfDocumentSchemaViewer Controller { get; set; }

        public DocumentSchemaBackend():this(new SwfDocumentSchemaViewer()) {}

        public DocumentSchemaBackend(SwfDocumentSchemaViewer controller) {
            this.Controller = controller;
            InitializeComponent();
            //this.SuspendLayout();
            this.WinVisualsDisplayBackend.TabStop = false;
            this.WinImageDisplayBackend.TabStop = false;
           
            Controller.GraphSceneDisplay = this.WinVisualsDisplayBackend.Display as IGraphSceneDisplay<IVisual, IVisualEdge>;
            Controller.ImageDisplay = this.WinImageDisplayBackend.Display;
            Controller.Compose();
            this.PerformLayout();
            Application.DoEvents();
            //this.SplitContainer.TabStop = false;
            //this.SplitContainer.KeyDown += new KeyEventHandler(SplitContainer_KeyDown);
        }

        void SplitContainer_KeyDown(object sender, KeyEventArgs e) {
            e.Handled = false;
        }
       

        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);
        }

        #region IZoomTarget Member

        public ZoomState ZoomState {
            get { return Controller.ImageDisplay.ZoomState; }
            set { Controller.ImageDisplay.ZoomState = value; }
        }

        public double ZoomFactor {
            get { return Controller.ImageDisplay.Viewport.ZoomFactor; }
            set { Controller.ImageDisplay.Viewport.ZoomFactor = value; }
        }

        public void UpdateZoom() {
            Controller.ImageDisplay.Viewport.UpdateZoom();
        }

        #endregion
    }
}
