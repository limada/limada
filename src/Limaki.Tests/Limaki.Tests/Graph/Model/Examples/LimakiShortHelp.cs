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
 * http://limada.sourceforge.net
 * 
 */

using Limaki.Model;
using Limaki.Visuals;
using Limaki.Tests.Graph.Model;
using System.IO;
using Limaki.Graphs;

namespace Limaki.Tests.Visuals {
    public class LimakiShortHelpFactory : GraphFactoryBase {
        public override void Populate(IGraph<IGraphItem, IGraphEdge> Graph,int start) {
            IGraphItem item = new GraphItem<string>("Limaki");
            Graph.Add(item);
            Node[1] = item;

            item = new GraphItem<string>("How To");
            Graph.Add(item);
            Node[2] = item;

            Edge[1] = new GraphEdge(Node[1], Node[2]);
            Graph.Add(Edge[1]);


            item = new GraphItem<string>("Warning");
            Graph.Add(item);
            Node[3] = item;

            Edge[2] = new GraphEdge(Node[1], Node[3]);
            Graph.Add(Edge[2]);

            item = new GraphItem<string>("This is a pre-alpha release");
            Graph.Add(item);
            Graph.Add(new GraphEdge(Node[3], item));

            item = new GraphItem<string>("Saved files will NOT be supported in later releases");
            Graph.Add(item);
            Graph.Add(new GraphEdge(Node[3], item));

            IGraphItem topic = new GraphItem<string>("add new nodes");
            Graph.Add(topic);
            Graph.Add(new GraphEdge(Node[2], topic));

            IGraphItem subtopic = new GraphItem<string>("click the Add Shape button");
            IGraphEdge subLink = null;

            Graph.Add(subtopic);
            Graph.Add(new GraphEdge(topic, subtopic));

            item = new GraphItem<string>("Draw new nodes");
            Graph.Add(item);
            Graph.Add(new GraphEdge(subtopic, item));


            topic = new GraphItem<string>("connect nodes");
            Graph.Add(topic);
            Graph.Add(new GraphEdge(Node[2], topic));

            
            subtopic = new GraphItem<string>("click the select button");
            Graph.Add(subtopic);
            Graph.Add(subLink = new GraphEdge(topic, subtopic));

            item = new GraphItem<string>("Drag a node(or link) over an other node (or link)");
            Graph.Add(item);
            Graph.Add(new GraphEdge(subLink, item));

            topic = new GraphItem<string>("edit nodes or links");
            Graph.Add(topic);
            Graph.Add(new GraphEdge(Node[2], topic));
            Graph.Add(subLink = new GraphEdge(topic, subtopic));

            item = new GraphItem<string>("Press F2, edit, cancel with ESC or save with F2 again");
            Graph.Add(item);
            Graph.Add(new GraphEdge(subLink, item));


            topic = new GraphItem<string>("expand and collapse nodes");
            Graph.Add(topic);
            Graph.Add(new GraphEdge(Node[2], topic));

            subtopic = new GraphItem<string>("press + to expand");
            Graph.Add(subtopic);
            Graph.Add(new GraphEdge(topic, subtopic));

            subtopic = new GraphItem<string>("press - to collapse");
            Graph.Add(subtopic);
            Graph.Add(new GraphEdge(topic, subtopic));

            subtopic = new GraphItem<string>("press / to reduce to focused item");
            Graph.Add(subtopic);
            Graph.Add(new GraphEdge(topic, subtopic));

            subtopic = new GraphItem<string>("press * to show all items");
            Graph.Add(subtopic);
            Graph.Add(new GraphEdge(topic, subtopic));

        }

        public override string Name {
            get { return "LimakiShortHelp"; }
        }
    }
}

