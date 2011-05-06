using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Drawing;
using Limaki.Graphs.Extensions;
using Limaki.Model.Streams;
using Limaki.View;
using System.Reflection;
using System;
using Limaki.Widgets;
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


            GraphMapping.ChainGraphMapping<GraphItemWidgetMapping>(context);
            GraphMapping.ChainGraphMapping<WidgetThingGraphMapping>(context);

            var markerProcessor =
                context.Pool.TryGetCreate<MarkerContextProcessor>();
            markerProcessor.CreateMarkerFacade = this.MarkerFacade;


            var presenterLoader = new Limaki.Presenter.Display.PresenterRecourceLoader();
            presenterLoader.ApplyResources (context);

            var widgetLoader = new Limaki.Presenter.Widgets.WidgetRecourceLoader();
            widgetLoader.ApplyResources (context);

            context.Factory.Add<ISheetManager, SheetManager>();

        }

        public virtual IMarkerFacade<IWidget, IEdgeWidget> MarkerFacade(IGraph<IWidget, IEdgeWidget> graph) {

            if (GraphPairExtension<IWidget, IEdgeWidget>
                    .Source<IThing, ILink>(graph) != null) {

                return new WidgetThingMarkerFacade(graph);
            }

            return null;
        }
    }
}