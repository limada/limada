using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Drawing;
using Limaki.Graphs.Extensions;
using Limaki.Model.Streams;
using System.Reflection;
using System;
using Limaki.Visuals;
using Limada.Model;
using Limada.View;
using Limaki.Graphs;
using Limada.Presenter;

namespace Limaki.Context {
    public class ViewContextRecourceLoader : IContextRecourceLoader {
        /// <summary>
        /// Attention! Before calling this ResourceLoader, all DrawingFactories
        /// have to be loaded! 
        /// </summary>
        /// <param name="context"></param>
        public virtual void ApplyResources(IApplicationContext context) {
            StyleSheets styleSheets = context.Pool.TryGetCreate<StyleSheets>();
            styleSheets.Init();


            GraphMapping.ChainGraphMapping<GraphItemVisualMapping>(context);
            GraphMapping.ChainGraphMapping<VisualThingGraphMapping>(context);

            var markerProcessor =
                context.Pool.TryGetCreate<MarkerContextProcessor>();
            markerProcessor.CreateMarkerFacade = this.MarkerFacade;


            var presenterLoader = new Limaki.Presenter.Display.PresenterRecourceLoader();
            presenterLoader.ApplyResources (context);

            var visualsRecourceLoader = new Limaki.Presenter.Visuals.VisualsRecourceLoader();
            visualsRecourceLoader.ApplyResources (context);

            context.Factory.Add<ISheetManager, SheetManager>();

        }

        public virtual IMarkerFacade<IVisual, IVisualEdge> MarkerFacade(IGraph<IVisual, IVisualEdge> graph) {

            if (graph.Source<IVisual, IVisualEdge,IThing, ILink>() != null) {
                return new VisualThingMarkerFacade(graph);
            }

            return null;
        }
    }
}