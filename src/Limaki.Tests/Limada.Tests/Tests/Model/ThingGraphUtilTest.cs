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
 * http://limada.sourceforge.net
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
                new IThing[] {factory.Node[1]},
                new IThing[] { factory.Node[1], factory.Node[2], factory.Edge[1],CommonSchema.DescriptionMarker },
                graph);

        }


        void Prove(IEnumerable<IThing> things, IEnumerable<IThing> expected, IThingGraph graph) {
            var completed = things.CompletedThings(graph);
            var result = new Set<IThing> ();
            result.UnionWith(completed);

            System.Console.WriteLine("** Completed:");
            foreach (var thing in completed) {
                System.Console.WriteLine(thing);
            }

            
            foreach(var thing in expected) {
                var compareThing = thing;
                Assert.IsTrue(result.Contains(compareThing), "Expected:" + thing.ToString());
            }

            Assert.AreEqual(expected.Count<IThing>(), result.Count);
        }

        [Test]
        public void CompletedThingsDocumentTest() {
            var thingGraph = new ThingGraph();
            var factory = new DocumentSchemaTestFactory();
            factory.Graph = thingGraph;
            factory.Populate();

            var graph = new SchemaThingGraph(thingGraph);

            Prove(
                new IThing[] { factory.Node[1] },

                new IThing[] { factory.Node[1], 
                               factory.Node[2], 
                               DocumentSchema.Document,
                               DocumentSchema.DocumentTitle, 
                               DocumentSchema.DocumentDefaultLink,
                               MetaSchema.DescriptionMarker,
                               factory.Edge[1] },
                graph);

            Prove(
                new IThing[] { factory.Node[1], factory.Node[3], factory.Edge[2] },
                
                new IThing[] { factory.Node[1], 
                               factory.Node[2], 
                               factory.Node[3], 
                               factory.Node[4], 

                               DocumentSchema.Document, 
                               DocumentSchema.DocumentTitle, 
                               
                               DocumentSchema.DocumentPage,
                               DocumentSchema.PageNumber,
                               
                               DocumentSchema.DocumentDefaultLink,
                               DocumentSchema.PageDefaultLink,
                               //DocumentSchema.HidePagesLink,
                               //ViewMetaSchema.Hide,
                               MetaSchema.DescriptionMarker,
                               
                               
                               //DocumentSchema.Author,
                               //DocumentSchema.AuthorHasDocument,
                               //MetaSchema.Root,

                               factory.Edge[1], 
                               factory.Edge[2],
                               factory.Edge[3]},
                graph);
        }
    }
}
