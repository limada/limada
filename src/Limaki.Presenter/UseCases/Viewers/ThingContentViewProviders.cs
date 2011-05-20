using System.Linq;
using Limada.Model;
using Limaki.Visuals;
using Limaki.Graphs;

namespace Limaki.UseCases.Viewers {
    public class ThingContentViewProviders:ContentViewProviders {
        public ThingViewerController Supports(IGraph<IVisual, IVisualEdge> graph, IVisual thing) {
            return Viewers.OfType<ThingViewerController>().Where(v => v.Supports(graph, thing)).FirstOrDefault();
        }
    }
}