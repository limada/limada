using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Widgets;

namespace Limaki.Widgets {
    public class MarkerContextProcessor:ContextProcessor<Scene> {
        public delegate IMarkerFacade<IWidget, IEdgeWidget> MarkerFacadeFactory (IGraph<IWidget, IEdgeWidget> graph)
            ;

        public MarkerFacadeFactory CreateMarkerFacade = null;
        public override void ApplyProperties(IApplicationContext context, Scene target) {
            if (CreateMarkerFacade != null) {
                target.Markers = CreateMarkerFacade(target.Graph);
            }
        }
    }
}