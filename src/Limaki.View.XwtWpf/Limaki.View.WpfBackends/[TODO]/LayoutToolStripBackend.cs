using Limaki.Viewers.ToolStripViewers;

namespace Limaki.View.WpfBackends {
    public class LayoutToolStripBackend : ToolStripBackend, ILayoutToolStripBackend {

        public LayoutToolStrip Frontend { get; set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (LayoutToolStrip)frontend;
            Compose();
        }

        private void Compose () {
           
        }

        public void AttachStyleSheet (string sheetName) {

        }

        public void DetachStyleSheet (string sheetName) {

        }
    }
}