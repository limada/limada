using System;
using System.Collections.Generic;
using Limada.Model;
using Limada.Tests.Model;
using Limada.View;
using Limaki.Data;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Tests.Graph.Model;
using Limaki.Widgets;
using Limaki.Widgets.Layout;
using NUnit.Framework;
using ThingGraph = Limada.Data.db4o.ThingGraph;

namespace Limada.Tests.Data.db4o {
    public class ThingGraphDeleteItemsTest : ThingGraphTestBase {
        public struct TestItem {
            public Int64 id;
            public IThing thing;
            public IWidget one;
            public IWidget two;
            public TestItem(Int64 id, IThing thing, IWidget one, IWidget two) {
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

            IThingGraph dataSource = this.Graph;

            ThingGraphFactory<ProgrammingLanguageFactory> factory =
                new ThingGraphFactory<ProgrammingLanguageFactory>();

            factory.Populate(dataSource);

            TestItem java = new TestItem(factory.Node[3].Id, null, null, null);// Java
            TestItem list = new TestItem(factory.Node[7].Id, null, null, null);// List
            TestItem programming = new TestItem(factory.Node[1].Id, null, null, null);// Programming
            TestItem programming2Language = new TestItem(factory.Edge[1].Id, null, null, null);// Programming->Language


            Gateway.Close();
            Gateway.Open(DataBaseInfo.FromFileName(FileName));
            dataSource = new ThingGraph(Gateway);

            java.thing = dataSource.GetById(java.id);
            list.thing = dataSource.GetById(list.id);
            programming2Language.thing = dataSource.GetById(programming2Language.id);
            programming.thing = dataSource.GetById(programming.id);

            IGraphPair<IWidget, IThing, IEdgeWidget, ILink> pairOne =
                new WidgetThingGraph(new Limaki.Widgets.WidgetGraph(), dataSource);

            GraphView<IWidget, IEdgeWidget> viewOne =
                new GraphView<IWidget, IEdgeWidget>(pairOne, new Limaki.Widgets.WidgetGraph());

            programming.one = pairOne.Get(programming.thing); // Programming

            // expand viewOne:
            Walker<IWidget, IEdgeWidget> walker = new Walker<IWidget, IEdgeWidget>(pairOne);
            foreach (LevelItem<IWidget> item in walker.DeepWalk(programming.one, 0)) {
                viewOne.One.Add(item.Node);
            }

            ICollection<IEdgeWidget> pairOneTwigs = GetTwigCollection<IWidget,IEdgeWidget> (pairOne, java.one);
            ICollection<ILink> dataSourceTwigs = GetTwigCollection<IThing,ILink>(dataSource, java.thing);

            IGraphPair<IWidget, IThing, IEdgeWidget, ILink> pairTwo =
                new WidgetThingGraph(new Limaki.Widgets.WidgetGraph(), dataSource);

            GraphView<IWidget, IEdgeWidget> viewTwo =
                new GraphView<IWidget, IEdgeWidget>(pairTwo, new Limaki.Widgets.WidgetGraph());

            java.one = pairOne.Get(java.thing);
            java.two = pairTwo.Get(java.thing);
            programming.two = pairTwo.Get(programming.thing);

            // delete over PingBack in both views:
            GraphPairFacade<IWidget, IEdgeWidget> facade = new GraphPairFacade<IWidget, IEdgeWidget>();

            ICollection<IEdgeWidget> deleteCollection =
                new List<IEdgeWidget>(viewOne.PostorderTwig(java.one));

            foreach (IWidget linkOne in deleteCollection) {// Java
                IWidget linkTwo = facade.LookUp<IThing, ILink>(viewOne, viewTwo, linkOne);
                viewTwo.Remove(linkTwo);
                viewOne.Remove(linkOne);
            }

            java.two = facade.LookUp<IThing, ILink>(viewOne, viewTwo, java.one);
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
            IsRemoved<IWidget, IEdgeWidget>(pairOne, programming.one, java.one);
            IsRemoved<IWidget, IEdgeWidget>(pairOne, pairOneTwigs);

            // testing pairTwo
            IsRemoved<IWidget, IEdgeWidget>(pairTwo, programming.two, java.two);

        }


        [Test]
        public void ProgramminglanguageJavaDeleteTest() {
            Gateway.Open(DataBaseInfo.FromFileName(FileName));
            ThingGraph dataSource = new ThingGraph(Gateway);

            ThingGraphFactory<ProgrammingLanguageFactory> factory =
                new ThingGraphFactory<ProgrammingLanguageFactory>();

            factory.Populate(dataSource);

            TestItem java = new TestItem(factory.Node[3].Id, null, null, null);// Java
            TestItem programming = new TestItem(factory.Node[1].Id, null, null, null);// Programming
            TestItem programming2Language = new TestItem(factory.Edge[1].Id, null, null, null);// Programming->Language

            Gateway.Close();
            Gateway.Open(DataBaseInfo.FromFileName(FileName));
            dataSource = new ThingGraph(Gateway);

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
            Gateway.Open(DataBaseInfo.FromFileName(FileName));
            ThingGraph dataSource = new ThingGraph(Gateway);

            ThingGraphFactory<ProgrammingLanguageFactory> factory =
                new ThingGraphFactory<ProgrammingLanguageFactory>();

            factory.Populate(dataSource);

            TestItem java = new TestItem(factory.Node[3].Id, null, null, null);// Java
            TestItem programming = new TestItem(factory.Node[1].Id, null, null, null);// Programming
            TestItem programming2Language = new TestItem(factory.Edge[1].Id, null, null, null);// Programming->Language

            Gateway.Close();
            Gateway.Open(DataBaseInfo.FromFileName(FileName));
            dataSource = new ThingGraph(Gateway);

            java.thing = dataSource.GetById(java.id);
            programming.thing = dataSource.GetById(programming.id);
            programming2Language.thing = dataSource.GetById (programming2Language.id);

            IGraphPair<IWidget, IThing, IEdgeWidget, ILink> pairOne =
                new WidgetThingGraph(new Limaki.Widgets.WidgetGraph(), dataSource);

            programming.one = pairOne.Get(programming.thing);

            Expand<IWidget, IEdgeWidget>(pairOne, programming.one);
            ICollection<IEdgeWidget> pairOneTwigs = GetTwigCollection<IWidget, IEdgeWidget>(pairOne, java.one);
            ICollection<ILink> dataSourceTwigs = GetTwigCollection<IThing, ILink>(dataSource, java.thing);

            java.one = pairOne.Get(java.thing);
            programming2Language.one = pairOne.Get (programming2Language.thing);

            Remove<IWidget, IEdgeWidget>(pairOne, java.one);

            IsRemoved<IWidget, IEdgeWidget>(pairOne, programming.one, java.one);
            IsRemoved<IWidget, IEdgeWidget>(pairOne, pairOneTwigs);
            IsRemoved<IWidget, IEdgeWidget>(pairOne, pairOne.Twig(programming2Language.one),java.one);

            IsRemoved<IThing, ILink>(dataSource, programming.thing, java.thing);
            IsRemoved<IThing, ILink>(dataSource, dataSourceTwigs);
            IsRemoved<IThing, ILink>(dataSource, dataSource.Twig(programming2Language.thing), java.thing);

        }
    }
}