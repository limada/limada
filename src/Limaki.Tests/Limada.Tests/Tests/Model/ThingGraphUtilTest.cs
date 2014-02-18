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
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Limada.Model;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model;
using Limaki.Tests;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Graph.Wrappers;
using Limaki.Visuals;
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
            var factory = new SimpleDescriptionTestFactory();
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
            var factory = new DigidocTestFactory();
            factory.Graph = thingGraph;
            factory.Populate();

            var graph = new SchemaThingGraph(thingGraph);

            Prove(
                new IThing[] { factory.Nodes[1] },

                new IThing[] { factory.Nodes[1], 
                               factory.Nodes[2], 
                               DigidocSchema.Document,
                               DigidocSchema.DocumentTitle, 
                               DigidocSchema.DocumentDefaultLink,
                               MetaSchema.DescriptionMarker,
                               factory.Edges[1] },
                graph);

            Prove(
                new IThing[] { factory.Nodes[1], factory.Nodes[3], factory.Edges[2] },
                
                new IThing[] { factory.Nodes[1], 
                               factory.Nodes[2], 
                               factory.Nodes[3], 
                               factory.Nodes[4], 

                               DigidocSchema.Document, 
                               DigidocSchema.DocumentTitle, 
                               
                               DigidocSchema.DocumentPage,
                               DigidocSchema.PageNumber,
                               
                               DigidocSchema.DocumentDefaultLink,
                               DigidocSchema.PageDefaultLink,
                               //DigidocSchema.HidePagesLink,
                               //ViewMetaSchema.Hide,
                               MetaSchema.DescriptionMarker,
                               
                               
                               //DigidocSchema.Author,
                               //DigidocSchema.AuthorHasDocument,
                               //MetaSchema.Root,

                               factory.Edges[1], 
                               factory.Edges[2],
                               factory.Edges[3]},
                graph);
        }
    }
}
