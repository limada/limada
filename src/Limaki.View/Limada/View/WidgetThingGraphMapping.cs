/*
 * Limada 
 * Version 0.08
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using Limada.Model;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Widgets;

namespace Limada.View {
    /// <summary>
    /// encapsulates operations on 
    /// where the type of TItemTwo and TEdgeTwo are unknown
    /// </summary>
    public class WidgetThingGraphMapping:GraphMapping {
        public WidgetThingGraphMapping() :base(){}

        /// <summary>
        /// looks if source is
        /// - WidgetThingGraphPair
        /// if so, 
        /// creates a new GraphPair according to source
        /// source.One = new WidgetGraph
        /// target.Two = source.Two (ThingGraph or IGraphPair<IGraphItem, IGraphEdge> as
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override IGraph<TItem, TEdge> CloneGraphPair<TItem, TEdge>(IGraph<TItem, TEdge> source) {
            
            IGraph<TItem, TEdge> targetGraph = null;

            if (source is WidgetThingGraph) {
                targetGraph = new WidgetThingGraph(
                                  new WidgetGraph(),
                                  ((WidgetThingGraph)source).Two as IThingGraph)
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

            var facade = new GraphPairFacade<TItem, TEdge>();
            
            if (facade.Source<IThing, ILink>(sourceGraph) != null) {
                return facade.LookUp<IThing, ILink> (
                    sourceGraph, targetGraph, sourceitem);
            } else if (Next != null){
                return Next.LookUp<TItem, TEdge> (sourceGraph, targetGraph, sourceitem);
            }
            return item;
        }

    
    }
}