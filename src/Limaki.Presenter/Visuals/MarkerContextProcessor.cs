using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Visuals;
using Limaki.Drawing;

namespace Limaki.Visuals {
    public class MarkerContextProcessor : ContextProcessor<IGraphScene<IVisual, IVisualEdge>> {
        public delegate IMarkerFacade<IVisual, IVisualEdge> MarkerFacadeFactory (IGraph<IVisual, IVisualEdge> graph)
            ;

        public MarkerFacadeFactory CreateMarkerFacade = null;
        public override void ApplyProperties(IApplicationContext context, IGraphScene<IVisual, IVisualEdge> target) {
            if (CreateMarkerFacade != null) {
                (target as Scene).Markers = CreateMarkerFacade(target.Graph);
            }
        }
    }
}