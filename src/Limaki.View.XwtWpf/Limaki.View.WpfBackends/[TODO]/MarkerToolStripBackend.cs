using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz.Visualizers.ToolStrips;

namespace Limaki.View.WpfBackends {
    public class MarkerToolStripBackend : ToolStripBackend, IMarkerToolStripBackend {

        public MarkerToolStrip Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (MarkerToolStrip)frontend;
        }

        public void Attach (IGraphScene<IVisual, IVisualEdge> scene) {

        }

        public void Detach (IGraphScene<IVisual, IVisualEdge> oldScene) {

        }
    }
}