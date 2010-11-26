/*
 * Limaki 
 * Version 0.08
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using Limada.Model;
using Limada.Schemata;
using Limaki.Tests.Graph;
using NUnit.Framework;

namespace Limada.Tests.Schemata {
    [TestFixture]
    public class DocumentSchemaTest : SchemaTest {
        public override Schema Schema {
            get { return new DocumentSchema(); }
        }

        [Test]
        public override void TestSchema() {
            base.TestSchema();
        }

        #region Title
        public void ValidateTitle(DocumentSchema schema, IThingGraph graph, IThing thing, IThing title) {
            Assert.AreSame(schema.Title, title);

            this.ReportDetail(GraphTestUtils.ReportGraph<IThing, ILink>(graph, "* Title added"));
            bool found = false;
            bool firstMarker = false;
            foreach (ILink link in graph.Edges(thing)) {
                if (link.Marker == DocumentSchema.DocumentTitle) {
                    Assert.IsFalse(firstMarker, "second title found");
                    firstMarker = true;
                }
                if (link.Leaf == schema.Title && link.Marker == DocumentSchema.DocumentTitle
                    && link.Leaf == title) {
                    Assert.IsFalse(found);
                    found = true;
                }
            }
            Assert.IsTrue(found, "Title not found");
        }

        [Test]
        public virtual void TestTitle() {
            this.ReportDetail("**** TestTitle");
            IThingGraph graph = new ThingGraph();
            IThing thing = new Thing();
            graph.Add(thing);

            IThing title = new Thing<string>("Title1");
            DocumentSchema schema = new DocumentSchema(graph, thing);

            // test the new description:
            schema.Title = title;
            ValidateTitle(schema, graph, thing, title);
            ValidateTitle(schema, graph, thing, title);

            // add same description again:
            schema.Title = title;
            ValidateTitle(schema, graph, thing, title);

            // the first description will be an orphan:
            IThing orphan = title;

            // make a new description:
            title = new Thing<string>("Title2");
            schema.Title = title;
            ValidateTitle(schema, graph, thing, title);

            // test if orphan is deleted:
            Assert.IsFalse(graph.Contains(orphan), "Orphan not deleted");

            // take a new schema:
            schema = new DocumentSchema(graph, thing);
            ValidateTitle(schema, graph, thing, title);

        }
        #endregion
    }
}