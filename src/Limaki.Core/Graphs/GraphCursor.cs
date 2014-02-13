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

using System.Collections.Generic;

namespace Limaki.Graphs {


    public class GraphCursor<TItem, TEdge> where TEdge : IEdge<TItem> {

        

        public GraphCursor (IGraph<TItem, TEdge> graph, TItem cursor):this(graph) {
            this.Cursor = cursor;
        }

        public GraphCursor (IGraph<TItem, TEdge> graph) {
            this.Graph = graph;
        }

        public TItem Cursor { get; set; }
        public IGraph<TItem, TEdge> Graph { get; set; }
    }

    public class GraphSelection<TItem, TEdge> where TEdge : IEdge<TItem> {

        public GraphSelection (IGraph<TItem, TEdge> graph, IEnumerable<TItem> selection): this(graph) {
            this.Selection = selection;
        }

        public GraphSelection (IGraph<TItem, TEdge> graph) {
            this.Graph = graph;
        }

        public IEnumerable<TItem>  Selection { get; set; }
        public IGraph<TItem, TEdge> Graph { get; set; }
    }
}