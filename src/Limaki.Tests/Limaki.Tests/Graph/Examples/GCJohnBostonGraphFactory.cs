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
using Limaki.Model;

namespace Limaki.Tests.Graph.Model {

    public class GCJohnBostonGraphFactory<TItem, TEdge> : SampleGraphFactory<TItem, TEdge>
        where TEdge : IEdge<TItem>, TItem {

        public override string Name {
            get { return "GC John going to Boston"; }
        }

        public override void Populate(IGraph<TItem, TEdge> Graph,int start) {
            TItem node = default (TItem);
            TEdge edge = default (TEdge);


            node = CreateItem<string>("Person");
            Graph.Add(node);
            Nodes[1] = node;


            node = CreateItem<string>("John");
            Graph.Add(node);
            Nodes[2] = node;

            edge = CreateEdge(Nodes[1], Nodes[2]);
            Graph.Add(edge);
            Edges[1] = edge;


            node = CreateItem<string>("City");
            Graph.Add(node);
            Nodes[3] = node;

            node = CreateItem<string>("Boston");
            Graph.Add(node);
            Nodes[4] = node;

            edge = CreateEdge(Nodes[3], Nodes[4]);
            Graph.Add(edge);
            Edges[2] = edge;

            node = CreateItem<string>("Go");
            Graph.Add(node);
            Nodes[5] = node;


            edge = CreateEdge(Edges[1], Nodes[5]);
            Graph.Add(edge);
            Edges[3] = edge;



            edge = CreateEdge(Edges[3],Edges[2]);
            Graph.Add(edge);
            Edges[4] = edge;

            node = CreateItem<string>("Bus");
            Graph.Add(node);
            Nodes[6] = node;

            edge = CreateEdge(Edges[4], Nodes[6]);
            Graph.Add(edge);
            Edges[5] = edge;

            

        }

       
    }
}