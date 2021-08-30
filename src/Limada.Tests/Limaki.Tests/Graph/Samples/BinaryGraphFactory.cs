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

    public class BinaryGraphFactory<TItem, TEdge> : SampleGraphFactory<TItem, TEdge>
        where TEdge : IEdge<TItem>, TItem {

        public override string Name {
            get { return "Binary Graph"; }
        }

        public override void Populate(IGraph<TItem, TEdge> graph, int start) {

            base.Populate(graph,start);

            var edge = default (TEdge); ;

            #region Binarytree Edges

            edge = CreateEdge(Nodes[1], Nodes[2]);
            graph.Add(edge);
            Edges[1] = edge;

            edge = CreateEdge(Nodes[4], Nodes[3]);
            graph.Add(edge);
            Edges[2] = edge;

            edge = CreateEdge(Nodes[1], Edges[2]);
            graph.Add(edge);
            Edges[3] = edge;

            edge = CreateEdge(Nodes[5], Nodes[8]);
            graph.Add(edge);
            Edges[4] = edge;


            edge = CreateEdge(Nodes[5], Nodes[6]);
            graph.Add(edge);
            Edges[5] = edge;

            edge = CreateEdge(Nodes[5], Nodes[7]);
            graph.Add(edge);
            Edges[6] = edge;

            edge = CreateEdge(Nodes[8], Nodes[9]);
            graph.Add(edge);
            Edges[7] = edge;

            if (!SeperateLattice) {
                edge = CreateEdge (Edges[2], Edges[4]);
                graph.Add (edge);
                Edges[8] = edge;
            }

            #endregion
        }
    }
}