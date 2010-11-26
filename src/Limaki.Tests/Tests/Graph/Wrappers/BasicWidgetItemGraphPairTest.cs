using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Basic;
using Limaki.Tests.View.Widget;
using Limaki.Tests.Widget;
using Limaki.View;
using Limaki.Widgets;

namespace Limaki.Tests.Graph.Wrappers {
    public class BasicWidgetItemGraphPairTest : BasicGraphPairTest<IWidget, IGraphItem, IEdgeWidget, IGraphEdge> {
        public override IGraph<IWidget, IEdgeWidget> Graph {
            get {
                if (!(base.Graph is IGraphPair<IWidget, IGraphItem, IEdgeWidget, IGraphEdge>)) {
                    IGraph<IWidget, IEdgeWidget> one = new Graph<IWidget, IEdgeWidget>();
                    IGraph<IGraphItem, IGraphEdge> two = new Graph<IGraphItem, IGraphEdge>();

                    base.Graph = new LiveGraphPair<IWidget, IGraphItem, IEdgeWidget, IGraphEdge>(
                        one, two, new GraphItem2WidgetAdapter().ReverseAdapter());
                }
                return base.Graph;
            }
            set { base.Graph = value; }
        }
        public override BasicTestDataFactory<IWidget, IEdgeWidget> GetFactory() {
            return new WidgetDataFactory();
        }
    }
}