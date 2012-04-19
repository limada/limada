using Limaki.Common.IOC;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;

namespace Limaki.Visuals {

    public class MarkerContextProcessor : ContextProcessor<IGraphScene<IVisual, IVisualEdge>> {
        public delegate IMarkerFacade<IVisual, IVisualEdge> MarkerFacadeFactory (IGraph<IVisual, IVisualEdge> graph);

        public MarkerFacadeFactory CreateMarkerFacade = null;
        public override void ApplyProperties(IApplicationContext context, IGraphScene<IVisual, IVisualEdge> target) {
            if (CreateMarkerFacade != null) {
                target.Markers = CreateMarkerFacade(target.Graph);
            }
        }
    }
}