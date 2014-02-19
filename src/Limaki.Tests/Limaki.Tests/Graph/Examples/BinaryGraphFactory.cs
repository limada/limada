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

    public class BinaryGraphFactory : EntityGraphFactory {

        public override string Name {
            get { return "Binary Graph"; }
        }

        public override void Populate(IGraph<IGraphEntity, IGraphEdge> Graph, int start) {

            base.Populate(Graph,start);

            GraphEdge edge = null;

            #region Binarytree Edges

            edge = new GraphEdge(Nodes[1], Nodes[2]);
            Graph.Add(edge);
            Edges[1] = edge;

            edge = new GraphEdge(Nodes[4], Nodes[3]);
            Graph.Add(edge);
            Edges[2] = edge;

            edge = new GraphEdge(Nodes[1], Edges[2]);
            Graph.Add(edge);
            Edges[3] = edge;

            edge = new GraphEdge(Nodes[5], Nodes[8]);
            Graph.Add(edge);
            Edges[4] = edge;


            edge = new GraphEdge(Nodes[5], Nodes[6]);
            Graph.Add(edge);
            Edges[5] = edge;

            edge = new GraphEdge(Nodes[5], Nodes[7]);
            Graph.Add(edge);
            Edges[6] = edge;

            edge = new GraphEdge(Nodes[8], Nodes[9]);
            Graph.Add(edge);
            Edges[7] = edge;

            if (!SeperateLattice) {
                edge = new GraphEdge (Edges[2], Edges[4]);
                Graph.Add (edge);
                Edges[8] = edge;
            }

            #endregion
        }
    }
}