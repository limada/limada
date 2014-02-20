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

    public class BinaryTreeFactory<TItem, TEdge> : SampleGraphFactory<TItem, TEdge>
        where TEdge : IEdge<TItem>, TItem {

        public override string Name {
            get { return "Binary Tree"; }
        }

        public override void Populate(IGraph<TItem, TEdge> Graph,int start) {

            base.Populate(Graph,start);

            TEdge edge = default (TEdge);

            #region Binarytree Links

            edge = CreateEdge(Nodes[1], Nodes[2]);
            Graph.Add(edge);
            Edges[1] = edge;

            edge = CreateEdge(Nodes[1], Nodes[4]);
            Graph.Add(edge);
            Edges[2] = edge;

            edge = CreateEdge(Nodes[2], Nodes[3]);
            Graph.Add(edge);
            Edges[3] = edge;

            edge = CreateEdge(Nodes[4], Nodes[5]);
            Graph.Add(edge);
            Edges[4] = edge;


            edge = CreateEdge(Nodes[4], Nodes[8]);
            Graph.Add(edge);
            Edges[5] = edge;

            edge = CreateEdge(Nodes[5], Nodes[6]);
            Graph.Add(edge);
            Edges[6] = edge;

            edge = CreateEdge(Nodes[5], Nodes[7]);
            Graph.Add(edge);
            Edges[7] = edge;


            edge = CreateEdge(Nodes[8], Nodes[9]);
            Graph.Add(edge);
            Edges[8] = edge;

            #endregion
        }
    }
}