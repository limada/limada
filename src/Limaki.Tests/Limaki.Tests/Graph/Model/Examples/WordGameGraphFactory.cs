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
    public class WordGameGraphFactory : GraphFactoryBase {

        public override string Name {
            get { return "Word Game"; }
        }

        public override void Populate(IGraph<IGraphEntity, IGraphEdge> Graph, int start) {
            GraphEntity<string> node = null;
            GraphEdge edge = null;


            node = new GraphEntity<string>("Tags");
            Graph.Add(node);
            Node[1] = node;


            node = new GraphEntity<string>("Word");
            Graph.Add(node);
            Node[2] = node;

            edge = new GraphEdge(Node[1], Node[2]);
            Graph.Add(edge);
            Edge[1] = edge;


            node = new GraphEntity<string>("Game");
            Graph.Add(node);
            Node[3] = node;

            edge = new GraphEdge(Node[2], Node[3]);
            Graph.Add(edge);
            Edge[2] = edge;

            node = new GraphEntity<string>("Something");
            Graph.Add(node);
            Node[4] = node;

            edge = new GraphEdge(Edge[2], Node[4]);
            Graph.Add(edge);
            Edge[3] = edge;

            edge = new GraphEdge(Node[1], Edge[2]);
            Graph.Add(edge);
            Edge[4] = edge;

            

        }

           
    }
}