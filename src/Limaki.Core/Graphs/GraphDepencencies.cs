using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Common;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;

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

    public class GraphDepencencyExtension {

        public static void DependencyVisitor<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> (GraphCursor<TSinkItem, TSinkEdge> sink, Action<TSinkItem> visit, GraphEventType eventType)
            where TSinkEdge : IEdge<TSinkItem>, TSinkItem
            where TSourceEdge : IEdge<TSourceItem>, TSourceItem {


            var graphPair = sink.Graph.Source<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> ();
            if (graphPair == null)
                return;

            var sourceGraph = graphPair.Source;
            var sourceItem = sink.Graph.SourceItemOf<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> (sink.Cursor);
            if (sourceGraph == null || sourceItem == null)
                return;

            Registry.Pooled<GraphDepencencies<TSourceItem, TSourceEdge>> ()
                .VisitItems (
                    GraphCursor.Create (sourceGraph, sourceItem),
                    source => {
                        if (graphPair.Source2Sink.ContainsKey (source))
                            visit (graphPair.Get (source));
                    },
                    eventType);

        }

    }
}
