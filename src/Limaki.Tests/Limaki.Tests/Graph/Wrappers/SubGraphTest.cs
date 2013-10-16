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
using System.Text;
using Limaki.Graphs.Extensions;
using Limaki.Model;
using NUnit.Framework;
using Limaki.Common;
using Limaki.Graphs;
using Limaki.UnitTest;
using Limaki.Tests.Graph.Model;

namespace Limaki.Tests.Graph.Wrappers {
    public class SubGraphTest: DomainTest {
        /// <summary>
        /// this tests a semantic error in SubGraph
        /// SubGraph should only contain edges contained in view
        /// under all cirumstances
        /// but e.g. Graph.Fork breaks this rule
        /// </summary>
        [Test]
        public  void EdgeNotInView() {
            var source = new Graph<IGraphEntity, IGraphEdge> ();
            var sink = new Graph<IGraphEntity, IGraphEdge>();
            var subGraph = new SubGraph<IGraphEntity, IGraphEdge> (source,sink);
            

            var one = new GraphEntity<string> ("1");
            var two = new GraphEntity<string>("2");
            var three = new GraphEntity<string>("3");
            source.Add (one);
            source.Add(two);
            source.Add(three);
            var link = new GraphEdge (one, two);
            var linklink = new GraphEdge (link, three);
            source.Add (linklink);
            sink.Add (one);
            sink.Add (two);
            sink.Add (link);
            Assert.IsTrue(source.Contains(link), "view has to contain link");
            Assert.IsTrue(source.Contains(linklink), "view has to contain linklink");
            Assert.IsFalse (sink.Contains (linklink),"view must not contain linklink");
            Assert.IsFalse(subGraph.Contains(linklink), "graphview must not contain linklink");
            sink.Add (linklink);
            sink.Remove (link);
            Assert.IsFalse(subGraph.Contains(linklink), "graphview must not contain linklink");
            sink.Add (link);

            IGraph<IGraphEntity, IGraphEdge> graph = subGraph;

            foreach (var edge in subGraph.Fork(three)) {
                Assert.IsFalse(edge.Equals(linklink), "graphview must not contain linklink");
            }

            bool found = false;
            foreach (var edge in source.Fork(three)) {
                if (edge == linklink) {
                    found = true;
                    break;
                }
            }
            Assert.IsTrue(found, "data.fork has to contain linklink");
        }

        [Test]
        public void SubGraphAsGraphPairTest() {
            var source = new Graph<IGraphEntity, IGraphEdge>();
            var sink = new Graph<IGraphEntity, IGraphEdge>();
            var subGraph = new SubGraph<IGraphEntity, IGraphEdge>(source, sink);

            var subGraph2 = new SubGraph<IGraphEntity, IGraphEdge>(subGraph, sink);

            var result = subGraph.RootSource();
            Assert.AreSame (subGraph, result);

            result = subGraph2.RootSource();
            Assert.AreSame(subGraph, result);
        }
    }
}
