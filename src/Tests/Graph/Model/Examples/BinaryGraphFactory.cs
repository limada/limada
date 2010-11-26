/*
 * Limaki 
 * Version 0.071
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
            Link[1] = edge;

            edge = new GraphEdge(Node[4], Node[3]);
            Graph.Add(edge);
            Link[2] = edge;

            edge = new GraphEdge(Node[1], Link[2]);
            Graph.Add(edge);
            Link[3] = edge;

            edge = new GraphEdge(Node[5], Node[8]);
            Graph.Add(edge);
            Link[4] = edge;


            edge = new GraphEdge(Node[5], Node[6]);
            Graph.Add(edge);
            Link[5] = edge;

            edge = new GraphEdge(Node[5], Node[7]);
            Graph.Add(edge);
            Link[6] = edge;

            edge = new GraphEdge(Node[8], Node[9]);
            Graph.Add(edge);
            Link[7] = edge;

            if (!SeperateLattice) {
                edge = new GraphEdge (Link[2], Link[4]);
                Graph.Add (edge);
                Link[8] = edge;
            }

            #endregion
        }
    }
}