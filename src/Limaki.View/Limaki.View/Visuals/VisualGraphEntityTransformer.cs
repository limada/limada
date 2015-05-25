using Limaki.Graphs;
using Limaki.Model;
using Limaki.Common;

namespace Limaki.View.Visuals {

    public class VisualGraphEntityTransformer : GraphItemTransformer<IVisual, IGraphEntity, IVisualEdge, IGraphEdge> {

        public override IVisual CreateSinkItem (IGraph<IGraphEntity, IGraphEdge> source, IGraph<IVisual, IVisualEdge> sink, IGraphEntity item) {
            return Registry.Pooled<IVisualFactory> ()
                .CreateItem (item.ToString ());
        }

        public override IVisualEdge CreateSinkEdge (IGraph<IGraphEntity, IGraphEdge> source, IGraph<IVisual, IVisualEdge> sink, IGraphEdge item) {
            return Registry.Pooled<IVisualFactory> ()
                    .CreateEdge (item.ToString ());
        }

        public override IGraphEntity CreateSourceItem (IGraph<IVisual, IVisualEdge> sink, IGraph<IGraphEntity, IGraphEdge> source, IVisual item) {
            return new GraphEntity<string> (item.ToString ());
        }

        public override IGraphEdge CreateSourceEdge (IGraph<IVisual, IVisualEdge> sink, IGraph<IGraphEntity, IGraphEdge> source, IVisualEdge item) {
            return new GraphEdge ();
        }

        public override void ChangeData (IGraph<IVisual, IVisualEdge> sink, IVisual item, object data) {
            var graph = sink as IGraphPair<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>;
            if (graph != null) {
                var entity = graph.Get (item);
                entity.Data = data;
                item.Data = data;
            }
        }

        public override void ChangeData (IGraph<IGraphEntity, IGraphEdge> source, IGraphEntity item, object data) {
            throw new System.NotImplementedException ();
        }

        public override void UpdateSinkItem (IGraph<IGraphEntity, IGraphEdge> source, IGraph<IVisual, IVisualEdge> sink, IGraphEntity sourceItem, IVisual sinkItem) {
            sinkItem.Data = sourceItem.Data;
        }
    }

}