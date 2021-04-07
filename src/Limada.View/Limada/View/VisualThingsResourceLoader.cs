using Limada.Model;
using Limada.View.ContentViewers;
using Limada.View.Vidgets;
using Limada.View.VisualThings;
using Limaki.Common.IOC;
using Limaki.Graphs;
using Limaki.View;
using Limaki.View.ContentViewers;
using Limaki.View.GraphScene;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;

namespace Limada.View {
    
    /// <summary>
    /// applies VisualThings
    /// </summary>
    public class VisualThingsResourceLoader : IContextResourceLoader {

        public virtual void ApplyResources (IApplicationContext context) {

            context.Factory.Add<GraphItemTransformer<IVisual, IThing, IVisualEdge, ILink>, VisualThingTransformer> ();
            context.Factory.Add<GraphItemTransformer<IThing, IVisual, ILink, IVisualEdge>> (t => new VisualThingTransformer ().Reverted ());

            context.Factory.Add<IVisualContentInteractor, VisualThingsContentInteractor> ();
            context.Factory.Add<IVisualContentInteractor<IThing>, VisualThingsContentInteractor> ();
			context.Factory.Add<ISceneInteractor<IVisual,IThing,IVisualEdge,ILink>, VisualThingsSceneInteractor> ();
            context.Factory.Add<ISceneInteractor<IVisual, IVisualEdge>, VisualThingsSceneInteractor> ();
            context.Factory.Add<IVisualSceneStoreInteractor, VisualSceneStoreInteractor> ();
            context.Factory.Add<IVisualGraphSceneSearch, VisualThingSearch> ();
            context.Factory.Add<IContentViewManager, ContentViewManager> ();

            context.Factory.Add<IMarkerInteractor<IVisual, IVisualEdge>> (p => {
                var graph = p[0] as IGraph<IVisual, IVisualEdge>;
                if (graph == null) {
                    var scene = p[0] as IGraphScene<IVisual, IVisualEdge>;
                    if (scene != null)
                        graph = scene.Graph;
                }

                if (graph.ThingGraph () != null)
                    return new VisualThingMarkerInteractor (graph);
                else
                    return null;
            });

        }
    }
}