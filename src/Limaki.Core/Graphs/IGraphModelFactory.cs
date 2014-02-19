using System.Collections.Generic;
using System;

namespace Limaki.Graphs {

    public interface IGraphModelFactory<TItem, TEdge>
       where TEdge : IEdge<TItem>, TItem {
        TItem CreateItem<T> (T data);
        TEdge CreateEdge<T> (T data);
        TEdge CreateEdge (TItem root, TItem leaf, object data);

        IGraph<TItem, TEdge> Graph ();
        TEdge CreateEdge (TItem root, TItem leaf);
        TEdge CreateEdge ();
       

    }
}