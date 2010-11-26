/*
 * Limaki 
 * Version 0.064
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

namespace Limaki.Tests.Graph {
    public class StringEdgeData : EdgeData<string, Edge<string>> {
        protected override void InitItems() {
            One = "One";
            Two = "Two";
            Three = "Three";
            Aside = "Aside";
            Single = "Single";
        }
    }

    public class StringGraphTest : GraphTest<string,Edge<string>> {
        public override EdgeData<string, Edge<string>> createData() {
            return new StringEdgeData (); 
        }
        [Test]
        public override void AddNothing() {
            base.AddNothing();
        }
        [Test]
        public override void JustAddingData() {
            base.JustAddingData();
        }
        [Test]
        public override void RemoveLink() {
            base.RemoveLink();
        }
        [Test]
        public override void RemoveItem() {
            base.RemoveItem();
        }
        [Test]
        public override void AddSingle() {
            base.AddSingle();
        }
    }
    
}
