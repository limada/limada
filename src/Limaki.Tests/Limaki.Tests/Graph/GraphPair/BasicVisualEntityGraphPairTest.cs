using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Basic;
using Limaki.Tests.View.Visuals;
using Limaki.View.Visuals;

namespace Limaki.Tests.Graph.GraphPair {

    public class BasicVisualEntityGraphPairTest : BasicGraphPairTest<IVisual, IGraphEntity, IVisualEdge, IGraphEdge> {
        public override IGraph<IVisual, IVisualEdge> Graph {
            get {
                if (!(base.Graph is IGraphPair<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>)) {
                    var one = new Graph<IVisual, IVisualEdge>();
                    var two = new Graph<IGraphEntity, IGraphEdge>();

                    base.Graph = new GraphPair<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>(
                        one, two, new VisualGraphEntityTransformer ());
                }
                return base.Graph;
            }
            set { base.Graph = value; }
        }
        public override BasicGraphTestDataFactory<IVisual, IVisualEdge> GetFactory() {
            return new VisualGraphTestDataFactory();
        }
    }
}