/*
 * Limaki 
 * Version 0.08
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


using System;
using System.Collections.Generic;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.UnitTest;
using Limaki.Tests.Graph;
using Limaki.Tests.Graph.Model;
using NUnit.Framework;
using System.IO;
using Limaki.Common.Collections;
using System.Collections.ObjectModel;
using Limaki.Tests.Graph.Basic;

namespace Limaki.Tests.Sandbox.Graph {
    public class FilteredGraphTest : DomainTest {

        [Test]
        public void BasicGraphTest() {
            StringGraphTest test = new StringGraphTest();
            FilteredGraph<string, Edge<string>> graph = 
                new FilteredGraph<string, Edge<string>>(test.Graph);
            graph.ItemFilter = delegate(string s) { return true; };
            graph.EdgeFilter = delegate(Edge<string> s) { return true; };
            test.Graph = graph;
            test.AllTests ();
        }

        [Test]
        public void OnlyItemsInListTest() {
            Set<IGraphItem> items = new Set<IGraphItem> ();
            GraphFactoryBase data = new BinaryTreeFactory ();
            data.Count = 1;
            data.Populate ();

            FilteredGraph<IGraphItem, IGraphEdge> graph = 
                new FilteredGraph<IGraphItem, IGraphEdge> (data.Graph);
            
            graph.ItemFilter = delegate(IGraphItem item) {
                                   return items.Contains(item);
                               };

            graph.EdgeFilter = delegate(IGraphEdge edge) {
                                   return 
                                       items.Contains(edge.Root)
                                       &&
                                       items.Contains(edge.Leaf);
                               };

            items.Add(data.Node[1]);
            items.Add(data.Node[2]);

            this.ReportDetail(GraphTestUtils.ReportGraph<IGraphItem, IGraphEdge> (graph, "Filtered Graph"));

        }
    }
}
