
using Limaki.Graphs;
using Limaki.Model;

namespace Limaki.Tests.Graph.Model {

    public class SimpleGraphFactory<TItem, TEdge> : SampleGraphFactory<TItem, TEdge> 
        where TEdge : IEdge<TItem> , TItem {
        public override void Populate(IGraph<TItem, TEdge> Graph, int start) {
            TItem item = CreateItem<int>((start++));
            Graph.Add(item);
            Nodes[1] = item;

            item = CreateItem<int>((start++));
            Graph.Add(item);
            Nodes[2] = item;

            Edges[1] = CreateEdge(Nodes[1], Nodes[2]);
            Graph.Add(Edges[1]);
        }

        public override string Name {
            get { return "SimpleGraph"; }
        }
    }
}