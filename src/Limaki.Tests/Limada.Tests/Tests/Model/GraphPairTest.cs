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

using Limada.Model;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model;
using Limaki.Tests;
using Limaki.Tests.Graph.Model;
using Limaki.UnitTest;
using NUnit.Framework;
using System.Linq;

namespace Limada.Tests.Model {

    public class GraphPairWithFactoryTest : DomainTest {

        public void TestGraphPair(ITestGraphFactory<IGraphEntity, IGraphEdge> source, ThingGraph target) {
            source.Count = 10;
            source.Populate();
            this.ReportDetail(source.GetType().FullName + "\t" + source.Count);

            var graphPair =
                new GraphPair<IGraphEntity, IThing, IGraphEdge, ILink>(
                source.Graph, 
                target, 
                new GraphItem2ThingTransformer());

            var mapper = graphPair.Mapper;
            mapper.ConvertSinkSource();

            var convertionTesterOne = new MapperTester<IGraphEntity, IThing, IGraphEdge, ILink>();
            mapper.ConvertSinkSource();
            convertionTesterOne.ProveConversion(graphPair.Sink, graphPair.Source, mapper.Get);

            var newEntity = new GraphEntity<string>("new");
            graphPair.Add(newEntity);
            convertionTesterOne.ProveConversion(graphPair.Sink, graphPair.Source, mapper.Get);

        }

        [Test]
        public void TestGraphPairWithFactoryGraph() {
            TestGraphPair(new BinaryTreeFactory(),new ThingGraph ());
            TestGraphPair(new BinaryGraphFactory(), new ThingGraph());
            ReportSummary();
        }


    }

    public class GraphPairTest : DomainTest {

        public IGraphPair<IGraphEntity, IThing, IGraphEdge, ILink> MakeGraphPair(
            ITestGraphFactory<IGraphEntity, IGraphEdge> source, ThingGraph target) {

            source.Count = 10;
            source.Populate();

            var mapper =
                new GraphMapper<IGraphEntity, IThing, IGraphEdge, ILink>(
                    source.Graph, 
                    target, 
                    new GraphItem2ThingTransformer());

            mapper.ConvertSinkSource ();

            var graphPair =
                new LiveGraphPair<IGraphEntity, IThing, IGraphEdge, ILink>(
                    new Graph<IGraphEntity, IGraphEdge>(), 
                    target, 
                    new GraphItem2ThingTransformer());

            return graphPair;

        }

        [Test]
        public void TestGraphPairGetSource() {
            var data =
                MakeGraphPair (new BinaryGraphFactory (), new ThingGraph ());

            var view = new Graph<IGraphEntity, IGraphEdge>();
            var graphView = new SubGraph<IGraphEntity, IGraphEdge>(data, view);

            var graphView2 = new SubGraph<IGraphEntity, IGraphEdge>(graphView, view);

            var result = graphView.RootSource();
            Assert.AreSame(graphView, result);

            result = graphView2.RootSource();
            Assert.AreSame(graphView, result);

            var result2 = graphView.Source<IGraphEntity, IGraphEdge,IThing, ILink>();
            
            Assert.AreSame(data, result2);

            result2 = graphView2.Source<IGraphEntity, IGraphEdge, IThing, ILink>();
            

            Assert.AreSame(data, result2);
        }

        [Test]
        public void TestGraphPairPingBack() {
            
            var data =
                MakeGraphPair (new BinaryGraphFactory (), new ThingGraph ());

            var graphView1 = new SubGraph<IGraphEntity, IGraphEdge> (data, new Graph<IGraphEntity, IGraphEdge> ());
            var graphView2 = new SubGraph<IGraphEntity, IGraphEdge>(data, new Graph<IGraphEntity, IGraphEdge>());

            

            foreach (var ping in graphView1.Source) {
                var back = graphView1.LookUp<IGraphEntity, IGraphEdge,IThing, ILink>(graphView2, ping);
                Assert.IsNotNull (back);
                Assert.AreSame (ping, back);
            }

            data = new LiveGraphPair<IGraphEntity, IThing, IGraphEdge, ILink> (
                new Graph<IGraphEntity, IGraphEdge>(), 
                data.Source,new GraphItem2ThingTransformer());

            graphView2 = new SubGraph<IGraphEntity, IGraphEdge>(data, new Graph<IGraphEntity, IGraphEdge>());
            
            foreach (var ping in graphView1.Source) {
                var back = graphView1.LookUp<IGraphEntity, IGraphEdge,IThing, ILink>(graphView2, ping);
                Assert.IsNotNull(back);
                Assert.AreNotSame(ping, back);
                Assert.AreEqual (ping.ToString(), back.ToString());
            }

        }

        [Test]
        public void TestGraphRootSource () {

            var data =
                MakeGraphPair (new BinaryGraphFactory(), new ThingGraph());

            IGraph<IGraphEntity, IGraphEdge> graph = new SubGraph<IGraphEntity, IGraphEdge> (data, new Graph<IGraphEntity, IGraphEdge>());

            var args = graph.RootSink().GraphPairTypes();
            if (args != null) {
                ReportDetail ("IGraphPair<{0},{1},{2},{3}>", args[0],args[1],args[2],args[3]);
            }
   
            
        }
    }

}