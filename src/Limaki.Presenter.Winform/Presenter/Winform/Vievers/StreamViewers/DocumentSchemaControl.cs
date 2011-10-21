using System.Windows.Forms;
using Limaki.Presenter.Display;
using Limaki.Visuals;

namespace Limaki.Presenter.Winform.Controls {
    // couldn't this replaced by WinformSplitView??
    public partial class DocumentSchemaControl : UserControl, IZoomTarget {
        public DocumentSchemaControl():this(new DocumentSchemaController()) {}

        public DocumentSchemaControl(DocumentSchemaController controller) {
            Controller = controller;
            InitializeComponent();
            //this.SuspendLayout();
            this.WinVisualsDisplay.TabStop = false;
            this.WinImageDisplay.TabStop = false;
           
            
            Controller.GraphSceneDisplay = this.WinVisualsDisplay.Display as IGraphSceneDisplay<IVisual, IVisualEdge>;
            Controller.ImageDisplay = this.WinImageDisplay.Display;
            Controller.Compose();
            this.PerformLayout();
            Application.DoEvents();
            //this.SplitContainer.TabStop = false;
            //this.SplitContainer.KeyDown += new KeyEventHandler(SplitContainer_KeyDown);
        }

        void SplitContainer_KeyDown(object sender, KeyEventArgs e) {
            e.Handled = false;
        }
        public DocumentSchemaController Controller { get; set; }


        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);
        }

        #region IZoomTarget Member

        public Drawing.ZoomState ZoomState {
            get { return Controller.ImageDisplay.ZoomState; }
            set { Controller.ImageDisplay.ZoomState = value; }
        }

        public float ZoomFactor {
            get { return Controller.ImageDisplay.Viewport.ZoomFactor; }
            set { Controller.ImageDisplay.Viewport.ZoomFactor = value; }
        }

        public void UpdateZoom() {
            Controller.ImageDisplay.Viewport.UpdateZoom();
        }

        #endregion
    }
}
