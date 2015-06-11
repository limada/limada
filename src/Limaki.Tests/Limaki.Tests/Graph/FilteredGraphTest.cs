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


using System;
using System.Collections.Generic;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.UnitTest;
using Limaki.Tests.Graph.Model;
using NUnit.Framework;
using System.IO;
using Limaki.Common.Collections;
using System.Collections.ObjectModel;
using Limaki.Tests.Graph.Basic;

namespace Limaki.Tests.Graph {
    public class FilteredGraphTest : DomainTest {

        [Test]
        public void BasicGraphTest() {
            var test = new StringGraphTest();
            var graph = new FilteredGraph<string, Edge<string>>(test.Graph);
            graph.ItemFilter = delegate(string s) { return true; };
            graph.EdgeFilter = delegate(Edge<string> s) { return true; };
            test.Graph = graph;
            test.AllTests ();
        }

        [Test]
        public void OnlyItemsInListTest() {
            var items = new Set<IGraphEntity> ();
            var data = new EntityBinaryTreeFactory ();
            data.Count = 1;
            data.Populate ();

            var graph = 
                new FilteredGraph<IGraphEntity, IGraphEdge> (data.Graph);
            
            graph.ItemFilter = delegate(IGraphEntity item) {
                                   return items.Contains(item);
                               };

            graph.EdgeFilter = delegate(IGraphEdge edge) {
                                   return 
                                       items.Contains(edge.Root)
                                       &&
                                       items.Contains(edge.Leaf);
                               };

            items.Add(data.Nodes[1]);
            items.Add(data.Nodes[2]);

            this.ReportDetail(GraphTestUtils.ReportGraph<IGraphEntity, IGraphEdge> (graph, "Filtered Graph"));

        }
    }
}
