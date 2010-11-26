using Limaki.Graphs;
using Limaki.Model;
using Limaki.Widgets;
using Limaki.Common;

namespace Limaki.View {
    public class GraphItem2WidgetAdapter : GraphModelAdapter<IGraphItem, IWidget, IGraphEdge, IEdgeWidget> {

        public override IGraphItem CreateItemOne(IGraph<IWidget, IEdgeWidget> sender,
                                                 IGraph<IGraphItem, IGraphEdge> target, IWidget item) {
            return new GraphItem<string>(item.ToString());
        }

        public override IGraphEdge CreateEdgeOne(IGraph<IWidget, IEdgeWidget> sender,
                                                 IGraph<IGraphItem, IGraphEdge> target, IEdgeWidget item) {
            return new GraphEdge();
        }

        public override IWidget CreateItemTwo(IGraph<IGraphItem, IGraphEdge> sender,
                                              IGraph<IWidget, IEdgeWidget> target,IGraphItem item) {
            return Registry.Pool.TryGetCreate<IWidgetFactory>()
                            .CreateWidget(item.ToString());
        }

        public override IEdgeWidget CreateEdgeTwo(IGraph<IGraphItem, IGraphEdge> sender,
                                                  IGraph<IWidget, IEdgeWidget> target, IGraphEdge item) {
            return Registry.Pool.TryGetCreate<IWidgetFactory>()
                            .CreateEdgeWidget(item.ToString());
        }
        public override void ChangeData(IGraph<IGraphItem, IGraphEdge> sender, IGraphItem item, object data) {
            throw new System.Exception("The method or operation is not implemented.");
        }
        public override void ChangeData(IGraph<IWidget, IEdgeWidget> sender, IWidget item, object data) {
            var graph = sender as IGraphPair<IWidget, IGraphItem, IEdgeWidget, IGraphEdge>;
            if (graph != null) {
                IGraphItem thing = graph.Get(item);
                thing.Data = data;
                item.Data = data;
            }
        }
    }
}