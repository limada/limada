using System.Collections.Generic;
using System;

namespace Limaki.Graphs {
    public interface IGraphModelFactory<TItem, TEdge> {

        TItem CreateItem<T>(T data);
        TEdge CreateEdge<T>(T data);
        TEdge CreateEdge(TItem root, TItem leaf, object data);

    }
}