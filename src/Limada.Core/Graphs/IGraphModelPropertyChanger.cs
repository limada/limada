namespace Limaki.Graphs {

    public interface IGraphModelPropertyChanger<TItem, TEdge>
        where TEdge : IEdge<TItem>, TItem {
        void SetProperty (TItem item, object data);
        object GetProperty (TItem item);
    }

}