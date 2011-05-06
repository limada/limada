using System.Windows.Forms;
using Limaki.Presenter.Display;
using Limaki.Widgets;

namespace Limaki.Presenter.Winform.Controls {
    // couldn't this replaced by WinformSplitView??
    public partial class DocumentSchemaControl : UserControl, IZoomTarget {
        public DocumentSchemaControl():this(new DocumentSchemaController()) {}

        public DocumentSchemaControl(DocumentSchemaController controller) {
            Controller = controller;
            InitializeComponent();
            Controller.GraphSceneDisplay = this.WidgetDisplay.Display as IGraphSceneDisplay<IWidget, IEdgeWidget>;
            Controller.ImageDisplay = this.WinImageDisplay.Display;
            Controller.Compose();
            this.PerformLayout();
            Application.DoEvents();
        }
        public DocumentSchemaController Controller { get; set; }


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
