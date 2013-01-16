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


namespace Limada.Tests.Wrappers {
    public class WrapperTest: DomainTest{
        public void GenericBiGraphFactory() {
            IThingGraph thingGraph = new ThingGraph();

            GenericBiGraphFactory<IThing, IGraphItem, ILink, IGraphEdge> factory =
                new GenericBiGraphFactory<IThing, IGraphItem, ILink, IGraphEdge>(
                new ProgrammingLanguageFactory(),
                new GraphItem2ThingAdapter()
                );
            
            factory.Mapper.Two = thingGraph;
            factory.Populate(thingGraph);

            Console.Out.Write(
                GraphTestUtils.ReportGraph<IThing, ILink>(thingGraph, factory.Name));
        }
        [Test]
        public void ThingGraphFactory() {
            VisualThingGraph graph = new VisualThingGraph ();

            ThingGraphFactory<ProgrammingLanguageFactory> factory = new ThingGraphFactory<ProgrammingLanguageFactory> ();
            factory.Populate (graph.Two);
            Console.Out.Write (
                GraphTestUtils.ReportGraph<IThing, ILink> (graph.Two, factory.Name));
        }

        [Test]
        public void FoldingTest1() {
            VisualThingGraph graph = new VisualThingGraph();

            ProgrammingLanguageFoldingTest test = new ProgrammingLanguageFoldingTest();
            test.Mock.Factory.Graph = graph;
            test.Net ();

        }

        [Test]
        public void Mock2() {
            VisualThingGraph graph = new VisualThingGraph();

            var test = new ProgrammingLanguageFoldingTest();
            test.Mock.Factory.Populate (graph);
            test.Mock.Factory.Graph = graph;
            var scene = new Scene();
            scene.Graph = graph;
            test.Mock.Scene = scene;
            

            test.Net();

        }

        [Test]
        public void FoldingExpandTest() {
            VisualThingGraph graph = new VisualThingGraph();

            ProgrammingLanguageFoldingTest test = new ProgrammingLanguageFoldingTest();
            test.Mock.Factory.Graph = graph;
            test.Mock.Scene = new Scene();
            test.Mock.Scene.Graph = graph;

            ThingGraphFactory<ProgrammingLanguageFactory> factory = new ThingGraphFactory<ProgrammingLanguageFactory>();
            factory.Count = 10;
            factory.Populate(graph.Two);


            test.Mock.Scene.Focused = graph.Get (factory.Node[1]);
            Console.Out.Write(
                    GraphTestUtils.ReportGraph<IVisual, IVisualEdge>(graph.One, factory.Name));

            test.Mock.SceneFacade.Expand (false);

            Console.Out.Write(
                GraphTestUtils.ReportGraph<IVisual, IVisualEdge>(graph.One, factory.Name));

        }
    }
}
