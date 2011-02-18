using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Widgets;
using Limaki.Drawing;

namespace Limaki.Widgets {
    public class MarkerContextProcessor : ContextProcessor<IGraphScene<IWidget, IEdgeWidget>> {
        public delegate IMarkerFacade<IWidget, IEdgeWidget> MarkerFacadeFactory (IGraph<IWidget, IEdgeWidget> graph)
            ;

        public MarkerFacadeFactory CreateMarkerFacade = null;
        public override void ApplyProperties(IApplicationContext context, IGraphScene<IWidget, IEdgeWidget> target) {
            if (CreateMarkerFacade != null) {
                (target as Scene).Markers = CreateMarkerFacade(target.Graph);
            }
        }
    }
}