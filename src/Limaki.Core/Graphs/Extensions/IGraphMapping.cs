/*
 * Limaki 
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


namespace Limaki.Graphs.Extensions {
    public interface IGraphMapping {

        IGraphMapping Next { get;set;}
        /// <summary>
        /// looks if source is
        /// - WidgetThingGraphPair
        /// - IGraphPair<IWidget, IGraphItem, IEdgeWidget, IGraphEdge>
        /// if so, 
        /// creates a new GraphPair according to source
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IGraph<TItem, TEdge> CloneGraphPair<TItem, TEdge> ( IGraph<TItem, TEdge> source )
            where TEdge : IEdge<TItem>;

        TItem LookUp<TItem, TEdge> (
            IGraphPair<TItem, TItem, TEdge, TEdge> sourceGraph,
            IGraphPair<TItem, TItem, TEdge, TEdge> targetGraph,
            TItem sourceitem ) where TEdge : IEdge<TItem>, TItem;
    }
}