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
    public class GCJohnBostonGraphFactory : GraphFactoryBase {

        public override string Name {
            get { return "GC John going to Boston"; }
        }

        public override void Populate(IGraph<IGraphItem, IGraphEdge> Graph,int start) {
            GraphItem<string> node = null;
            GraphEdge edge = null;


            node = new GraphItem<string>("Person");
            Graph.Add(node);
            Node[1] = node;


            node = new GraphItem<string>("John");
            Graph.Add(node);
            Node[2] = node;

            edge = new GraphEdge(Node[1], Node[2]);
            Graph.Add(edge);
            Edge[1] = edge;


            node = new GraphItem<string>("City");
            Graph.Add(node);
            Node[3] = node;

            node = new GraphItem<string>("Boston");
            Graph.Add(node);
            Node[4] = node;

            edge = new GraphEdge(Node[3], Node[4]);
            Graph.Add(edge);
            Edge[2] = edge;

            node = new GraphItem<string>("Go");
            Graph.Add(node);
            Node[5] = node;


            edge = new GraphEdge(Edge[1], Node[5]);
            Graph.Add(edge);
            Edge[3] = edge;



            edge = new GraphEdge(Edge[3],Edge[2]);
            Graph.Add(edge);
            Edge[4] = edge;

            node = new GraphItem<string>("Bus");
            Graph.Add(node);
            Node[6] = node;

            edge = new GraphEdge(Edge[4], Node[6]);
            Graph.Add(edge);
            Edge[5] = edge;

            

        }

       
    }
}