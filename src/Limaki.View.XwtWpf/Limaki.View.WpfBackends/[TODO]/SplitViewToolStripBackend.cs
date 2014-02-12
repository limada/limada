using Limaki.Viewers.ToolStripViewers;
using System.ComponentModel;

namespace Limaki.View.WpfBackends {
    public class SplitViewToolStripBackend : ToolStripBackend, ISplitViewToolStripBackend {

        public override void InitializeBackend (Limaki.View.IVidget frontend, Limaki.View.VidgetApplicationContext context) {
            this.Frontend = (SplitViewToolStrip)frontend;
            Compose();
        }

        private void Compose () {
            
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SplitViewToolStrip Frontend { get; protected set; }

        public Viewers.SplitViewMode ViewMode { get; set; }

        public void CheckBackForward (Viewers.ISplitView splitView) {

        }

        public void AttachSheets () {

        }
    }
}