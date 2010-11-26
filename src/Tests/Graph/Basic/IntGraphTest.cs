/*
 * Limaki 
 * Version 0.07
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
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Limaki.Common;
using Limaki.Graphs;
using Limaki.UnitTest;

namespace Limaki.Tests.Graph.Basic {
    public class IntDataFactory : BasicTestDataFactory<int, Edge<int>> {
        protected override void CreateItems() {
            One = 1;
            Two = 2;
            Three = 3;
            Aside = 100;
            Single = 123436;
        }
    }

    public class IntGraphTest : BasicGraphTests<int,Edge<int>> {
        public override BasicTestDataFactory<int, Edge<int>> GetFactory() {
            return new IntDataFactory (); 
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
