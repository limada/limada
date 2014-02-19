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
using Limaki.Tests.Graph.Model;
using Limaki.Tests.View.Visuals;
using Limaki.Tests.Visuals;
using Limaki.UnitTest;
using Limaki.Visuals;
using NUnit.Framework;

namespace Limaki.Tests.Graph.Wrappers {
    public class GraphPairTest : DomainTest {
        protected virtual IGraphPair<IVisual, IGraphEntity, IVisualEdge, IGraphEdge> GetPair() {
            IGraph<IVisual, IVisualEdge> one = new Graph<IVisual, IVisualEdge>();
            IGraph<IGraphEntity, IGraphEdge> two = new Graph<IGraphEntity, IGraphEdge>();

            return
                new GraphPair<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>(
                one, two, new GraphItem2VisualTransformer().Reverted());
        }

        [Test]
        public void RemoveTest() {
            var pair = GetPair();

            var factory = new VisualDataFactory();
            foreach (var item in factory.Edges) {
                pair.Add(item);
            }
            foreach (IVisualEdge edge in factory.Edges) {
                Assert.IsTrue(pair.Contains(edge), edge.ToString());
                Assert.IsTrue(pair.Contains(edge.Root), edge.Root.ToString());
                Assert.IsTrue(pair.Contains(edge.Leaf), edge.Leaf.ToString());

                Assert.IsTrue(pair.Sink.Contains(edge), edge.ToString());
                Assert.IsTrue(pair.Sink.Contains(edge.Root), edge.Root.ToString());
                Assert.IsTrue(pair.Sink.Contains(edge.Leaf), edge.Leaf.ToString());

                IGraphEdge edge2 = (IGraphEdge)pair.Get(edge);
                IGraphEntity root2 = pair.Get (edge.Root);
                IGraphEntity leaf2 = pair.Get(edge.Leaf);

                Assert.IsTrue(pair.Source.Contains(edge2), edge2.ToString());
                Assert.IsTrue(pair.Source.Contains(edge2.Root), edge2.Root.ToString());
                Assert.IsTrue(pair.Source.Contains(edge2.Leaf), edge2.Leaf.ToString());

                Assert.IsTrue(pair.Source.Contains(root2), root2.ToString());
                Assert.IsTrue(pair.Source.Contains(leaf2), leaf2.ToString());


            }

            var item2 = pair.Get(factory.One);
            pair.Remove(factory.One);

            Assert.IsFalse(pair.Contains(factory.One), factory.One.ToString());
            Assert.IsFalse(pair.Sink.Contains(factory.One), factory.One.ToString());

            Assert.IsFalse(pair.Source.Contains(item2), "pair.Source contains:\t" + item2.ToString());

            var pingback = pair.Get(item2);
            Assert.IsNull(pingback, "pair.Get(item) must be null\t" + item2.ToString());


        }

    }
}
