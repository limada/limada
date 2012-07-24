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
using Limaki.Drawing;
using Limaki.View.Visuals.Layout;
using Limaki.Common;
using Limaki.View.UI;

namespace Limaki.IOC {

    public class ViewContextRecourceLoader : IContextRecourceLoader {
        /// <summary>
        /// Attention! Before calling this ResourceLoader, all DrawingFactories
        /// have to be loaded! 
        /// </summary>
        /// <param name="context"></param>
        public virtual void ApplyResources (IApplicationContext context) {

            if (!context.Factory.Contains<IDrawingUtils> ())
                context.Factory.Add<IDrawingUtils, BlindDrawingUtils> ();

            if (!context.Factory.Contains<ISystemFonts> ())
                context.Factory.Add<ISystemFonts, BlindSystemFonts> ();

            if (!context.Factory.Contains<IUISystemInformation> ())
                context.Factory.Add<IUISystemInformation, BlindSystemInformation> ();

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
            context.Factory.Add<IGraphSceneLayout<IVisual, IVisualEdge>>(args =>
                    new VisualsSceneArrangerLayout<IVisual, IVisualEdge>(args[0] as Get<IGraphScene<IVisual, IVisualEdge>>, args[1] as IStyleSheet)
                );
        }

        public virtual IMarkerFacade<IVisual, IVisualEdge> MarkerFacade (IGraph<IVisual, IVisualEdge> graph) {

            if (graph.Source<IVisual, IVisualEdge, IThing, ILink> () != null) {
                return new VisualThingMarkerFacade (graph);
            }

            return null;
        }
    }
}