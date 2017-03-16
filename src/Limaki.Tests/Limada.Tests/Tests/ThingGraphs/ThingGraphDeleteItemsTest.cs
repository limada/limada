using System;
using System.Collections.Generic;
using Limada.Model;
using Limada.Tests.Model;
using Limaki.Data;
using Limaki.Graphs;
using Limada.View.VisualThings;
using Limaki.Tests.Graph.Model;
using Limaki.View.Visuals;
using NUnit.Framework;
using System.Linq;

namespace Limada.Tests.ThingGraphs {

    public class ThingGraphDeleteItemsTest : ThingGraphTestBase {

        public struct TestItem {
            public Int64 id;
            public IThing thing;
            public IVisual one;
            public IVisual two;
            public TestItem(Int64 id, IThing thing, IVisual one, IVisual two) {
                this.id = id;
                this.thing = thing;
                this.one = one;
                this.two = two;
            }
        }

        public void IsRemoved<TItem, TEdge>(IGraph<TItem, TEdge> dataSource, TItem root, TItem removed)
        where TEdge : IEdge<TItem>, TItem {

            foreach (var level in dataSource.Walk().DeepWalk(root, 0)) {
                var item = level.Node;
                Assert.AreNotEqual(item, removed);
                if (item is TEdge) {
                    var link = (TEdge)item;
                    Assert.AreNotEqual(link.Root, removed);
                    Assert.AreNotEqual(link.Leaf, removed);
                }
            }
            Assert.IsFalse (dataSource.Contains (removed));
            foreach (var edge in dataSource.Edges(removed))
                Assert.Fail ();
            foreach (var edge in dataSource.Twig(removed))
                Assert.Fail();
        }

        public void IsRemoved<TItem, TEdge>(IGraph<TItem, TEdge> dataSource, IEnumerable<TEdge> removed)
        where TEdge : IEdge<TItem>, TItem {
            foreach (var item in removed)
                Assert.IsFalse(dataSource.Contains(item));
        }

        public void IsRemoved<TItem, TEdge>(IGraph<TItem, TEdge> dataSource, IEnumerable<TEdge> edges, TItem removed)
        where TEdge : IEdge<TItem>, TItem {
            foreach (var edge in edges) {
                Assert.AreNotEqual(edge.Root, removed);
                Assert.AreNotEqual(edge.Leaf, removed);
            }
        }

        /// <summary>
        /// expand dataSource to fill db4o.Graph.edgesCache
        /// error is in: db4o.Graph.Remove(TItem item)
        /// the edgecaches are not deleted properly
        /// to get the error: comment out RemoveInternal(TEdge)
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TEdge"></typeparam>
        /// <param name="dataSource"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        public int Expand<TItem, TEdge>(IGraph<TItem, TEdge> dataSource, TItem root)
        where TEdge : IEdge<TItem>, TItem {

            int result = 0;
            foreach (var level in dataSource.Walk().DeepWalk(root, 0)) {
                result += level.Level; // do something here
            }
            return result;
        }

        public void Remove<TItem, TEdge>(IGraph<TItem, TEdge> dataSource, TItem removed)
        where TEdge : IEdge<TItem>, TItem {
            var deleteInPairOne = dataSource.PostorderTwig (removed).ToArray ();
            foreach (var linkOne in deleteInPairOne) {
                dataSource.Remove(linkOne);
            }
            dataSource.Remove(removed);
        }


