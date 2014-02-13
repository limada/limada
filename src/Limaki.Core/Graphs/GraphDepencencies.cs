using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Graphs;

namespace Limaki.Graphs {

    public class GraphDepencencies<TItem, TEdge> where TEdge : TItem, IEdge<TItem> {

        public Action<GraphCursor<TItem, TEdge>, Action<TItem>, GraphChangeType> Visitor { get; set; }

        public virtual void DependentItems (
            GraphCursor<TItem, TEdge> graphCursor, 
            Action<TItem> visit, 
            GraphChangeType changeType) {

            if (Visitor != null)
                Visitor(graphCursor, visit, changeType);
        }
    }
}
