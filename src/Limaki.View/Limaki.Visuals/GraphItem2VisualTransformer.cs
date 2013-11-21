using Limaki.Graphs;
using Limaki.Model;
using Limaki.Common;

namespace Limaki.Visuals {
    public class GraphItem2VisualTransformer : GraphItemTransformer<IGraphEntity, IVisual, IGraphEdge, IVisualEdge> {

        public override IGraphEntity CreateSinkItem(IGraph<IVisual, IVisualEdge> source,
                                                 IGraph<IGraphEntity, IGraphEdge> sink, IVisual item) {
            return new GraphEntity<string>(item.ToString());
        }

        public override IGraphEdge CreateSinkEdge(IGraph<IVisual, IVisualEdge> source,
                                                 IGraph<IGraphEntity, IGraphEdge> sink, IVisualEdge item) {
            return new GraphEdge();
        }

        public override IVisual CreateSourceItem(IGraph<IGraphEntity, IGraphEdge> sink,
                                              IGraph<IVisual, IVisualEdge> source,IGraphEntity item) {
            return Registry.Pool.TryGetCreate<IVisualFactory>()
                .CreateItem(item.ToString());
        }

        public override IVisualEdge CreateSourceEdge(IGraph<IGraphEntity, IGraphEdge> sink,
                                                  IGraph<IVisual, IVisualEdge> source, IGraphEdge item) {
            return Registry.Pool.TryGetCreate<IVisualFactory>()
                .CreateEdge(item.ToString());
        }
        public override void ChangeData(IGraph<IGraphEntity, IGraphEdge> sink, IGraphEntity item, object data) {
            throw new System.Exception("The method or operation is not implemented.");
        }
        public override void ChangeData(IGraph<IVisual, IVisualEdge> source, IVisual item, object data) {
            var graph = source as IGraphPair<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>;
            if (graph != null) {
                var thing = graph.Get(item);
                thing.Data = data;
                item.Data = data;
            }
        }
    }
}