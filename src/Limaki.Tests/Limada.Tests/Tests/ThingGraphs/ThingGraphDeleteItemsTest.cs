using System;
using System.Collections.Generic;
using Limada.Model;
using Limada.Tests.Model;
using Limaki.Data;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limada.View;
using Limada.VisualThings;
using Limaki.Tests.Graph.Model;
using Limaki.Visuals;
using NUnit.Framework;


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
            Walker<TItem, TEdge> walker = new Walker<TItem, TEdge>(dataSource);
            foreach (LevelItem<TItem> level in walker.DeepWalk(root, 0)) {
                TItem item = level.Node;
                Assert.AreNotEqual(item, removed);
                if (item is TEdge) {
                    TEdge link = (TEdge)item;
                    Assert.AreNotEqual(link.Root, removed);
                    Assert.AreNotEqual(link.Leaf, removed);
                }
            }
            Assert.IsFalse (dataSource.Contains (removed));
            foreach (TEdge edge in dataSource.Edges(removed))
                Assert.Fail ();
            foreach (TEdge edge in dataSource.Twig(removed))
                Assert.Fail();
        }

        public void IsRemoved<TItem, TEdge>(IGraph<TItem, TEdge> dataSource, IEnumerable<TEdge> removed)
        where TEdge : IEdge<TItem>, TItem {
            foreach (TEdge item in removed)
                Assert.IsFalse(dataSource.Contains(item));
        }

        public void IsRemoved<TItem, TEdge>(IGraph<TItem, TEdge> dataSource, IEnumerable<TEdge> edges, TItem removed)
        where TEdge : IEdge<TItem>, TItem {
            foreach (TEdge edge in edges) {
                Assert.AreNotEqual(edge.Root, removed);
                Assert.AreNotEqual(edge.Leaf, removed);
            }
        }

        public ICollection<TEdge> GetTwigCollection<TItem, TEdge>(IGraph<TItem, TEdge> dataSource, TItem root)
        where TEdge : IEdge<TItem>, TItem {
            return new List<TEdge> (dataSource.Twig (root));
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
            Walker<TItem, TEdge> walker = new Walker<TItem, TEdge>(dataSource);
            foreach (LevelItem<TItem> level in walker.DeepWalk(root, 0)) {
                result += level.Level; // do something here
            }
            return result;
        }

        public void Remove<TItem, TEdge>(IGraph<TItem, TEdge> dataSource, TItem removed)
        where TEdge : IEdge<TItem>, TItem {
            ICollection<TEdge> deleteInPairOne = new List<TEdge>(dataSource.PostorderTwig(removed));
            foreach (TItem linkOne in deleteInPairOne) {
                dataSource.Remove(linkOne);
            }
            dataSource.Remove(removed);
        }


        [Test]
        public void ProgramminglanguageJavaDeleteTestPingBack() {
            ReportDetail ("ProgramminglanguageJavaDeleteTestPingBack");
            IThingGraph dataSource = this.Graph;

            ThingGraphFactory<ProgrammingLanguageFactory> factory =
                new ThingGraphFactory<ProgrammingLanguageFactory>();

            factory.Populate(dataSource);

            TestItem java = new TestItem(factory.Nodes[3].Id, null, null, null);// Java
            TestItem list = new TestItem(factory.Nodes[7].Id, null, null, null);// List
            TestItem programming = new TestItem(factory.Nodes[1].Id, null, null, null);// Programming
            TestItem programming2Language = new TestItem(factory.Edges[1].Id, null, null, null);// Programming->Language


            this.Close();
            dataSource = this.Graph;

            java.thing = dataSource.GetById(java.id);
            list.thing = dataSource.GetById(list.id);
            programming2Language.thing = dataSource.GetById(programming2Language.id);
            programming.thing = dataSource.GetById(programming.id);

            IGraphPair<IVisual, IThing, IVisualEdge, ILink> pairOne =
                new VisualThingGraph(new Limaki.Visuals.VisualGraph(), dataSource);

            SubGraph<IVisual, IVisualEdge> viewOne =
                new SubGraph<IVisual, IVisualEdge>(pairOne, new Limaki.Visuals.VisualGraph());

            programming.one = pairOne.Get(programming.thing); // Programming

            // expand viewOne:
            Walker<IVisual, IVisualEdge> walker = new Walker<IVisual, IVisualEdge>(pairOne);
            foreach (LevelItem<IVisual> item in walker.DeepWalk(programming.one, 0)) {
                viewOne.Sink.Add(item.Node);
            }

            ICollection<IVisualEdge> pairOneTwigs = GetTwigCollection<IVisual,IVisualEdge> (pairOne, java.one);
            ICollection<ILink> dataSourceTwigs = GetTwigCollection<IThing,ILink>(dataSource, java.thing);

            IGraphPair<IVisual, IThing, IVisualEdge, ILink> pairTwo =
                new VisualThingGraph(new Limaki.Visuals.VisualGraph(), dataSource);

            SubGraph<IVisual, IVisualEdge> viewTwo =
                new SubGraph<IVisual, IVisualEdge>(pairTwo, new Limaki.Visuals.VisualGraph());

            java.one = pairOne.Get(java.thing);
            java.two = pairTwo.Get(java.thing);
            programming.two = pairTwo.Get(programming.thing);

            // delete over PingBack in both views:
            

            ICollection<IVisualEdge> deleteCollection =
                new List<IVisualEdge>(viewOne.PostorderTwig(java.one));

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

            ICollection<ILink> edges = dataSource.Edges(programming2Language.thing);
            foreach (ILink link in edges) {
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
            IThingGraph dataSource = this.Graph;

            ThingGraphFactory<ProgrammingLanguageFactory> factory =
                new ThingGraphFactory<ProgrammingLanguageFactory>();

            factory.Populate(dataSource);

            TestItem java = new TestItem(factory.Nodes[3].Id, null, null, null);// Java
            TestItem programming = new TestItem(factory.Nodes[1].Id, null, null, null);// Programming
            TestItem programming2Language = new TestItem(factory.Edges[1].Id, null, null, null);// Programming->Language

            this.Close();
            dataSource = this.Graph;

            java.thing = dataSource.GetById(java.id);
            programming.thing = dataSource.GetById(programming.id);
            programming2Language.thing = dataSource.GetById(programming2Language.id);

            Expand<IThing, ILink>(dataSource, programming.thing);
            ICollection<ILink> dataSourceTwigs = GetTwigCollection<IThing, ILink>(dataSource, java.thing);

            Remove<IThing, ILink>(dataSource, java.thing);

            IsRemoved<IThing, ILink>(dataSource, programming.thing, java.thing);
            IsRemoved<IThing, ILink>(dataSource, dataSourceTwigs);
            IsRemoved<IThing, ILink> (dataSource, dataSource.Twig (programming2Language.thing), java.thing);
        }

        [Test]
        public void ProgramminglanguageJavaDeleteOverPair() {
            ReportDetail("ProgramminglanguageJavaDeleteOverPair");
            var dataSource = this.Graph;
            ThingGraphFactory<ProgrammingLanguageFactory> factory =
                new ThingGraphFactory<ProgrammingLanguageFactory>();

            factory.Populate(dataSource);

            TestItem java = new TestItem(factory.Nodes[3].Id, null, null, null);// Java
            TestItem programming = new TestItem(factory.Nodes[1].Id, null, null, null);// Programming
            TestItem programming2Language = new TestItem(factory.Edges[1].Id, null, null, null);// Programming->Language

            this.Close();
            dataSource = this.Graph;

            java.thing = dataSource.GetById(java.id);
            programming.thing = dataSource.GetById(programming.id);
            programming2Language.thing = dataSource.GetById (programming2Language.id);

            IGraphPair<IVisual, IThing, IVisualEdge, ILink> pairOne =
                new VisualThingGraph(new Limaki.Visuals.VisualGraph(), dataSource);

            programming.one = pairOne.Get(programming.thing);

            Expand<IVisual, IVisualEdge>(pairOne, programming.one);
            ICollection<IVisualEdge> pairOneTwigs = GetTwigCollection<IVisual, IVisualEdge>(pairOne, java.one);
            ICollection<ILink> dataSourceTwigs = GetTwigCollection<IThing, ILink>(dataSource, java.thing);

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