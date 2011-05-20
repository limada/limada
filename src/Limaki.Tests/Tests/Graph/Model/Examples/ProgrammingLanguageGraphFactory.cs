/*
 * Limaki 
 * Version 0.081
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
using Limaki.Model;

namespace Limaki.Tests.Graph.Model {
    public class ProgrammingLanguageFactory : GraphFactoryBase {

        public override string Name {
            get { return "Programming Language"; }
        }

        public override void Populate(IGraph<IGraphItem, IGraphEdge> Graph, int start) {
            GraphItem<string> node = null;
            GraphEdge edge = null;


            node = new GraphItem<string>("Programming");
            Graph.Add(node);
            Node[1] = node;


            node = new GraphItem<string>("Language");
            Graph.Add(node);
            Node[2] = node;

            edge = new GraphEdge(Node[1], Node[2]);
            Graph.Add(edge);
            Edge[1] = edge;


            node = new GraphItem<string>("Java");
            Graph.Add(node);
            Node[3] = node;

            edge = new GraphEdge(Edge[1], Node[3]);
            Graph.Add(edge);
            Edge[2] = edge;

            node = new GraphItem<string>(".NET");
            Graph.Add(node);
            Node[4] = node;

            edge = new GraphEdge(Edge[1], Node[4]);
            Graph.Add(edge);
            Edge[3] = edge;

            node = new GraphItem<string>("Libraries");
            Graph.Add(node);
            Node[5] = node;

            edge = new GraphEdge(Node[1], Node[5]);
            Graph.Add(edge);
            Edge[4] = edge;

            node = new GraphItem<string>("Collections");
            Graph.Add(node);
            Node[6] = node;

            edge = new GraphEdge(Edge[4], Node[6]);
            Graph.Add(edge);
            Edge[5] = edge;

            node = new GraphItem<string>("List");
            Graph.Add(node);
            Node[7] = node;

            edge = new GraphEdge(Edge[5], Node[7]);
            Graph.Add(edge);
            Edge[6] = edge;

            edge = new GraphEdge(Edge[2], Node[7]);
            Graph.Add(edge);
            Edge[9] = edge;

            node = new GraphItem<string>("IList");
            Graph.Add(node);
            Node[8] = node;

            edge = new GraphEdge(Edge[5], Node[8]);
            Graph.Add(edge);
            Edge[7] = edge;

            edge = new GraphEdge(Edge[3], Node[8]);
            Graph.Add(edge);
            Edge[8] = edge;

        }

        public override void Populate() {
            IGraphItem lastNode1 = null;
            IGraphItem lastNode2 = null;
            IGraphItem lastNode3 = null;
            for (int i = 0; i < Count; i++) {
                if (i > 0) {
                    lastNode1 = Node[1];
                    lastNode2 = Node[5];
                    lastNode3 = Node[8];
                }
                Populate(this.Graph,Start + 1);
                if (i > 0) {
                    GraphEdge edge = null;
                    if (!SeperateLattice) {
                        edge = new GraphEdge();
                        edge.Root = lastNode1;
                        edge.Leaf = Node[1];
                        Graph.Add(edge);


                    }
                    if (AddDensity) {
                        edge = new GraphEdge(Node[2], lastNode3);
                        Graph.Add(edge);
                    }
                }
            }
        }
    }
}