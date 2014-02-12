using Limaki.Viewers.ToolStripViewers;
using System.ComponentModel;

namespace Limaki.View.WpfBackends {
    public class DisplayModeToolStripBackend : ToolStripBackend, IDisplayModeToolStripBackend {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DisplayModeToolStrip Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, Limaki.View.VidgetApplicationContext context) {
            this.Frontend = (DisplayModeToolStrip)frontend;
            Compose();
        }

        private void Compose () {
            
        }
    }
}