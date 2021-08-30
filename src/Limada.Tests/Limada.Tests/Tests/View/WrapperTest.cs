/*
* Limaki 
* 
* This code is free software; you can redistribute it and/or modify it
* under the terms of the GNU General Public License version 2 only, as
* published by the Free Software Foundation.
* 
* Author: Lytico
* Copyright (C) 2014 Lytico
*
* http://www.limada.org
* 
*/

using Limada.Model;
using Limada.Tests.Model;
using Limada.View.VisualThings;
using Limaki.Model;
using Limaki.Tests.Graph;
using Limaki.Tests.Graph.Model;
using Limaki.View.Visuals;
using NUnit.Framework;
using System.Diagnostics;

namespace Limaki.Tests.View.Visuals {

    public class WrapperTest : DomainTest {

        [Test]
        public void TestGraphPairFactory () {

            IThingGraph thingGraph = new ThingGraph ();

            var factory =
                new SampleGraphPairFactory<IThing, IGraphEntity, ILink, IGraphEdge> (
                    new ProgrammingLanguageFactory<IGraphEntity, IGraphEdge> (),
                    new GraphEntity2ThingTransformer ().Reverted()
                    );

            factory.Mapper.Sink = thingGraph;
            factory.Populate (thingGraph);

            Trace.Write (
                GraphTestUtils.ReportGraph<IThing, ILink> (thingGraph, factory.Name));
        }

        [Test]
        public void ThingGraphFactory () {
            var graph = new VisualThingGraph ();

            var factory = new ThingEntityGraphFactory<ProgrammingLanguageFactory<IGraphEntity, IGraphEdge>> ();
            factory.Populate (graph.Source);
            Trace.Write (
                GraphTestUtils.ReportGraph<IThing, ILink> (graph.Source, factory.Name));
        }

        [Test]
        public void FoldingTest1 () {
            var graph = new VisualThingGraph ();

            var test = new EntityProgrammingSceneTest ();
            test.Mock.SampleFactory.Graph = graph;
            test.Net ();

        }

        [Test]
        public void FoldingExpandTest () {
            var graph = new VisualThingGraph ();

            var test = new EntityProgrammingSceneTest ();
            test.Mock.SampleFactory.Graph = graph;
            test.Mock.Scene = new Scene ();
            test.Mock.Scene.Graph = graph;

            var factory = new ThingEntityGraphFactory<ProgrammingLanguageFactory<IGraphEntity, IGraphEdge>> ();
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
