
using Limaki.Graphs;
using Limaki.Model;

namespace Limaki.Tests.Graph.Model {
    public class SimpleGraphFactory : GraphFactoryBase {
        public override void Populate(IGraph<IGraphItem, IGraphEdge> Graph, int start) {
            IGraphItem item = new GraphItem<int>((start++));
            Graph.Add(item);
            Node[1] = item;

            item = new GraphItem<int>((start++));
            Graph.Add(item);
            Node[2] = item;

            Edge[1] = new GraphEdge(Node[1], Node[2]);
            Graph.Add(Edge[1]);
        }

        public override string Name {
            get { return "SimpleGraph"; }
        }
    }
}