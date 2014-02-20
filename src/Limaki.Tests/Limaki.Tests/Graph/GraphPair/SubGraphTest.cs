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
        /// SubGraph should only contain edges contained in sink
        /// under all cirumstances
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
            Assert.IsTrue (source.Contains (link), "source has to contain link");
            Assert.IsTrue (source.Contains (linklink), "source has to contain linklink");
            Assert.IsFalse (sink.Contains (linklink), "sink must not contain linklink");
            Assert.IsFalse (subGraph.Contains (linklink), "subGraph must not contain linklink");
            sink.Add (linklink);

            sink.Remove (link);
            Assert.IsFalse (subGraph.Contains (linklink), "subGraph must not contain linklink");

            sink.Add (link);

            foreach (var edge in subGraph.Fork(three)) {
                Assert.IsFalse (edge.Equals (linklink), "subGraph must not contain linklink");
            }

            bool found = false;
            foreach (var edge in source.Fork(three)) {
                if (edge == linklink) {
                    found = true;
                    break;
                }
            }
            Assert.IsTrue (found, "source.fork has to contain linklink");
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

        [Test]
        public void ChangeEdge () {
            var source = new Graph<IGraphEntity, IGraphEdge> ();
            var sink = new Graph<IGraphEntity, IGraphEdge> ();
            var subGraph = new SubGraph<IGraphEntity, IGraphEdge> (source, sink);

            var one = new GraphEntity<string> ("1");
            var two = new GraphEntity<string> ("2");
            var three = new GraphEntity<string> ("3");
            var link = new GraphEdge (one, two);
            subGraph.Add (one);
            subGraph.Add (two);
            subGraph.Add (three);
            subGraph.Add (link);

            Assert.IsTrue (source.Contains (link), "source has to contain link");
            Assert.IsTrue (sink.Contains (link), "sink has to contain link");

            subGraph.ChangeEdge (link, three, true);

            Assert.IsFalse (sink.Edges (one).Contains (link), "sink.Edges (one) has not to contain link");
            Assert.IsFalse (source.Edges (one).Contains (link), "source.Edges (one) has not to contain link");
            Assert.IsFalse (subGraph.Edges (one).Contains (link), "subGraph.Edges (one) has not to contain link");
       
        }

       
    }
}
