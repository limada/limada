using Limaki.Graphs;
using Limaki.Visuals;

namespace Limaki.UseCases.Viewers {
    public abstract class ThingViewerController:ViewerController {
        public abstract bool Supports(IGraph<IVisual, IVisualEdge> graph, IVisual visual);
        public abstract void SetContent(IGraph<IVisual, IVisualEdge> graph, IVisual visual);
    }
}