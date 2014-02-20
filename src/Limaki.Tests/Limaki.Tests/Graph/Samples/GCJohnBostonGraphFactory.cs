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

namespace Limaki.Tests.Graph.Model {

    public class GCJohnBostonGraphFactory<TItem, TEdge> : SampleGraphFactory<TItem, TEdge>
        where TEdge : IEdge<TItem>, TItem {

        public override string Name {
            get { return "GC John going to Boston"; }
        }

        public override void Populate(IGraph<TItem, TEdge> Graph,int start) {

            SetNode (1, "Person");
            SetNode (2, "John");
            SetNode (3, "City");
            SetNode (4, "Boston");
            SetNode (5, "Go");
            SetNode (6, "Bus");

            SetEdge (1, Nodes[1], Nodes[2]);
            SetEdge (2, Nodes[3], Nodes[4]);
            SetEdge (3, Edges[1], Nodes[5]);
            SetEdge (4, Edges[3], Edges[2]);
            SetEdge (5, Edges[4], Nodes[6]);

        }

       
    }
}