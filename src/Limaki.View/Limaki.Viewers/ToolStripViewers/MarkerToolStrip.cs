using Limaki.Drawing;
using Limaki.View.Visualizers;
using Limaki.View.Visuals.UI;
using Limaki.Visuals;

namespace Limaki.Viewers.ToolStripViewers {

    public interface IMarkerToolStripBackend : IToolStripViewerBackend {
        void Attach (IGraphScene<IVisual, IVisualEdge> scene);
        void Detach (IGraphScene<IVisual, IVisualEdge> oldScene);
    }

    public class MarkerToolStrip : ToolStripViewer<IGraphSceneDisplay<IVisual, IVisualEdge>, IMarkerToolStripBackend> {
        
        public override void Attach (object sender) {
            var display = sender as IGraphSceneDisplay<IVisual, IVisualEdge>;
            if (display != null) {
                this.CurrentDisplay = display;
                Backend.Attach(display.Data);
            }
        }

        public override void Detach (object sender) {
            this.CurrentDisplay = null;
        }

        public virtual void ChangeMarkers (string marker) {
            var display = CurrentDisplay;
            if (display != null) {
                var scene = display.Data;
                if (scene.Markers != null) {
                    SceneExtensions.ChangeMarkers(scene, scene.Selected.Elements, marker);
                }
                display.Execute();
            }
        }
    }
}