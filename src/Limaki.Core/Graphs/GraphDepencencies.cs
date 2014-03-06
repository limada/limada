using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Graphs;

namespace Limaki.Graphs {

    public class GraphDepencencies<TItem, TEdge> where TEdge : TItem, IEdge<TItem> {

        public Action<GraphCursor<TItem, TEdge>, Action<TItem>, GraphEventType> Visitor { get; set; }

        public virtual void VisitItems (
            GraphCursor<TItem, TEdge> graphCursor, 
            Action<TItem> visit, 
            GraphEventType eventType) {

            if (Visitor != null)
                Visitor(graphCursor, visit, eventType);
        }
    }
}
