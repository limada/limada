using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model;
using Limaki.Widgets;

namespace Limaki.View {
    public class GraphItemWidgetMapping : GraphMapping {

        /// <summary>
        /// looks if source is
        /// - IGraphPair<IWidget, IGraphItem, IEdgeWidget, IGraphEdge>
        /// if so, 
        /// creates a new GraphPair according to source
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override IGraph<TItem, TEdge> CloneGraphPair<TItem, TEdge>(
            IGraph<TItem, TEdge> source) {

            IGraph<TItem, TEdge> targetGraph = null;
            if (source is IGraphPair<IWidget, IGraphItem, IEdgeWidget, IGraphEdge>) {
                targetGraph = new LiveGraphPair<IWidget, IGraphItem, IEdgeWidget, IGraphEdge>(
                                  new WidgetGraph(),
                                  ((IGraphPair<IWidget, IGraphItem, IEdgeWidget, IGraphEdge>)source).Two,
                                  new GraphItem2WidgetAdapter().ReverseAdapter())
                              as IGraph<TItem, TEdge> ;
            } else  if (Next != null) {
                Next.CloneGraphPair<TItem, TEdge> (source);
            }

            return targetGraph;
        }

        public override TItem LookUp<TItem,TEdge>(
            IGraphPair<TItem, TItem, TEdge, TEdge> sourceGraph,
            IGraphPair<TItem, TItem, TEdge, TEdge> targetGraph,
            TItem sourceitem)  {

            TItem item = default(TItem);
            if (sourceGraph == null || targetGraph == null || sourceitem == null)
                return item;

            var facade = new GraphPairFacade<TItem, TEdge>();
            
            if (facade.Source<IGraphItem, IGraphEdge>(sourceGraph) != null) {
                return facade.LookUp<IGraphItem, IGraphEdge>(sourceGraph, targetGraph, sourceitem);
            } else if (Next != null) {
                return Next.LookUp<TItem,TEdge> (sourceGraph,targetGraph,sourceitem);
            }

            return item;

        }
    }
}