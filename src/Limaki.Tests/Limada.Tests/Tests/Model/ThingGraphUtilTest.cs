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

using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Limada.Model;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Graph.GraphPair;
using NUnit.Framework;
using Id = System.Int64;
using Limada.Schemata;
using System.Linq;
using Limaki.Common.Collections;

namespace Limada.Tests.Model {
    public class ThingGraphUtilTest: DomainTest {

        [Test]
        public void CompletedThingsSimpleTest() {
            var thingGraph = new ThingGraph ();
            var factory = new DescriptionSampleFactory();
            factory.Graph = thingGraph;
            factory.Populate ();

            var graph = new SchemaThingGraph(thingGraph);

            Prove (
                new IThing[] {factory.Nodes[1]},
                new IThing[] { factory.Nodes[1], factory.Nodes[2], factory.Edges[1],CommonSchema.DescriptionMarker },
                graph);

        }


        void Prove(IEnumerable<IThing> things, IEnumerable<IThing> expected, IThingGraph graph) {
            var completed = things.CompletedThings(graph);
            var result = new Set<IThing> ();
            result.UnionWith(completed);

            ReportDetail("** Completed:");
            foreach (var thing in completed) {
                ReportDetail(thing.ToString());
            }

            
            foreach(var thing in expected) {
                var compareThing = thing;
                Assert.IsTrue(result.Contains(compareThing), "Expected:" + thing.ToString());
            }

            Assert.AreEqual(expected.Count<IThing>(), result.Count);
        }

        [Test]
        public void CompletedThingsDigidocTest() {
            var thingGraph = new ThingGraph();
            var factory = new DigidocSampleFactory();
            factory.Graph = thingGraph;
            factory.Populate();

            var graph = new SchemaThingGraph(thingGraph);
            var expectedDefault = new IThing[] {
                DigidocSchema.Document,
                DigidocSchema.DocumentTitle,
                DigidocSchema.DocumentDefaultLink,
                DigidocSchema.PageNumber,
                DigidocSchema.PageDefaultLink,
                DigidocSchema.DocumentPage,
                DigidocSchema.HidePagesLink,
                ViewMetaSchema.Hide,
                MetaSchema.DescriptionMarker,
            };

            var pageItems = new List<IThing> ();
            for (int i = 0; i < factory.PageCount; i++) {
                pageItems.Add(factory.Nodes[factory.PageNodeStart + i*2]);
                pageItems.Add (factory.Edges[factory.PageEdgeStart + i*2]);
            }
            Prove (
                new IThing[] { factory.Nodes[1] }, 
                expectedDefault.Union (
                    new IThing[] {
                        factory.Nodes[1],
                        factory.Nodes[2],
                        factory.Edges[1],
                    }).Union(
                       pageItems
                    )
                    , graph);

            Prove(
                new IThing[] { factory.Nodes[1], factory.Nodes[3], factory.Edges[2] },
                expectedDefault.Union (
                    new IThing[] {
                        factory.Nodes[1], 
                               factory.Nodes[2], 
                               factory.Nodes[3], 
                               factory.Nodes[4], 
                               factory.Edges[1], 
                               factory.Edges[2],
                               factory.Edges[3],
                    }).Union(
                       pageItems
                    )
                    , graph);

        }
    }
}
