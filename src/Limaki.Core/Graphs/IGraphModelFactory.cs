namespace Limaki.Graphs {
    public interface IGraphModelFactory<TItem, TEdge> {
        //TItem CreateItem(object data);
        //TEdge CreateEdge(object data);
        TEdge CreateEdge(TItem root, TItem leaf, object data);
        TItem CreateItem<T>(T data);
        TEdge CreateEdge<T>(T data);
    }
}