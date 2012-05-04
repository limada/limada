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

using Limada.Schemata;
using NUnit.Framework;
using Limada.Model;
using Limada.Tests.Model;
using Limaki.Tests.Graph;

namespace Limada.Tests.Schemata {
    [TestFixture]
    public class CommonSchemaTest : SchemaTest {
        public override Schema Schema {
            get { return new CommonSchema (); }
        }
        
        [Test]
        public override void TestSchema() {
            base.TestSchema();
        }

        public void ValidateDescription(CommonSchema schema, IThingGraph graph, IThing thing, IThing description) {
            Assert.AreSame(schema.Description, description);

            this.ReportDetail(GraphTestUtils.ReportGraph<IThing, ILink>(graph, "* Descriptioin added"));
            bool found = false;
            bool firstMarker = false;
            foreach (ILink link in graph.Edges(thing)) {
                if (link.Marker == CommonSchema.DescriptionMarker) {
                    Assert.IsFalse(firstMarker,"second descriptionmarker found");
                    firstMarker = true;
                }
                if (link.Leaf == schema.Description && link.Marker == CommonSchema.DescriptionMarker
                    && link.Leaf == description) {
                    Assert.IsFalse(found);
                    found = true;
                }
            }
            Assert.IsTrue(found);
        }

        [Test]
        public virtual void TestDescription() {
            string testName = "TestDescription";
            this.ReportDetail(testName);
            IThingGraph graph = new ThingGraph ();
            IThing thing = Factory.CreateItem();
            graph.Add (thing);

            IThing description = Factory.CreateItem("Description1");
            CommonSchema schema = new CommonSchema (graph, thing);

            // test the new description:
            schema.Description = description;
            ValidateDescription (schema, graph, thing, description);
            ValidateDescription(schema, graph, thing, description);

            // add same description again:
            schema.Description = description;
            ValidateDescription(schema, graph, thing, description);

            // the first description will be an orphan:
            IThing orphan = description;
            
            // make a new description:
            description = Factory.CreateItem("Description2");
            schema.Description = description;
            ValidateDescription(schema, graph, thing, description);
            
            // test if orphan is deleted:
            Assert.IsFalse (graph.Contains (orphan),"Orphan not deleted");

            // take a new schema:
            schema = new CommonSchema (graph, thing);
            ValidateDescription(schema, graph, thing, description);

            ReportSummary();
        }
    }
}