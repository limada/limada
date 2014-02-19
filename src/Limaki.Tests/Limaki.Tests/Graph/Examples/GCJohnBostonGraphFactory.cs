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
    public class GCJohnBostonGraphFactory : EntityGraphFactory {

        public override string Name {
            get { return "GC John going to Boston"; }
        }

        public override void Populate(IGraph<IGraphEntity, IGraphEdge> Graph,int start) {
            GraphEntity<string> node = null;
            GraphEdge edge = null;


            node = new GraphEntity<string>("Person");
            Graph.Add(node);
            Nodes[1] = node;


            node = new GraphEntity<string>("John");
            Graph.Add(node);
            Nodes[2] = node;

            edge = new GraphEdge(Nodes[1], Nodes[2]);
            Graph.Add(edge);
            Edges[1] = edge;


            node = new GraphEntity<string>("City");
            Graph.Add(node);
            Nodes[3] = node;

            node = new GraphEntity<string>("Boston");
            Graph.Add(node);
            Nodes[4] = node;

            edge = new GraphEdge(Nodes[3], Nodes[4]);
            Graph.Add(edge);
            Edges[2] = edge;

            node = new GraphEntity<string>("Go");
            Graph.Add(node);
            Nodes[5] = node;


            edge = new GraphEdge(Edges[1], Nodes[5]);
            Graph.Add(edge);
            Edges[3] = edge;



            edge = new GraphEdge(Edges[3],Edges[2]);
            Graph.Add(edge);
            Edges[4] = edge;

            node = new GraphEntity<string>("Bus");
            Graph.Add(node);
            Nodes[6] = node;

            edge = new GraphEdge(Edges[4], Nodes[6]);
            Graph.Add(edge);
            Edges[5] = edge;

            

        }

       
    }
}