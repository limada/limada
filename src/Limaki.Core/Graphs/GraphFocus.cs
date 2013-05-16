/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

namespace Limaki.Graphs {
    public class GraphFocus<TItem, TEdge> where TEdge : IEdge<TItem> {
        public GraphFocus (IGraph<TItem, TEdge> graph, TItem focused) {
            this.Graph = graph;
            this.Focused = focused;
        }
        public TItem Focused { get; set; }
        public IGraph<TItem, TEdge> Graph { get; protected set; }
    }
}