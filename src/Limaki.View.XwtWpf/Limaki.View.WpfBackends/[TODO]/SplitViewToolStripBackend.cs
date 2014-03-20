using System.ComponentModel;
using Limada.View.Vidgets;
using Limaki.Usecases.Vidgets;
using Limaki.View.Vidgets;

namespace Limaki.View.WpfBackends {
    public class SplitViewToolStripBackend : ToolStripBackend, ISplitViewToolStripBackend {

        public override void InitializeBackend (Limaki.View.IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (SplitViewToolStrip)frontend;
            Compose();
        }

        private void Compose () {
            
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SplitViewToolStrip Frontend { get; protected set; }

        public SplitViewMode ViewMode { get; set; }

        public void CheckBackForward (ISplitView splitView) {

        }

        public void AttachSheets () {

        }
    }
}