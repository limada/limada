/*
 * Limada
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.IO;
using System.Linq;
using Limaki.Common.Linqish;
using Limaki.Tests;
using NUnit.Framework;
using Limaki.Data;
using Limada.Data;
using Limada.Model;
using Limaki.Common;
using System;


namespace Limaki.LinqData.Tests {

    public class ThingGraphTest : Limada.Tests.ThingGraphs.ThingGraphTest {

        public static void TearUp (Limada.Tests.ThingGraphs.ThingGraphTestBase test) {
            DbQuoreTestData.TearUp ();
            test.ThingGraphIo = new QuoreThingGraphIo ();
        }

        public static Iori CheckIori (Iori iori) {
            if (iori != null && iori.Provider == null) {
                return DbQuoreTestData.Iori;
            }
            return iori;
        }

        public override void Setup () {
            base.Setup ();
            TearUp (this);
        }

        public override Iori Iori => CheckIori (base.Iori);

        [Test]
        public void TestOpen () {
            var content = this.ThingGraphIo.Open (this.Iori);
            ReportDetail ("{0}", this.Graph.Count);
            this.ThingGraphIo.Close (content);

        }

        [Test]
        public void TestReadWrite () {
            var strdata = "hello";
            var thing = this.Factory.CreateItem (strdata);
            this.Graph.Add (thing);
            var readThing = this.Graph.FirstOrDefault ();
            Assert.IsNotNull (readThing);

            try {
                // this fails!
                // Thing.Data is not a valid property in a Database
                readThing = this.Graph.WhereQ (t => t.Data == strdata).FirstOrDefault ();
                Assert.IsNotNull (readThing);
            } catch (Exception ex) {
                ReportDetail ("{0} {1}", "Must fail cause Thing.Data is not a valid property", ex.Message);
            }
            readThing = this.Graph.WhereQ<IThing<string>> (t => t.Data == strdata).FirstOrDefault ();
            Assert.IsNotNull (readThing);
            readThing = this.Graph.GetByData (strdata).FirstOrDefault ();
            Assert.IsNotNull (readThing);
        }
    }

    public class SchemaGraphPerformanceTest : Limada.Tests.ThingGraphs.SchemaGraph.SchemaGraphPerformanceTest {

        public override void Setup () {
            base.Setup ();
            ThingGraphTest.TearUp (this);
        }

        public override Iori Iori => ThingGraphTest.CheckIori (base.Iori);

    }

    public class StreamThingTest : Limada.Tests.ThingGraphs.StreamThingTest {
        public override void Setup () {
            base.Setup ();
            ThingGraphTest.TearUp (this);
        }
        public override Iori Iori => ThingGraphTest.CheckIori (base.Iori);
    }

    public class ThingGraphDeleteItemsTest : Limada.Tests.ThingGraphs.ThingGraphDeleteItemsTest {
        public override void Setup () {
            base.Setup ();
            ThingGraphTest.TearUp (this);
        }
        public override Iori Iori => ThingGraphTest.CheckIori (base.Iori);
    }


    public class SchemaGraphTest : Limada.Tests.ThingGraphs.SchemaGraph.SchemaGraphTest {
        public override void Setup () {
            base.Setup ();
            ThingGraphTest.TearUp (this);
        }
        public override Iori Iori => ThingGraphTest.CheckIori (base.Iori);
    }
}