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

using Limaki.Model;
using Limaki.Visuals;
using Limaki.Tests.Graph.Model;
using System.IO;
using Limaki.Graphs;

namespace Limaki.Tests.Visuals {
    public class LimakiShortHelpFactory<IGraphEntity, IGraphEdge> : BasicTestGraphFactory<IGraphEntity, IGraphEdge>
        where IGraphEdge : IEdge<IGraphEntity>, IGraphEntity {
        public override void Populate(IGraph<IGraphEntity, IGraphEdge> Graph,int start) {
            IGraphEntity item = CreateItem<string>("Limaki");
            Graph.Add(item);
            Nodes[1] = item;

            item = CreateItem<string>("How To");
            Graph.Add(item);
            Nodes[2] = item;

            Edges[1] = CreateEdge(Nodes[1], Nodes[2]);
            Graph.Add(Edges[1]);


            item = CreateItem<string>("Warning");
            Graph.Add(item);
            Nodes[3] = item;

            Edges[2] = CreateEdge(Nodes[1], Nodes[3]);
            Graph.Add(Edges[2]);

            item = CreateItem<string>("This is a pre-alpha release");
            Graph.Add(item);
            Graph.Add(CreateEdge(Nodes[3], item));

            item = CreateItem<string>("Saved files will NOT be supported in later releases");
            Graph.Add(item);
            Graph.Add(CreateEdge(Nodes[3], item));

            IGraphEntity topic = CreateItem<string>("add new nodes");
            Graph.Add(topic);
            Graph.Add(CreateEdge(Nodes[2], topic));

            IGraphEntity subtopic = CreateItem<string>("click the Add Shape button");
            IGraphEdge subLink = default (IGraphEdge); ;

            Graph.Add(subtopic);
            Graph.Add(CreateEdge(topic, subtopic));

            item = CreateItem<string>("Draw new nodes");
            Graph.Add(item);
            Graph.Add(CreateEdge(subtopic, item));


            topic = CreateItem<string>("connect nodes");
            Graph.Add(topic);
            Graph.Add(CreateEdge(Nodes[2], topic));

            
            subtopic = CreateItem<string>("click the select button");
            Graph.Add(subtopic);
            Graph.Add(subLink = CreateEdge(topic, subtopic));

            item = CreateItem<string>("Drag a node(or link) over an other node (or link)");
            Graph.Add(item);
            Graph.Add(CreateEdge(subLink, item));

            topic = CreateItem<string>("edit nodes or links");
            Graph.Add(topic);
            Graph.Add(CreateEdge(Nodes[2], topic));
            Graph.Add(subLink = CreateEdge(topic, subtopic));

            item = CreateItem<string>("Press F2, edit, cancel with ESC or save with F2 again");
            Graph.Add(item);
            Graph.Add(CreateEdge(subLink, item));


            topic = CreateItem<string>("expand and collapse nodes");
            Graph.Add(topic);
            Graph.Add(CreateEdge(Nodes[2], topic));

            subtopic = CreateItem<string>("press + to expand");
            Graph.Add(subtopic);
            Graph.Add(CreateEdge(topic, subtopic));

            subtopic = CreateItem<string>("press - to collapse");
            Graph.Add(subtopic);
            Graph.Add(CreateEdge(topic, subtopic));

            subtopic = CreateItem<string>("press / to reduce to focused item");
            Graph.Add(subtopic);
            Graph.Add(CreateEdge(topic, subtopic));

            subtopic = CreateItem<string>("press * to show all items");
            Graph.Add(subtopic);
            Graph.Add(CreateEdge(topic, subtopic));

        }

        public override string Name {
            get { return "LimakiShortHelp"; }
        }
    }
}

