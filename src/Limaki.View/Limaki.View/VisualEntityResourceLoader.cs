using Limaki.Common.IOC;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model;
using Limaki.Visuals;

namespace Limaki.View {
    /// <summary>
    /// applies VisualEntity
    /// </summary>
    public class VisualEntityResourceLoader : IContextResourceLoader {

        public virtual void ApplyResources (IApplicationContext context) {

            GraphMapping.ChainGraphMapping<GraphVisualEntityMapping> (context);

            context.Factory.Add<GraphItemTransformer<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>, VisualGraphEntityTransformer> ();
            context.Factory.Add<GraphItemTransformer<IGraphEntity, IVisual, IGraphEdge, IVisualEdge>> (() => new VisualGraphEntityTransformer ().Reverted ());

        }

    }
}