        [Test]
        public void ProgramminglanguageJavaDeleteTestPingBack() {
            ReportDetail ("ProgramminglanguageJavaDeleteTestPingBack");
            var dataSource = this.Graph;

           var factory =
                new ThingEntityGraphFactory<EntityProgrammingLanguageFactory> ();

            factory.Populate(dataSource);

            var java = new TestItem(factory.Nodes[3].Id, null, null, null);// Java
            var list = new TestItem(factory.Nodes[7].Id, null, null, null);// List
            var programming = new TestItem(factory.Nodes[1].Id, null, null, null);// Programming
            var programming2Language = new TestItem(factory.Edges[1].Id, null, null, null);// Programming->Language


            this.Close();
            dataSource = this.Graph;

            java.thing = dataSource.GetById(java.id);
            list.thing = dataSource.GetById(list.id);
            programming2Language.thing = dataSource.GetById(programming2Language.id);
            programming.thing = dataSource.GetById(programming.id);

            var pairOne = new VisualThingGraph(new VisualGraph(), dataSource);

            var viewOne = new SubGraph<IVisual, IVisualEdge>(pairOne, new VisualGraph());

            programming.one = pairOne.Get(programming.thing); // Programming

            // expand viewOne:
            foreach (var item in pairOne.Walk().DeepWalk(programming.one, 0)) {
                viewOne.Sink.Add(item.Node);
            }

            var pairOneTwigs = pairOne.Twig(java.one);
            var dataSourceTwigs = dataSource.Twig(java.thing);

            var pairTwo = new VisualThingGraph(new VisualGraph(), dataSource);

            var viewTwo = new SubGraph<IVisual, IVisualEdge>(pairTwo, new VisualGraph());

            java.one = pairOne.Get(java.thing);
            java.two = pairTwo.Get(java.thing);
            programming.two = pairTwo.Get(programming.thing);

            // delete over PingBack in both views:
            

            var deleteCollection = new List<IVisualEdge>(viewOne.PostorderTwig(java.one));

            foreach (var linkOne in deleteCollection) {// Java
                var linkTwo = viewOne.LookUp<IVisual, IVisualEdge, IThing, ILink>(viewTwo, linkOne);
                viewTwo.Remove(linkTwo);
                viewOne.Remove(linkOne);
            }

            java.two = viewOne.LookUp<IVisual, IVisualEdge, IThing, ILink>(viewTwo, java.one);
            viewTwo.Remove(java.two);
            viewOne.Remove(java.one);

            // testing the dataSource:
            IsRemoved<IThing, ILink>(dataSource, programming.thing, java.thing);

            var edges = dataSource.Edges(programming2Language.thing);
            foreach (var link in edges) {
                Assert.AreNotEqual(link.Root, java.thing);
                Assert.AreNotEqual(link.Leaf, java.thing);
            }

            IsRemoved<IThing, ILink>(dataSource, dataSourceTwigs);

            // testing pairOne
            IsRemoved<IVisual, IVisualEdge>(pairOne, programming.one, java.one);
            IsRemoved<IVisual, IVisualEdge>(pairOne, pairOneTwigs);

            // testing pairTwo
            IsRemoved<IVisual, IVisualEdge>(pairTwo, programming.two, java.two);

        }


        [Test]
        public void ProgramminglanguageJavaDeleteTest() {
            ReportDetail ("ProgramminglanguageJavaDeleteTest");
            var dataSource = this.Graph;

            var factory =
                new ThingEntityGraphFactory<EntityProgrammingLanguageFactory>();

            factory.Populate(dataSource);

            var java = new TestItem(factory.Nodes[3].Id, null, null, null);// Java
            var programming = new TestItem(factory.Nodes[1].Id, null, null, null);// Programming
            var programming2Language = new TestItem(factory.Edges[1].Id, null, null, null);// Programming->Language

            this.Close();
            dataSource = this.Graph;

            java.thing = dataSource.GetById(java.id);
            programming.thing = dataSource.GetById(programming.id);
            programming2Language.thing = dataSource.GetById(programming2Language.id);

            Expand<IThing, ILink>(dataSource, programming.thing);
            
            var dataSourceTwigs = dataSource.Twig (java.thing);

            Remove<IThing, ILink>(dataSource, java.thing);

            IsRemoved<IThing, ILink>(dataSource, programming.thing, java.thing);
            IsRemoved<IThing, ILink>(dataSource, dataSourceTwigs);
            IsRemoved<IThing, ILink> (dataSource, dataSource.Twig (programming2Language.thing), java.thing);
        }

        [Test]
        public void ProgramminglanguageJavaDeleteOverPair() {
            ReportDetail("ProgramminglanguageJavaDeleteOverPair");
            var dataSource = this.Graph;
            var factory =
                new ThingEntityGraphFactory<EntityProgrammingLanguageFactory>();

            factory.Populate(dataSource);

            var java = new TestItem(factory.Nodes[3].Id, null, null, null);// Java
            var programming = new TestItem(factory.Nodes[1].Id, null, null, null);// Programming
            var programming2Language = new TestItem(factory.Edges[1].Id, null, null, null);// Programming->Language

            this.Close();
            dataSource = this.Graph;

            java.thing = dataSource.GetById(java.id);
            programming.thing = dataSource.GetById(programming.id);
            programming2Language.thing = dataSource.GetById (programming2Language.id);

            var pairOne = new VisualThingGraph(new VisualGraph(), dataSource);

            programming.one = pairOne.Get(programming.thing);

            Expand<IVisual, IVisualEdge>(pairOne, programming.one);
            var pairOneTwigs = pairOne.Twig (java.one);
            var dataSourceTwigs = dataSource.Twig (java.thing);

            java.one = pairOne.Get(java.thing);
            programming2Language.one = pairOne.Get (programming2Language.thing);

            Remove<IVisual, IVisualEdge>(pairOne, java.one);

            IsRemoved<IVisual, IVisualEdge>(pairOne, programming.one, java.one);
            IsRemoved<IVisual, IVisualEdge>(pairOne, pairOneTwigs);
            IsRemoved<IVisual, IVisualEdge>(pairOne, pairOne.Twig(programming2Language.one),java.one);

            IsRemoved<IThing, ILink>(dataSource, programming.thing, java.thing);
            IsRemoved<IThing, ILink>(dataSource, dataSourceTwigs);
            IsRemoved<IThing, ILink>(dataSource, dataSource.Twig(programming2Language.thing), java.thing);

        }
    }
}