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

namespace Limaki.Tests.Graph.Model {

    public class LimakiShortHelpFactory<TItem, TEdge> : SampleGraphFactory<TItem, TEdge>
        where TEdge : IEdge<TItem>, TItem {

        public override void Populate (IGraph<TItem, TEdge> graph, int start) {

            var item = CreateItem<string> ("Limaki");
            graph.Add (item);
            Nodes[1] = item;

            item = CreateItem<string> ("How To");
            graph.Add (item);
            Nodes[2] = item;

            Edges[1] = CreateEdge (Nodes[1], Nodes[2]);
            graph.Add (Edges[1]);


            item = CreateItem<string> ("Warning");
            graph.Add (item);
            Nodes[3] = item;

            Edges[2] = CreateEdge (Nodes[1], Nodes[3]);
            graph.Add (Edges[2]);

            item = CreateItem<string> ("This is a pre-alpha release");
            graph.Add (item);
            graph.Add (CreateEdge (Nodes[3], item));

            item = CreateItem<string> ("Saved files will NOT be supported in later releases");
            graph.Add (item);
            graph.Add (CreateEdge (Nodes[3], item));

            TItem topic = CreateItem<string> ("add new nodes");
            graph.Add (topic);
            graph.Add (CreateEdge (Nodes[2], topic));

            TItem subtopic = CreateItem<string> ("click the Add Shape button");
            TEdge subLink = default (TEdge); ;

            graph.Add (subtopic);
            graph.Add (CreateEdge (topic, subtopic));

            item = CreateItem<string> ("Draw new nodes");
            graph.Add (item);
            graph.Add (CreateEdge (subtopic, item));


            topic = CreateItem<string> ("connect nodes");
            graph.Add (topic);
            graph.Add (CreateEdge (Nodes[2], topic));


            subtopic = CreateItem<string> ("click the select button");
            graph.Add (subtopic);
            graph.Add (subLink = CreateEdge (topic, subtopic));

            item = CreateItem<string> ("Drag a node(or link) over an other node (or link)");
            graph.Add (item);
            graph.Add (CreateEdge (subLink, item));

            topic = CreateItem<string> ("edit nodes or links");
            graph.Add (topic);
            graph.Add (CreateEdge (Nodes[2], topic));
            graph.Add (subLink = CreateEdge (topic, subtopic));

            item = CreateItem<string> ("Press F2, edit, cancel with ESC or save with F2 again");
            graph.Add (item);
            graph.Add (CreateEdge (subLink, item));


            topic = CreateItem<string> ("expand and collapse nodes");
            graph.Add (topic);
            graph.Add (CreateEdge (Nodes[2], topic));

            subtopic = CreateItem<string> ("press + to expand");
            graph.Add (subtopic);
            graph.Add (CreateEdge (topic, subtopic));

            subtopic = CreateItem<string> ("press - to collapse");
            graph.Add (subtopic);
            graph.Add (CreateEdge (topic, subtopic));

            subtopic = CreateItem<string> ("press / to reduce to focused item");
            graph.Add (subtopic);
            graph.Add (CreateEdge (topic, subtopic));

            subtopic = CreateItem<string> ("press * to show all items");
            graph.Add (subtopic);
            graph.Add (CreateEdge (topic, subtopic));

        }

        public override string Name {
            get { return "LimakiShortHelp"; }
        }
    }
}

