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
using Limada.Model;
using Limada.Schemata;
using Limaki.Tests.Graph;
using Limaki.UnitTest;
using NUnit.Framework;

namespace Limada.Tests.Schemata {
    [TestFixture]
    public class SchemaTest1 : SchemaTest {
        public class TestSchema1 : Schema {
            public IThing marker2 = Thing<string>("Marker2", 0x1D51044A508E6685);
            [UniqueThing]
            public static ILink link1 { get { return Link(marker1, marker1, marker1, 0x0C0D72694A11042E); } }
            [UniqueThing]
            public static IThing marker1 { get { return Thing<string>("Marker1", 0x2F6A0AC91B149B9D); } }
            
        }
        
        public override Schema Schema {
            get {
                return new TestSchema1();
            }
        }

        [Test]
        public override void TestSchema() {
            base.TestSchema ();
        }
    }


}