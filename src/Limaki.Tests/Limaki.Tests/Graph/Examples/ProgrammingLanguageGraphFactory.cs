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

    public class ProgrammingLanguageFactory : EntityGraphFactory {

        public override string Name {
            get { return "Programming Language"; }
        }

        public override void Populate(IGraph<IGraphEntity, IGraphEdge> Graph, int start) {
            GraphEntity<string> node = null;
            GraphEdge edge = null;


            node = new GraphEntity<string>("Programming");
            Graph.Add(node);
            Nodes[1] = node;


            node = new GraphEntity<string>("Language");
            Graph.Add(node);
            Nodes[2] = node;

            edge = new GraphEdge(Nodes[1], Nodes[2]);
            Graph.Add(edge);
            Edges[1] = edge;


            node = new GraphEntity<string>("Java");
            Graph.Add(node);
            Nodes[3] = node;

            edge = new GraphEdge(Edges[1], Nodes[3]);
            Graph.Add(edge);
            Edges[2] = edge;

            node = new GraphEntity<string>(".NET");
            Graph.Add(node);
            Nodes[4] = node;

            edge = new GraphEdge(Edges[1], Nodes[4]);
            Graph.Add(edge);
            Edges[3] = edge;

            node = new GraphEntity<string>("Libraries");
            Graph.Add(node);
            Nodes[5] = node;

            edge = new GraphEdge(Nodes[1], Nodes[5]);
            Graph.Add(edge);
            Edges[4] = edge;

            node = new GraphEntity<string>("Collections");
            Graph.Add(node);
            Nodes[6] = node;

            edge = new GraphEdge(Edges[4], Nodes[6]);
            Graph.Add(edge);
            Edges[5] = edge;

            node = new GraphEntity<string>("List");
            Graph.Add(node);
            Nodes[7] = node;

            edge = new GraphEdge(Edges[5], Nodes[7]);
            Graph.Add(edge);
            Edges[6] = edge;

            edge = new GraphEdge(Edges[2], Nodes[7]);
            Graph.Add(edge);
            Edges[9] = edge;

            node = new GraphEntity<string>("IList");
            Graph.Add(node);
            Nodes[8] = node;

            edge = new GraphEdge(Edges[5], Nodes[8]);
            Graph.Add(edge);
            Edges[7] = edge;

            edge = new GraphEdge(Edges[3], Nodes[8]);
            Graph.Add(edge);
            Edges[8] = edge;

        }

        public override void Populate() {
            IGraphEntity lastNode1 = null;
            IGraphEntity lastNode2 = null;
            IGraphEntity lastNode3 = null;
            for (int i = 0; i < Count; i++) {
                if (i > 0) {
                    lastNode1 = Nodes[1];
                    lastNode2 = Nodes[5];
                    lastNode3 = Nodes[8];
                }
                Populate(this.Graph,Start + 1);
                if (i > 0) {
                    GraphEdge edge = null;
                    if (!SeperateLattice) {
                        edge = new GraphEdge();
                        edge.Root = lastNode1;
                        edge.Leaf = Nodes[1];
                        Graph.Add(edge);


                    }
                    if (AddDensity) {
                        edge = new GraphEdge(Nodes[2], lastNode3);
                        Graph.Add(edge);
                    }
                }
            }
        }
    }
}