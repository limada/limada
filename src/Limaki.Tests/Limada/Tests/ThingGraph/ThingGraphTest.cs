/*
 * Limada 
 * Version 0.081
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


using System.Collections.Generic;
using Limada.Model;
using Limada.Tests.Model;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
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
        /// Testing ThingGraphTest over Limada.Data.db4o.ThingGraph
        /// </summary>
        [Test]
        public void StandardGraphTest() {
            BasicThingGraphTest graphTest = new BasicThingGraphTest();
            graphTest.DoDetail = this.DoDetail;

            graphTest.Graph = this.Graph;
            graphTest.Setup();
            graphTest.AllTests();
            graphTest.TearDown();

            this.OnClose();
            ReportSummary();
        }

        /// <summary>
        /// Testing Open a Limada.Data.db4o.ThingGraph, close it, read in again
        /// also tests activation of Link.Leaf and Link.Root
        /// </summary>
        [Test]
        public void OpenCloseOpenRead() {
            BasicThingGraphTest graphTest = new BasicThingGraphTest();

            IThingGraph graph = this.Graph;
            AddData(graph, graphTest);

            this.OnClose();
            graph = this.Graph;

            IThing thing = graph.GetById(graphTest.Data.Two.Id);
            Assert.AreEqual(thing.Id, graphTest.Data.Two.Id);

            IEnumerable<ILink> links = graph.Edges(graphTest.Data.Two);
            foreach (ILink l in links) {
                Assert.IsNotNull(l.Root);
                Assert.IsNotNull(l.Leaf);
                Assert.IsNotNull(l.Marker);
            }

            thing = graph.GetById(graphTest.Data.TwoThree.Id);
            ILink link = (ILink)thing;
            Assert.AreEqual(link.Root.Id, graphTest.Data.Two.Id);

            this.OnClose();
            ReportSummary();
        }

        [Test]
        public void CheckInvalidLinks() {
            IThingGraph graph = this.Graph;
            foreach (ILink link in graph.Edges()) {
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
            this.OnClose();
            ReportSummary();
        }

        [Test]
        public void StorePerformanceTest() {
            IThingGraph target = this.Graph;
            IGraphPair<IGraphItem, IThing, IGraphEdge, ILink> graphPair =
                  new GraphPair<IGraphItem, IThing, IGraphEdge, ILink>(
                      new Limaki.Graphs.Graph<IGraphItem, IGraphEdge>(), 
                      target, 
                      new GraphItem2ThingAdapter());

            GraphFactoryBase factory = new BinaryGraphFactory();
            factory.Graph = graphPair;
            factory.Count = 150;
            factory.AddDensity = true;

            this.Tickers.Start();
            factory.Populate();
            ReportSummary(factory.Name + "\t" + (factory.Graph.Count));
            this.OnFlush (target);

            ReportSummary();
            this.OnClose();
        }



        [Test]
        public void EdgeListTest() {
            IThingGraph target = this.Graph;

            //IGraphPair<IGraphItem, IThing, IGraphEdge, ILink> graphPair =
            //      new GraphPair<IGraphItem, IThing, IGraphEdge, ILink>(
            //          new Limaki.Graphs.Graph<IGraphItem, IGraphEdge>(), target, new GraphItem2ThingMapper());

            //GraphFactoryBase factory = new BinaryGraphFactory();
            //factory.Graph = graphPair;
            //factory.Count = 1;
            //factory.AddDensity = true;
            //factory.Populate();

            IDictionary<IThing, int> edgesCount = new Dictionary<IThing, int>();
            foreach (IThing thing in target) {
                int i = 0;
                foreach (ILink link in target.Edges(thing)) {
                    i++;
                }
                edgesCount.Add(thing, i);
            }
            for (int j = 0; j < 3; j++)
                foreach (IThing thing in target) {
                    int i = 0;
                    foreach (ILink link in target.Edges(thing)) {
                        i++;
                    }
                    Assert.AreEqual(edgesCount[thing], i);
                }

            ReportSummary();
            this.OnClose();
        }

        [Test]
        public void ProgramminglanguageJavaDeleteTest2() {
            IThingGraph target = this.Graph;

            ThingGraphFactory<ProgrammingLanguageFactory> factory =
                new ThingGraphFactory<ProgrammingLanguageFactory>();

            factory.Populate(target);

            IThing testThing = factory.Node[3];// Java
            IThing testThing2 = factory.Node[7]; // List
            IThing testThing3 = factory.Edge[1]; // Programming->Language
            IThing testThing4 = factory.Node[1]; // Programming

            IGraphPair<IGraphItem, IThing, IGraphEdge, ILink> pair =
                new GraphPair<IGraphItem, IThing, IGraphEdge, ILink>(
                    new Limaki.Graphs.Graph<IGraphItem, IGraphEdge>(),
                    target,
                    new GraphItem2ThingAdapter()
                    );

            pair.Mapper.ConvertTwoOne();

            IGraphItem testItem = pair.Get(testThing);
            IGraphItem testItem4 = pair.Get(testThing4);

            Assert.IsTrue(target.Contains(testThing));

            ICollection<IGraphEdge> deleteCollection =
                new List<IGraphEdge>(pair.PostorderTwig(testItem));

            foreach (IGraphEdge link in deleteCollection) {// Java
                pair.Remove(link);
            }

            pair.Remove(testItem); // Java


            foreach (IGraphEdge link in deleteCollection) {// Java
                pair.Remove(link);
            }

            pair.Remove(testItem); // Java

            Walker<IThing, ILink> walker1 = new Walker<IThing, ILink>(pair.Two);
            foreach (LevelItem<IThing> item in walker1.DeepWalk(testThing4, 0)) {
                IThing thing = item.Node;
                Assert.AreNotEqual(thing, testThing);
                if (thing is ILink) {
                    ILink link = (ILink)thing;
                    Assert.AreNotEqual(link.Root, testThing);
                    Assert.AreNotEqual(link.Leaf, testThing);
                }
            }

            Walker<IGraphItem, IGraphEdge> walker = new Walker<IGraphItem, IGraphEdge>(pair);
            foreach (LevelItem<IGraphItem> item in walker.DeepWalk(testItem4, 0)) {
                IGraphItem thing = item.Node;
                Assert.AreNotEqual(thing, testItem);
                if (thing is IGraphEdge) {
                    IGraphEdge link = (IGraphEdge)thing;
                    Assert.AreNotEqual(link.Root, testItem);
                    Assert.AreNotEqual(link.Leaf, testItem);
                }
            }
            ReportSummary();
        }

    }
}

