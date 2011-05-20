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

using Limada.Model;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model;
using Limaki.Tests;
using Limaki.Tests.Graph.Model;
using Limaki.UnitTest;
using NUnit.Framework;

namespace Limada.Tests.Model {
    public class GraphPairWithFactoryTest : DomainTest {
        public void TestGraphPair(IGraphFactory<IGraphItem, IGraphEdge> source, ThingGraph target) {
            source.Count = 10;
            source.Populate();
            this.ReportDetail(source.GetType().FullName + "\t" + source.Count);

            IGraphPair<IGraphItem, IThing, IGraphEdge, ILink> graphPair =
                new GraphPair<IGraphItem, IThing, IGraphEdge, ILink>(
                source.Graph, 
                target, 
                new GraphItem2ThingAdapter());

            GraphMapper<IGraphItem, IThing, IGraphEdge, ILink> mapper = graphPair.Mapper;
            mapper.ConvertOneTwo();

            MapperTester<IGraphItem, IThing, IGraphEdge, ILink>
                convertionTesterOne = new MapperTester<IGraphItem, IThing, IGraphEdge, ILink>();
            mapper.ConvertOneTwo();
            convertionTesterOne.ProveConversion(graphPair.One, graphPair.Two, mapper.Get);

            IGraphItem newItem = new GraphItem<string>("new");
            graphPair.Add(newItem);
            convertionTesterOne.ProveConversion(graphPair.One, graphPair.Two, mapper.Get);

        }

        [Test]
        public void TestGraphPairWithFactoryGraph() {
            TestGraphPair(new BinaryTreeFactory(),new ThingGraph ());
            TestGraphPair(new BinaryGraphFactory(), new ThingGraph());
            ReportSummary();
        }


    }

    public class GraphPairTest : DomainTest {

        public IGraphPair<IGraphItem, IThing, IGraphEdge, ILink> MakeGraphPair(
            IGraphFactory<IGraphItem, IGraphEdge> source, ThingGraph target) {

            source.Count = 10;
            source.Populate();

            GraphMapper<IGraphItem, IThing, IGraphEdge, ILink> mapper =
                new GraphMapper<IGraphItem, IThing, IGraphEdge, ILink>(
                    source.Graph, 
                    target, 
                    new GraphItem2ThingAdapter());

            mapper.ConvertOneTwo ();

            IGraphPair<IGraphItem, IThing, IGraphEdge, ILink> graphPair =
                new LiveGraphPair<IGraphItem, IThing, IGraphEdge, ILink>(
                    new Graph<IGraphItem, IGraphEdge>(), 
                    target, 
                    new GraphItem2ThingAdapter());

            return graphPair;

        }

        [Test]
        public void TestGraphPairGetSource() {
            IGraphPair<IGraphItem, IThing, IGraphEdge, ILink> data =
                MakeGraphPair (new BinaryGraphFactory (), new ThingGraph ());

            var view = new Graph<IGraphItem, IGraphEdge>();
            var graphView = new GraphView<IGraphItem, IGraphEdge>(data, view);

            var graphView2 = new GraphView<IGraphItem, IGraphEdge>(graphView, view);

            var result = GraphPairExtension<IGraphItem, IGraphEdge>.Source(graphView);
            Assert.AreSame(graphView, result);

            result = GraphPairExtension<IGraphItem, IGraphEdge>.Source(graphView2);
            Assert.AreSame(graphView, result);

            var result2 = GraphPairExtension<IGraphItem, IGraphEdge>.Source<IThing, ILink>(graphView);
            
            Assert.AreSame(data, result2);

            result2 = GraphPairExtension<IGraphItem, IGraphEdge>.Source<IThing, ILink>(graphView2);

            Assert.AreSame(data, result2);
        }

        [Test]
        public void TestGraphPairPingBack() {
            
            var data =
                MakeGraphPair (new BinaryGraphFactory (), new ThingGraph ());

            var graphView1 = new GraphView<IGraphItem, IGraphEdge> (data, new Graph<IGraphItem, IGraphEdge> ());
            var graphView2 = new GraphView<IGraphItem, IGraphEdge>(data, new Graph<IGraphItem, IGraphEdge>());

            

            foreach (IGraphItem ping in graphView1.Two) {
                IGraphItem back = GraphPairExtension<IGraphItem, IGraphEdge>.LookUp<IThing, ILink>(graphView1, graphView2, ping);
                Assert.IsNotNull (back);
                Assert.AreSame (ping, back);
            }

            data = new LiveGraphPair<IGraphItem, IThing, IGraphEdge, ILink> (
                new Graph<IGraphItem, IGraphEdge>(), 
                data.Two,new GraphItem2ThingAdapter());

            graphView2 = new GraphView<IGraphItem, IGraphEdge>(data, new Graph<IGraphItem, IGraphEdge>());
            
            foreach (IGraphItem ping in graphView1.Two) {
                IGraphItem back = GraphPairExtension<IGraphItem, IGraphEdge>.LookUp<IThing, ILink>(graphView1, graphView2, ping);
                Assert.IsNotNull(back);
                Assert.AreNotSame(ping, back);
                Assert.AreEqual (ping.ToString(), back.ToString());
            }

        }

        
    }

}