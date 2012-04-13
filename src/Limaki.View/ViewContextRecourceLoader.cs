using Limaki.Common.IOC;
using Limaki.Drawing.Styles;
using Limaki.Graphs.Extensions;
using Limada.Model;
using Limaki.Graphs;
using Limada.View;
using Limada.VisualThings;
using Limaki.View.Display;
using Limaki.View.Visuals.Display;
using Limaki.Visuals;

namespace Limaki.IOC {

    public class ViewContextRecourceLoader : IContextRecourceLoader {
        /// <summary>
        /// Attention! Before calling this ResourceLoader, all DrawingFactories
        /// have to be loaded! 
        /// </summary>
        /// <param name="context"></param>
        public virtual void ApplyResources (IApplicationContext context) {
           
            var styleSheets = context.Pool.TryGetCreate<StyleSheets> ();
            styleSheets.Init ();

            GraphMapping.ChainGraphMapping<GraphItemVisualMapping> (context);
            GraphMapping.ChainGraphMapping<VisualThingGraphMapping> (context);

            var markerProcessor =
                context.Pool.TryGetCreate<MarkerContextProcessor> ();
            markerProcessor.CreateMarkerFacade = this.MarkerFacade;

            var displayRecourceLoader = new DisplayRecourceLoader ();
            displayRecourceLoader.ApplyResources (context);

            var visualsRecourceLoader = new VisualsRecourceLoader ();
            visualsRecourceLoader.ApplyResources (context);

            context.Factory.Add<ISheetManager, SheetManager> ();

        }

        public virtual IMarkerFacade<IVisual, IVisualEdge> MarkerFacade (IGraph<IVisual, IVisualEdge> graph) {

            if (graph.Source<IVisual, IVisualEdge, IThing, ILink> () != null) {
                return new VisualThingMarkerFacade (graph);
            }

            return null;
        }
    }
}