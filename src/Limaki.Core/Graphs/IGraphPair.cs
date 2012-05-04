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
    /// A Graph that transforms domain-data #TItemTwo,TEdgeTwo# 
    /// into view-data #TItemOne,TEdgeOne# (similar to Fowler's Transform View).
    /// A GraphPair connects to Graphs of different types
    /// every operation concerns both graphs
    /// eg. Add(TItemOne) result in
    /// One.Add(TItemOne) and Two.Add(TItemTwo)
    /// </summary>
    /// <typeparam name="TItemOne"></typeparam>
    /// <typeparam name="TItemTwo"></typeparam>
    /// <typeparam name="TEdgeOne"></typeparam>
    /// <typeparam name="TEdgeTwo"></typeparam>
    public interface IGraphPair<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> :
        IGraph<TItemOne, TEdgeOne>, IFactoryListener<TItemOne>, IBaseGraphPair<TItemOne, TEdgeOne>
        where TEdgeOne : IEdge<TItemOne>, TItemOne
        where TEdgeTwo : IEdge<TItemTwo>, TItemTwo 
    {

        //IGraph<TItemOne, TEdgeOne> One { get;set;}
        IGraph<TItemTwo, TEdgeTwo> Two { get;set;}
        IDictionary<TItemOne, TItemTwo> One2Two { get;set;}
        IDictionary<TItemTwo, TItemOne> Two2One { get;set;}
        GraphMapper<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> Mapper { get;set;}

        TItemTwo Get ( TItemOne a );
        TItemOne Get ( TItemTwo a );

        
    }

    /// <summary>
    /// this is to get the One-Graph of a Graphpair without
    /// knowing the types of the Two-Graph
    /// </summary>
    /// <typeparam name="TItemOne"></typeparam>
    /// <typeparam name="TEdgeOne"></typeparam>
    public interface IBaseGraphPair<TItemOne,TEdgeOne> :IGraph<TItemOne, TEdgeOne>
    where TEdgeOne : IEdge<TItemOne>, TItemOne {
        IGraph<TItemOne, TEdgeOne> One { get; }
        IEnumerable<TEdgeOne> ComplementEdges(TItemOne item, IGraph<TItemOne, TEdgeOne> graph);
    }
}