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

    public class ProgrammingLanguageFactory : ProgrammingLanguageFactory<IGraphEntity, IGraphEdge> { }

    public class ProgrammingLanguageFactory <IGraphEntity, IGraphEdge> : BasicTestGraphFactory<IGraphEntity, IGraphEdge>
        where IGraphEdge : IEdge<IGraphEntity>, IGraphEntity {

        public override string Name {
            get { return "Programming Language"; }
        }

        public override void Populate(IGraph<IGraphEntity, IGraphEdge> Graph, int start) {
            IGraphEntity node = default (IGraphEntity);
            IGraphEdge edge = default (IGraphEdge);


            node = CreateItem<string>("Programming");
            Graph.Add(node);
            Nodes[1] = node;


            node = CreateItem<string>("Language");
            Graph.Add(node);
            Nodes[2] = node;

            edge = CreateEdge(Nodes[1], Nodes[2]);
            Graph.Add(edge);
            Edges[1] = edge;


            node = CreateItem<string>("Java");
            Graph.Add(node);
            Nodes[3] = node;

            edge = CreateEdge(Edges[1], Nodes[3]);
            Graph.Add(edge);
            Edges[2] = edge;

            node = CreateItem<string>(".NET");
            Graph.Add(node);
            Nodes[4] = node;

            edge = CreateEdge(Edges[1], Nodes[4]);
            Graph.Add(edge);
            Edges[3] = edge;

            node = CreateItem<string>("Libraries");
            Graph.Add(node);
            Nodes[5] = node;

            edge = CreateEdge(Nodes[1], Nodes[5]);
            Graph.Add(edge);
            Edges[4] = edge;

            node = CreateItem<string>("Collections");
            Graph.Add(node);
            Nodes[6] = node;

            edge = CreateEdge(Edges[4], Nodes[6]);
            Graph.Add(edge);
            Edges[5] = edge;

            node = CreateItem<string>("List");
            Graph.Add(node);
            Nodes[7] = node;

            edge = CreateEdge(Edges[5], Nodes[7]);
            Graph.Add(edge);
            Edges[6] = edge;

            edge = CreateEdge(Edges[2], Nodes[7]);
            Graph.Add(edge);
            Edges[9] = edge;

            node = CreateItem<string>("IList");
            Graph.Add(node);
            Nodes[8] = node;

            edge = CreateEdge(Edges[5], Nodes[8]);
            Graph.Add(edge);
            Edges[7] = edge;

            edge = CreateEdge(Edges[3], Nodes[8]);
            Graph.Add(edge);
            Edges[8] = edge;

        }

        public override void Populate() {
            IGraphEntity lastNode1 = default (IGraphEntity); ;
            IGraphEntity lastNode2 = default (IGraphEntity); ;
            IGraphEntity lastNode3 = default (IGraphEntity); ;
            for (int i = 0; i < Count; i++) {
                if (i > 0) {
                    lastNode1 = Nodes[1];
                    lastNode2 = Nodes[5];
                    lastNode3 = Nodes[8];
                }
                Populate(this.Graph,Start + 1);
                if (i > 0) {
                    IGraphEdge edge = default (IGraphEdge); ;
                    if (!SeperateLattice) {
                        edge = CreateEdge();
                        edge.Root = lastNode1;
                        edge.Leaf = Nodes[1];
                        Graph.Add(edge);


                    }
                    if (AddDensity) {
                        edge = CreateEdge(Nodes[2], lastNode3);
                        Graph.Add(edge);
                    }
                }
            }
        }
    }
}