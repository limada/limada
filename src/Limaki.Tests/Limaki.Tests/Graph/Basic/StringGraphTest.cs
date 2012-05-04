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
using Limaki.Model;
using NUnit.Framework;
using Limaki.Common;
using Limaki.Graphs;
using Limaki.UnitTest;

namespace Limaki.Tests.Graph.Basic {
    public class StringDataFactory : BasicTestDataFactory<string, Edge<string>> {
        protected override void CreateItems() {
            One = "One";
            Two = "Two";
            Three = "Three";
            Aside = "Aside";
            Single = "Single";
        }
    }

    public class StringGraphTest : BasicGraphTests<string,Edge<string>> {
        public override BasicTestDataFactory<string, Edge<string>> GetFactory() {
            return new StringDataFactory (); 
        }
        [Test]
        public override void AddNothing() {
            base.AddNothing();
        }
        [Test]
        public override void AddData() {
            base.AddData();
        }
        [Test]
        public override void RemoveEdge() {
            base.RemoveEdge();
        }
        [Test]
        public override void RemoveItem() {
            base.RemoveItem();
        }
        [Test]
        public override void AddSingle() {
            base.AddSingle();
        }
        [Test]
        public override void AllTests() {
            base.AllTests();
        }
    }
}
