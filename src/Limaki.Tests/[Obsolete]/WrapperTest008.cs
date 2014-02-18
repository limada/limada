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


using System;
using System.Collections.Generic;
using System.Text;
using Limaki.Graphs;
using Limada.View;
using Limada.VisualThings;
using Limaki.Model;
using Limaki.Tests;
using Limaki.UnitTest;
using Limaki.Tests.Graph.Model;
using Limada.Model;
using NUnit.Framework;
using Limaki.Tests.Visuals;
using Limaki.Tests.Graph;
using Limada.Tests.Model;
using Limaki.Visuals;
using Limaki.Tests.Graph.Wrappers;
using System.Diagnostics;


namespace Limada.Tests.Wrappers {
    [Obsolete]
    public class WrapperTest008: DomainTest{
        public void GenericBiGraphFactory() {
            IThingGraph thingGraph = new ThingGraph();

            GenericBiGraphFactory<IThing, IGraphEntity, ILink, IGraphEdge> factory =
                new GenericBiGraphFactory<IThing, IGraphEntity, ILink, IGraphEdge>(
                new ProgrammingLanguageFactory(),
                new GraphItem2ThingTransformer()
                );
            
            factory.Mapper.Source = thingGraph;
            factory.Populate(thingGraph);

            Trace.Write(
                GraphTestUtils.ReportGraph<IThing, ILink>(thingGraph, factory.Name));
        }
        [Test]
        public void ThingGraphFactory() {
            var graph = new VisualThingGraph ();

            var factory = new ThingGraphFactory<ProgrammingLanguageFactory> ();
            factory.Populate (graph.Source);
            Trace.Write (
                GraphTestUtils.ReportGraph<IThing, ILink> (graph.Source, factory.Name));
        }

        [Test]
        public void FoldingTest1() {
            var graph = new VisualThingGraph();

            var test = new ProgrammingLanguageFoldingTest008();
            test.Mock.Factory.Graph = graph;
            test.Net ();

        }

        [Test]
        public void Mock2() {
            var graph = new VisualThingGraph();

            var test = new ProgrammingLanguageFoldingTest008();
            test.Mock.Factory.Populate (graph);
            test.Mock.Factory.Graph = graph;
            var scene = new Scene();
            scene.Graph = graph;
            test.Mock.Scene = scene;
            

            test.Net();

        }

        [Test]
        public void FoldingExpandTest() {
            var graph = new VisualThingGraph();

            var test = new ProgrammingLanguageFoldingTest008();
            test.Mock.Factory.Graph = graph;
            test.Mock.Scene = new Scene();
            test.Mock.Scene.Graph = graph;

            var factory = new ThingGraphFactory<ProgrammingLanguageFactory>();
            factory.Count = 10;
            factory.Populate(graph.Source);


            test.Mock.Scene.Focused = graph.Get (factory.Nodes[1]);
            Trace.Write(
                    GraphTestUtils.ReportGraph<IVisual, IVisualEdge>(graph.Sink, factory.Name));

            test.Mock.SceneFacade.Expand (false);

            Trace.Write(
                GraphTestUtils.ReportGraph<IVisual, IVisualEdge>(graph.Sink, factory.Name));

        }
    }
}
