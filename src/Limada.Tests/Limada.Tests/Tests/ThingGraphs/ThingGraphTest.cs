/*
 * Limada 
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


using System.Collections.Generic;
using Limada.Model;
using Limada.Tests.Model;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using NUnit.Framework;
using BasicThingGraphTest=Limada.Tests.Basic.BasicThingGraphTest;
using Id = System.Int64;


namespace Limada.Tests.ThingGraphs {

    public class ThingGraphTest : ThingGraphTestBase {

        public void AddData(IThingGraph graph, BasicThingGraphTest graphTest) {
            graphTest.Graph = graph;
            graphTest.Setup();
            graphTest.FillGraph(graph, graphTest.Data);
        }

        /// <summary>
        /// Testing ThingGraphTest 
        /// </summary>
        [Test]
        public virtual void StandardGraphTest() {
            var graphTest = new BasicThingGraphTest();
            graphTest.DoDetail = this.DoDetail;

            graphTest.Graph = this.Graph;
            graphTest.Setup();
            graphTest.AllTests();
            graphTest.TearDown();

            this.Close();
            ReportSummary();
        }

        /// <summary>
        /// Testing Open a Limada.Data.db4o.ThingGraph, close it, read in again
        /// also tests activation of Link.Leaf and Link.Root
        /// </summary>
        [Test]
        public virtual void OpenCloseOpenRead() {
            var graphTest = new BasicThingGraphTest();

            var graph = this.Graph;
            AddData(graph, graphTest);

            this.Close();
            graph = this.Graph;

            var thing = graph.GetById(graphTest.Data.Two.Id);
            Assert.IsNotNull (thing);
            Assert.AreEqual(thing.Id, graphTest.Data.Two.Id);

            var links = graph.Edges(graphTest.Data.Two);
            foreach (var l in links) {
                Assert.IsNotNull(l.Root);
                Assert.IsNotNull(l.Leaf);
                Assert.IsNotNull(l.Marker);
            }

            thing = graph.GetById(graphTest.Data.TwoThree.Id);
            var link = (ILink)thing;
            Assert.AreEqual(link.Root.Id, graphTest.Data.Two.Id);

            this.Close();
            ReportSummary();
        }

        [Test]
        public virtual void CheckInvalidLinks() {
            var graph = this.Graph;
            foreach (var link in graph.Edges()) {
                if (((ILink<Id>)link).Leaf != default(Id)) {
                    Assert.IsNotNull(link.Leaf);
                }
                if (((ILink<Id>)link).Marker != default(Id)) {
                    Assert.IsNotNull(link.Marker);
                }
                if (((ILink<Id>)link).Root != default(Id)) {
                    Assert.IsNotNull(link.Root);
                }
            }
            this.Close();
            ReportSummary();
        }

        public int RepeatCount = 150;
        [Test]
        public virtual void StorePerformanceTest() {
            var target = this.Graph;
            var graphPair = new HollowGraphPair<IGraphEntity, IThing, IGraphEdge, ILink>(
                      new Limaki.Graphs.Graph<IGraphEntity, IGraphEdge>(), 
                      target, 
                      new GraphEntity2ThingTransformer());

            var factory = new EntityBinaryGraphFactory();
            factory.Graph = graphPair;
            factory.Count = RepeatCount;
            factory.AddDensity = true;

            this.Tickers.Start();
            factory.Populate();
            ReportSummary(factory.Name + "\t" + (factory.Graph.Count));
            this.OnFlush (target);

            ReportSummary();
            this.Close();
        }

        [Test]
        public virtual void EdgeListTest() {
            var target = this.Graph;

            //IGraphPair<IGraphItem, IThing, IGraphEdge, ILink> graphPair =
            //      new HollowGraphPair<IGraphItem, IThing, IGraphEdge, ILink>(
            //          new Limaki.Graphs.Graph<IGraphItem, IGraphEdge>(), target, new GraphItem2ThingMapper());

            //GraphFactoryBase factory = new BinaryGraphFactory();
            //factory.Graph = graphPair;
            //factory.Count = 1;
            //factory.AddDensity = true;
            //factory.Populate();

            var edgesCount = new Dictionary<IThing, int>();
            foreach (var thing in target) {
                int i = 0;
                foreach (var link in target.Edges(thing)) {
                    i++;
                }
                edgesCount.Add(thing, i);
            }
            for (int j = 0; j < 3; j++)
                foreach (var thing in target) {
                    int i = 0;
                    foreach (var link in target.Edges(thing)) {
                        i++;
                    }
                    Assert.AreEqual(edgesCount[thing], i);
                }

            ReportSummary();
            this.Close();
        }

        [Test]
        public virtual void ProgramminglanguageJavaDeleteTest2() {
            var target = this.Graph;

            var factory = new ThingEntityGraphFactory<EntityProgrammingLanguageFactory>();

            factory.Populate(target);

            var testThing = factory.Nodes[3];// Java
            var testThing2 = factory.Nodes[7]; // List
            var testThing3 = factory.Edges[1]; // Programming->Language
            var testThing4 = factory.Nodes[1]; // Programming

            var pair = new HollowGraphPair<IGraphEntity, IThing, IGraphEdge, ILink> (
                new Limaki.Graphs.Graph<IGraphEntity, IGraphEdge> (),
                target,
                new GraphEntity2ThingTransformer ()
                );

            pair.Mapper.ConvertSourceSink();

            var testEntity = pair.Get(testThing);
            var testItem4 = pair.Get(testThing4);

            Assert.IsTrue(target.Contains(testThing));

            var deleteCollection =
                new List<IGraphEdge>(pair.PostorderTwig(testEntity));

            foreach (var link in deleteCollection) {// Java
                pair.Remove(link);
            }

            pair.Remove(testEntity); // Java


            foreach (var link in deleteCollection) {// Java
                pair.Remove(link);
            }

            pair.Remove(testEntity); // Java

            foreach (var item in pair.Source.Walk().DeepWalk(testThing4, 0)) {
                var thing = item.Node;
                Assert.AreNotEqual(thing, testThing);
                if (thing is ILink) {
                    var link = (ILink)thing;
                    Assert.AreNotEqual(link.Root, testThing);
                    Assert.AreNotEqual(link.Leaf, testThing);
                }
            }


            foreach (var item in pair.Walk().DeepWalk(testItem4, 0)) {
                var thing = item.Node;
                Assert.AreNotEqual(thing, testEntity);
                if (thing is IGraphEdge) {
                    var link = (IGraphEdge)thing;
                    Assert.AreNotEqual(link.Root, testEntity);
                    Assert.AreNotEqual(link.Leaf, testEntity);
                }
            }
            ReportSummary();
        }

    }
}

