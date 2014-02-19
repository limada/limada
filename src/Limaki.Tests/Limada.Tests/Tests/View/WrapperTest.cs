using System.Diagnostics;
using Limada.Model;
using Limada.Tests.Model;
using Limada.VisualThings;
using Limaki.Model;
using Limaki.Tests;
using Limaki.Tests.Graph;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Graph.Wrappers;
using Limaki.Visuals;
using NUnit.Framework;

namespace Limada.Tests.Wrappers {

    public class WrapperTest : DomainTest {

        public void GenericBiGraphFactory () {
            IThingGraph thingGraph = new ThingGraph ();

            TestGraphPairFactory<IThing, IGraphEntity, ILink, IGraphEdge> factory =
                new TestGraphPairFactory<IThing, IGraphEntity, ILink, IGraphEdge> (
                    new ProgrammingLanguageFactory (),
                    new GraphItem2ThingTransformer ()
                    );

            factory.Mapper.Source = thingGraph;
            factory.Populate (thingGraph);

            Trace.Write (
                GraphTestUtils.ReportGraph<IThing, ILink> (thingGraph, factory.Name));
        }

        [Test]
        public void ThingGraphFactory () {
            var graph = new VisualThingGraph ();

            var factory = new ThingGraphFactory<ProgrammingLanguageFactory> ();
            factory.Populate (graph.Source);
            Trace.Write (
                GraphTestUtils.ReportGraph<IThing, ILink> (graph.Source, factory.Name));
        }

        [Test]
        public void FoldingTest1 () {
            var graph = new VisualThingGraph ();

            var test = new ProgrammingGraphSceneTest ();
            test.Mock.Factory.Graph = graph;
            test.Net ();

        }

        [Test]
        public void Mock2 () {
            var graph = new VisualThingGraph ();

            var test = new ProgrammingGraphSceneTest ();
            test.Mock.Factory.Populate (graph);
            test.Mock.Factory.Graph = graph;
            var scene = new Scene ();
            scene.Graph = graph;
            test.Mock.Scene = scene;


            test.Net ();

        }

        [Test]
        public void FoldingExpandTest () {
            var graph = new VisualThingGraph ();

            var test = new ProgrammingGraphSceneTest ();
            test.Mock.Factory.Graph = graph;
            test.Mock.Scene = new Scene ();
            test.Mock.Scene.Graph = graph;

            var factory = new ThingGraphFactory<ProgrammingLanguageFactory> ();
            factory.Count = 10;
            factory.Populate (graph.Source);


            test.Mock.SetFocused (graph.Get (factory.Nodes[1]));
            Trace.Write (
                GraphTestUtils.ReportGraph<IVisual, IVisualEdge> (graph.Sink, factory.Name));

            test.Mock.SceneFacade.Expand (false);

            Trace.Write (
                GraphTestUtils.ReportGraph<IVisual, IVisualEdge> (graph.Sink, factory.Name));

        }
    }
}