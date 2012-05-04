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
    public class BinaryGraphFactory : GraphFactoryBase {

        public override string Name {
            get { return "Binary Graph"; }
        }

        public override void Populate(IGraph<IGraphItem, IGraphEdge> Graph, int start) {

            base.Populate(Graph,start);

            GraphEdge edge = null;

            #region Binarytree Edges

            edge = new GraphEdge(Node[1], Node[2]);
            Graph.Add(edge);
            Edge[1] = edge;

            edge = new GraphEdge(Node[4], Node[3]);
            Graph.Add(edge);
            Edge[2] = edge;

            edge = new GraphEdge(Node[1], Edge[2]);
            Graph.Add(edge);
            Edge[3] = edge;

            edge = new GraphEdge(Node[5], Node[8]);
            Graph.Add(edge);
            Edge[4] = edge;


            edge = new GraphEdge(Node[5], Node[6]);
            Graph.Add(edge);
            Edge[5] = edge;

            edge = new GraphEdge(Node[5], Node[7]);
            Graph.Add(edge);
            Edge[6] = edge;

            edge = new GraphEdge(Node[8], Node[9]);
            Graph.Add(edge);
            Edge[7] = edge;

            if (!SeperateLattice) {
                edge = new GraphEdge (Edge[2], Edge[4]);
                Graph.Add (edge);
                Edge[8] = edge;
            }

            #endregion
        }
    }
}