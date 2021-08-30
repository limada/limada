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

using Limaki.Graphs;
using System.Collections.Generic;

namespace Limaki.Tests.Graph.Model {

    /// <summary>
    /// fills a <see cref="IGraph{TItemOne, TEdgeOne}"/>
    /// with sample-items
    /// the sample items are accessible in Nodes // Edges
    /// </summary>
    /// 
    public interface ISampleGraphFactory<TItem, TEdge> where TEdge : IEdge<TItem> {

        IGraph<TItem, TEdge> Graph { get;set;}
        int Count { get; set; }
        string Name { get; }
        void Populate();
        bool SeperateLattice { get;set;}
        bool AddDensity { get;set;}
        IList<TItem> Nodes { get; }
        IList<TEdge> Edges { get; }

		void Populate( IGraph<TItem,TEdge> graph );
	}
}