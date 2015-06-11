
using Limaki.Graphs;

namespace Limaki.Tests.Graph.Model {

    public class SimpleGraphFactory<TItem, TEdge> : SampleGraphFactory<TItem, TEdge> 
        where TEdge : IEdge<TItem> , TItem {

        public override void Populate(IGraph<TItem, TEdge> graph, int start) {

            var item = CreateItem<int>((start++));
            graph.Add(item);
            Nodes[1] = item;

            item = CreateItem<int>((start++));
            graph.Add(item);
            Nodes[2] = item;

            Edges[1] = CreateEdge(Nodes[1], Nodes[2]);
            graph.Add(Edges[1]);
        }

        public override string Name {
            get { return "SimpleGraph"; }
        }
    }
}