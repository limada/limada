namespace Limaki.Graphs {
    public class Walker {
        public static Walker<TItem, TEdge> Create<TItem, TEdge> (IGraph<TItem, TEdge> graph) where TEdge : IEdge<TItem>, TItem {
            return new Walker<TItem, TEdge>(graph);
        }
    }
}