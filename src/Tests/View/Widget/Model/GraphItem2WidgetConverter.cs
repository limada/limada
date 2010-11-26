using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using Limaki.Widgets;

namespace Limaki.Tests.Widget {
    public class GraphItem2WidgetConverter : GraphConverter<IGraphItem, IWidget, IGraphEdge, IEdgeWidget> {
        public override void InitFactoryMethods() {
            this.CreateItemTwo = delegate(IGraphItem a) {
                                     return new Widget<string>(a.ToString());
                                 };
            this.CreateEdgeTwo = delegate(IGraphEdge a) {
                                     return new EdgeWidget<string>(string.Empty);
                                 };
            CreateItemOne = delegate(IWidget b) {
                                return new GraphItem<string>(b.ToString());
                            };
            CreateEdgeOne = delegate(IEdgeWidget b) {
                                return new GraphEdge();
                            };
            RegisterOneTwo = delegate(IGraphEdge a, IEdgeWidget b) { };
            RegisterTwoOne = delegate(IEdgeWidget b, IGraphEdge a) { };
        }
    }
}