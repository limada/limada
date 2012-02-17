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
 * http://limada.sourceforge.net
 * 
 */


using System;
using System.Collections.Generic;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using NUnit.Framework;

namespace Limaki.Tests.Graph.Basic {
    public class GraphItemDataFactory : BasicTestDataFactory<IGraphItem, IGraphEdge> {
        protected override void CreateItems() {
            One = new GraphItem<string>("One");
            Two = new GraphItem<string>("Two");
            Three = new GraphItem<string>("Three");
            Aside = new GraphItem<string>("Aside");
            Single = new GraphItem<string>("Single");
        }

        protected override IGraphEdge CreateEdge(IGraphItem root, IGraphItem leaf) {
            return new GraphEdge(root, leaf);
        }
    }

    public class GraphItemGraphTest : BasicGraphTests<IGraphItem, IGraphEdge> {
        public override BasicTestDataFactory<IGraphItem, IGraphEdge> GetFactory() {
            return new GraphItemDataFactory();
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
            InitGraphTest("** remove " + Data.One);
            Graph.Remove(Data.One);
            FullReportGraph(Graph, "Removed:\t" + Data.One);

        }
        [Test]
        public override void AddSingle() {
            base.AddSingle();
        }
        [Test]
        public override void AddEdge() {
            base.AddEdge();
        }

        [Test]
        public override void ChangeEdge() {
            base.ChangeEdge();
        }

        [Test]
        public override void AllTests() {
            base.AllTests();
        }
    }
}