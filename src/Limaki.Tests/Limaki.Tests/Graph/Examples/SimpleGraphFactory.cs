
using Limaki.Graphs;
using Limaki.Model;

namespace Limaki.Tests.Graph.Model {

    public class SimpleGraphFactory : EntityGraphFactory {
        public override void Populate(IGraph<IGraphEntity, IGraphEdge> Graph, int start) {
            IGraphEntity item = new GraphEntity<int>((start++));
            Graph.Add(item);
            Nodes[1] = item;

            item = new GraphEntity<int>((start++));
            Graph.Add(item);
            Nodes[2] = item;

            Edges[1] = new GraphEdge(Nodes[1], Nodes[2]);
            Graph.Add(Edges[1]);
        }

        public override string Name {
            get { return "SimpleGraph"; }
        }
    }
}