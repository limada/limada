/*
 * Limada
 * Version 0.081
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

using System;
using Limada.Model;
using Limada.Schemata;
using Limaki.Tests;
using Limaki.Tests.Graph;
using Limaki.UnitTest;
using NUnit.Framework;

namespace Limada.Tests.Schemata {
    [TestFixture]
    public class SchemaTest : DomainTest {
        public ThingFactory Factory = new ThingFactory();
        public virtual void TestIdentity(IThing thing) {
            bool identity = false;
            if (thing is ILink)
                identity = Schema.IdentityGraph.Contains((ILink)thing);
            else
                identity = Schema.IdentityGraph.Contains(thing);

            if (!identity) {
                this.ReportDetail(thing.ToString() + " is not contained in IdentityGraph");
            }
            Assert.IsTrue(identity);
        }

        public virtual void TestIdentity(IThingGraph graph) {
            foreach (IThing thing in graph) {
                TestIdentity(thing);
            }
            foreach (ILink thing in graph.Edges()) {
                TestIdentity(thing);
            }
            foreach (IThing thing in Schema.IdentityMap.Values) {
                TestIdentity(thing);
            }
        }

        public void TestSchema(Schema schema) {
            Console.WriteLine(GraphTestUtils.ReportGraph<IThing, ILink>(
                                  schema.SchemaGraph, schema.GetType().FullName + ".Graph"));
            Console.WriteLine(GraphTestUtils.ReportGraph<IThing, ILink>(
                                  Schema.IdentityGraph, "Schema.IdentityGraph"));

            TestIdentity(schema.SchemaGraph);
        }

        public virtual Schema Schema {
            get { return new Schema (); }
        }

        [Test]
        public virtual void TestSchema() {
            this.ReportDetail ("**** " + this.GetType ().FullName);
            TestSchema(Schema);
            TestSchema(Schema);
            ReportSummary();
        }
    }

    [TestFixture]
    public class MetaSchemaTest : SchemaTest {
        public override Schema Schema {
            get { return new MetaSchema(); }
        }

        [Test]
        public override void TestSchema() {
            base.TestSchema();
        }
    }

    [TestFixture]
    public class TopicSchemaTest : SchemaTest {
        public override Schema Schema {
            get { return new TopicSchema(); }
        }

        [Test]
        public override void TestSchema() {
            base.TestSchema();
        }
    }

    [TestFixture]
    public class ViewMetaSchemaTest : SchemaTest {
        public override Schema Schema {
            get { return new ViewMetaSchema(); }
        }

        [Test]
        public override void TestSchema() {
            base.TestSchema();
        }
    }
}