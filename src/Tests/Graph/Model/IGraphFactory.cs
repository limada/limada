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

using Limaki.Graphs;

namespace Limaki.Tests.Graph.Model {
    public interface IGraphFactory<TItem, TEdge> where TEdge : IEdge<TItem> {
        IGraph<TItem, TEdge> Graph { get;set;}
        int Count { get; set; }
        string Name { get; }
        void Populate();
        bool SeperateLattice { get;set;}
        bool AddDensity { get;set;}
    }
}