using Limada.Model;
using Limada.View;
using Limada.VisualThings;
using Limaki.Common.IOC;
using Limaki.Drawing;
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

            context.Factory.Add<IMarkerFacade<IVisual, IVisualEdge>> (p => {
                var graph = p[0] as IGraph<IVisual, IVisualEdge>;
                if (graph == null) {
                    var scene = p[0] as IGraphScene<IVisual, IVisualEdge>;
                    if (scene != null)
                        graph = scene.Graph;
                }

                return new VisualThingMarkerFacade (graph);
            });

        }
    }
}