using Limada.Model;
using Limada.View;
using Limada.VisualThings;
using Limaki.Common.IOC;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Visuals;

namespace Limaki.View {
    /// <summary>
    /// applies VisualThings
    /// </summary>
    public class VisualThingsResourceLoader : IContextResourceLoader {

        public virtual void ApplyResources (IApplicationContext context) {

            context.Factory.Add<GraphItemTransformer<IVisual, IThing, IVisualEdge, ILink>, VisualThingTransformer> ();
            context.Factory.Add<GraphItemTransformer<IThing, IVisual, ILink, IVisualEdge>> (t => new VisualThingTransformer ().Reverted ());
            
            GraphMapping.ChainGraphMapping<VisualThingGraphMapping> (context);

            context.Factory.Add<IVisualContentViz, VisualThingsContentViz> ();
            context.Factory.Add<IVisualContentViz<IThing>, VisualThingsContentViz> ();

            context.Factory.Add<ISheetManager, SheetManager> ();

            var markerProcessor = context.Pool.TryGetCreate<MarkerContextProcessor> ();
            markerProcessor.CreateMarkerFacade = graph => {
                if (graph.Source<IVisual, IVisualEdge, IThing, ILink> () != null) {
                    return new VisualThingMarkerFacade (graph);
                }

                return null;
            };
        }
    }
}