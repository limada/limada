using System.Linq;
using Limada.Model;
using Limaki.Widgets;
using Limaki.Graphs;

namespace Limaki.UseCases.Viewers {
    public class ThingContentViewProviders:ContentViewProviders {
        public ThingViewerController Supports(IGraph<IWidget, IEdgeWidget> graph, IWidget thing) {
            return Viewers.OfType<ThingViewerController>().Where(v => v.Supports(graph, thing)).FirstOrDefault();
        }
    }
}