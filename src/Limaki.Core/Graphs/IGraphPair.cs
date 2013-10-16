/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */


using System.Collections.Generic;

namespace Limaki.Graphs {
    /// <summary>
    /// A Graph that transforms domain-data #TSourceItem,TSourceEdge# 
    /// into view-data #TSinkItem,TSinkEdge# (similar to Fowler's Transform View).
    /// A GraphPair connects to Graphs of different types
    /// every operation concerns both graphs
    /// eg. Add(TSinkItem) result in
    /// Sink.Add(TSinkItem) and Source.Add(TSourceItem)
    /// </summary>
    /// <typeparam name="TSinkItem"></typeparam>
    /// <typeparam name="TSourceItem"></typeparam>
    /// <typeparam name="TSinkEdge"></typeparam>
    /// <typeparam name="TSourceEdge"></typeparam>
    public interface IGraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> :
        IGraph<TSinkItem, TSinkEdge>, IFactoryListener<TSinkItem>, ISinkGraph<TSinkItem, TSinkEdge>
        where TSinkEdge : IEdge<TSinkItem>, TSinkItem
        where TSourceEdge : IEdge<TSourceItem>, TSourceItem 
    {

        //IGraph<TItemOne, TEdgeOne> Sink { get;set;}
        IGraph<TSourceItem, TSourceEdge> Source { get;set;}
        IDictionary<TSinkItem, TSourceItem> Sink2Source { get;set;}
        IDictionary<TSourceItem, TSinkItem> Source2Sink { get;set;}
        GraphMapper<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> Mapper { get;set;}

        TSourceItem Get ( TSinkItem sink );
        TSinkItem Get ( TSourceItem source );

        
    }

    /// <summary>
    /// this is to get the SinkGraph of a Graphpair without
    /// knowing the types of the SourceGraph
    /// </summary>
    /// <typeparam name="TSinkItem"></typeparam>
    /// <typeparam name="TSinkEdge"></typeparam>
    public interface ISinkGraph<TSinkItem,TSinkEdge> :IGraph<TSinkItem, TSinkEdge>
    where TSinkEdge : IEdge<TSinkItem>, TSinkItem {
        IGraph<TSinkItem, TSinkEdge> Sink { get; }
        IEnumerable<TSinkEdge> ComplementEdges(TSinkItem item, IGraph<TSinkItem, TSinkEdge> graph);
    }
}