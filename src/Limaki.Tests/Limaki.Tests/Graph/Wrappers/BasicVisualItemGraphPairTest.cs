using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Basic;
using Limaki.Tests.View.Visuals;
using Limaki.Tests.Visuals;
using Limaki.Visuals;

namespace Limaki.Tests.Graph.Wrappers {
    public class BasicVisualItemGraphPairTest : BasicGraphPairTest<IVisual, IGraphEntity, IVisualEdge, IGraphEdge> {
        public override IGraph<IVisual, IVisualEdge> Graph {
            get {
                if (!(base.Graph is IGraphPair<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>)) {
                    var one = new Graph<IVisual, IVisualEdge>();
                    var two = new Graph<IGraphEntity, IGraphEdge>();

                    base.Graph = new LiveGraphPair<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>(
                        one, two, new GraphItem2VisualAdapter().ReverseAdapter());
                }
                return base.Graph;
            }
            set { base.Graph = value; }
        }
        public override BasicTestDataFactory<IVisual, IVisualEdge> GetFactory() {
            return new VisualDataFactory();
        }
    }
}