using Limada.Model;
using Limaki.Graphs;
using Limaki.Widgets;

namespace Limaki.UseCases.Viewers {
    public abstract class ThingViewerController:ViewerController {
        public abstract bool Supports(IGraph<IWidget, IEdgeWidget> graph, IWidget widget);
        public abstract void SetContent(IGraph<IWidget, IEdgeWidget> graph, IWidget widget);
    }
}