using Limaki.Common.IOC;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.View.Visuals;

namespace Limaki.Usecases {
    /// <summary>
    /// applies VisualEntity
    /// </summary>
    public class VisualEntityResourceLoader : IContextResourceLoader {

        public virtual void ApplyResources (IApplicationContext context) {
#if !__ANDROID__
            GraphMapping.ChainGraphMapping<GraphVisualEntityMapping> (context);
#endif
            context.Factory.Add<GraphItemTransformer<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>, VisualGraphEntityTransformer> ();
            context.Factory.Add<GraphItemTransformer<IGraphEntity, IVisual, IGraphEdge, IVisualEdge>> (() => new VisualGraphEntityTransformer ().Reverted ());

        }

    }
}