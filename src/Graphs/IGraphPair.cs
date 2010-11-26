/*
 * Limaki 
 * Version 0.07
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


using System.Collections.Generic;

namespace Limaki.Graphs {
    /// <summary>
    /// a GraphPair connects to Graphs of different types
    /// every operation should concern both graphs
    /// eg. Add(TItemOne) should result in
    /// One.Add(TItemOne) and Two.Add(TItemTwo)
    /// </summary>
    /// <typeparam name="TItemOne"></typeparam>
    /// <typeparam name="TItemTwo"></typeparam>
    /// <typeparam name="TEdgeOne"></typeparam>
    /// <typeparam name="TEdgeTwo"></typeparam>
    public interface IGraphPair<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> :
        IGraph<TItemOne, TEdgeOne>
        where TEdgeOne : IEdge<TItemOne>, TItemOne
        where TEdgeTwo : IEdge<TItemTwo>, TItemTwo {

        IGraph<TItemOne, TEdgeOne> One { get;set;}
        IGraph<TItemTwo, TEdgeTwo> Two { get;set;}
        IDictionary<TItemOne, TItemTwo> One2Two { get;set;}
        IDictionary<TItemTwo, TItemOne> Two2One { get;set;}
        GraphConverter<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> Converter { get;set;}
    }
}