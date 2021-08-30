/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2014 Lytico
 *
 * http://www.limada.org
 * 
 */


using Limaki.Graphs;

namespace Limaki.Tests.Graph.Model {

    public class WordGameGraphFactory<TItem, TEdge> : SampleGraphFactory<TItem, TEdge>
        where TEdge : IEdge<TItem>, TItem {

        public override string Name {
            get { return "Word Game"; }
        }

        public override void Populate(IGraph<TItem, TEdge> graph, int start) {
            var node = default (TItem);
            var edge = default (TEdge);


            node = CreateItem<string>("Tags");
            graph.Add(node);
            Nodes[1] = node;


            node = CreateItem<string>("Word");
            graph.Add(node);
            Nodes[2] = node;

            edge = CreateEdge(Nodes[1], Nodes[2]);
            graph.Add(edge);
            Edges[1] = edge;


            node = CreateItem<string>("Game");
            graph.Add(node);
            Nodes[3] = node;

            edge = CreateEdge(Nodes[2], Nodes[3]);
            graph.Add(edge);
            Edges[2] = edge;

            node = CreateItem<string>("Something");
            graph.Add(node);
            Nodes[4] = node;

            edge = CreateEdge(Edges[2], Nodes[4]);
            graph.Add(edge);
            Edges[3] = edge;

            edge = CreateEdge(Nodes[1], Edges[2]);
            graph.Add(edge);
            Edges[4] = edge;

        }

           
    }
}