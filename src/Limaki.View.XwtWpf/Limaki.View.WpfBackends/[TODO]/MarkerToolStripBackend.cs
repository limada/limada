using Limaki.Viewers.ToolStripViewers;
using Limaki.Visuals;

namespace Limaki.View.WpfBackends {
    public class MarkerToolStripBackend : ToolStripBackend, IMarkerToolStripBackend {

        public MarkerToolStrip Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (MarkerToolStrip)frontend;
        }

        public void Attach (Drawing.IGraphScene<IVisual, IVisualEdge> scene) {

        }

        public void Detach (Drawing.IGraphScene<Limaki.Visuals.IVisual, Limaki.Visuals.IVisualEdge> oldScene) {

        }
    }
}