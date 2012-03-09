using System.Linq;
using Limaki.Graphs;
using Limaki.Visuals;

namespace Limaki.Viewers {
    public class ThingContentViewProviders:ContentViewProviders {
        public ThingViewerController Supports(IGraph<IVisual, IVisualEdge> graph, IVisual thing) {
            return Viewers.OfType<ThingViewerController>().Where(v => v.Supports(graph, thing)).FirstOrDefault();
        }
    }
}