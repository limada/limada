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
        protected virtual IGraphPair<IVisual, IGraphItem, IVisualEdge, IGraphEdge> GetPair() {
            IGraph<IVisual, IVisualEdge> one = new Graph<IVisual, IVisualEdge>();
            IGraph<IGraphItem, IGraphEdge> two = new Graph<IGraphItem, IGraphEdge>();

            return
                new GraphPair<IVisual, IGraphItem, IVisualEdge, IGraphEdge>(
                one, two, new GraphItem2VisualAdapter().ReverseAdapter());
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

                Assert.IsTrue(pair.One.Contains(edge), edge.ToString());
                Assert.IsTrue(pair.One.Contains(edge.Root), edge.Root.ToString());
                Assert.IsTrue(pair.One.Contains(edge.Leaf), edge.Leaf.ToString());

                IGraphEdge edge2 = (IGraphEdge)pair.Get(edge);
                IGraphItem root2 = pair.Get (edge.Root);
                IGraphItem leaf2 = pair.Get(edge.Leaf);

                Assert.IsTrue(pair.Two.Contains(edge2), edge2.ToString());
                Assert.IsTrue(pair.Two.Contains(edge2.Root), edge2.Root.ToString());
                Assert.IsTrue(pair.Two.Contains(edge2.Leaf), edge2.Leaf.ToString());

                Assert.IsTrue(pair.Two.Contains(root2), root2.ToString());
                Assert.IsTrue(pair.Two.Contains(leaf2), leaf2.ToString());


            }

            var item2 = pair.Get(factory.One);
            pair.Remove(factory.One);

            Assert.IsFalse(pair.Contains(factory.One), factory.One.ToString());
            Assert.IsFalse(pair.One.Contains(factory.One), factory.One.ToString());

            Assert.IsFalse(pair.Two.Contains(item2), "pair.Two contains:\t" + item2.ToString());

            var pingback = pair.Get(item2);
            Assert.IsNull(pingback, "pair.Get(item) must be null\t" + item2.ToString());


        }

    }
}
