using Limaki.Common;
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

namespace Limaki.Context {
    public class ViewContextRecourceLoader : Common.ContextRecourceLoader {
        /// <summary>
        /// Attention! Before calling this ResourceLoader, all DrawingFactories
        /// have to be loaded! 
        /// </summary>
        /// <param name="context"></param>
        public override void ApplyResources(IApplicationContext context) {
            StyleSheets styleSheets = context.Pool.TryGetCreate<StyleSheets>();
            styleSheets.Init();


            context.Factory.Add<ISheetManager, SheetManager>();

            GraphMapping.ChainGraphMapping<GraphItemWidgetMapping>(context);
            GraphMapping.ChainGraphMapping<WidgetThingGraphMapping>(context);

            MarkerContextProcessor markerProcessor =
                context.Pool.TryGetCreate<MarkerContextProcessor>();
            markerProcessor.CreateMarkerFacade = this.MarkerFacade;
        }

        public virtual IMarkerFacade<IWidget, IEdgeWidget> MarkerFacade(IGraph<IWidget, IEdgeWidget> graph) {

            if (new GraphPairFacade<IWidget, IEdgeWidget>()
                    .Source<IThing, ILink>(graph) != null) {

                return new WidgetThingMarkerFacade(graph);
            }

            return null;
        }
    }
}