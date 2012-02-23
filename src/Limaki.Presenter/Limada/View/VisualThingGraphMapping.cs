/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using Limada.Model;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Visuals;

namespace Limaki.Limada.View {
    /// <summary>
    /// encapsulates operations on 
    /// where the type of TItemTwo and TEdgeTwo are unknown
    /// </summary>
    public class VisualThingGraphMapping:GraphMapping {
        public VisualThingGraphMapping() :base(){}

        /// <summary>
        /// looks if source is
        /// - VisualThingGraphPair
        /// if so, 
        /// creates a new GraphPair according to source
        /// source.One = new VisualGraph
        /// target.Two = source.Two (ThingGraph or IGraphPair<IGraphItem, IGraphEdge> as
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override IGraph<TItem, TEdge> CloneGraphPair<TItem, TEdge>(IGraph<TItem, TEdge> source) {
            
            IGraph<TItem, TEdge> targetGraph = null;

            if (source is VisualThingGraph) {
                targetGraph = new VisualThingGraph(
                                  new VisualGraph(),
                                  ((VisualThingGraph)source).Two as IThingGraph)
                              as IGraph<TItem, TEdge>;

            } else if (Next!=null){
                targetGraph = Next.CloneGraphPair<TItem, TEdge> (source);
            }

            return targetGraph;
        }

        

        public override TItem LookUp<TItem,TEdge>(
            IGraphPair<TItem, TItem, TEdge, TEdge> sourceGraph,
            IGraphPair<TItem, TItem, TEdge, TEdge> targetGraph,
            TItem sourceitem) {

            TItem item = default(TItem);
            if (sourceGraph == null || targetGraph == null || sourceitem == null)
                return item;

            if (sourceGraph.Source<TItem, TEdge, IThing, ILink>() != null) {
                return sourceGraph.LookUp<TItem, TEdge,IThing, ILink>(targetGraph, sourceitem);
            } else if (Next != null){
                return Next.LookUp<TItem, TEdge> (sourceGraph, targetGraph, sourceitem);
            }
            return item;
        }

    
    }
}