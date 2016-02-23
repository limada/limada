using System;
using Limaki.Graphs;
using Limaki.Model;

namespace Limaki.View.Visuals {
    [Obsolete]
    public class GraphVisualEntityMapping : GraphMapping {

        /// <summary>
        /// looks if source is
        /// - IGraphPair<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>
        /// if so, 
        /// creates a new GraphPair according to source
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override IGraph<TItem, TEdge> CloneGraphPair<TItem, TEdge>(
            IGraph<TItem, TEdge> source) {

            IGraph<TItem, TEdge> targetGraph = null;
            if (source is IGraphPair<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>) {
                targetGraph = new GraphPair<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>(
                                  new VisualGraph(),
                                  ((IGraphPair<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>)source).Source,
                                  new VisualGraphEntityTransformer ())
                              as IGraph<TItem, TEdge> ;
            } else  if (Next != null) {
                return Next.CloneGraphPair<TItem, TEdge> (source);
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

            if (sourceGraph.Source<TItem, TEdge, IGraphEntity, IGraphEdge>() != null) {
                return sourceGraph.LookUp<TItem, TEdge, IGraphEntity, IGraphEdge>(targetGraph, sourceitem);
            } else if (Next != null) {
                return Next.LookUp<TItem,TEdge> (sourceGraph,targetGraph,sourceitem);
            }

            return item;

        }
    }
}