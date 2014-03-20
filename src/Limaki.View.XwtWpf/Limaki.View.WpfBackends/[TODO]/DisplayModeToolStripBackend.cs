using System.ComponentModel;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers.ToolStrips;

namespace Limaki.View.WpfBackends {
    public class DisplayModeToolStripBackend : ToolStripBackend, IDisplayModeToolStripBackend {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DisplayModeToolStrip Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (DisplayModeToolStrip)frontend;
            Compose();
        }

        private void Compose () {
            
        }
    }
}