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
using Limada.Model;
using Limaki.Graphs;
using Limaki.Tests.Graph.Basic;
using NUnit.Framework;
using Limaki.Common;

namespace Limada.Tests.Basic {
    /// <summary>
    /// Test Limada.Model.ThingGraph
    /// </summary>
    public class BasicThingGraphTest : BasicGraphTests<IThing, ILink> {
        public override IGraph<IThing, ILink> Graph {
            get {
                if (_graph==null) {
                    _graph = new ThingGraph();
                }
                return base.Graph;
            }
            set {
                if (value is IThingGraph) {
                    base.Graph = value;
                } else {
                    throw new Exception ("graph must be a ThingGraph");
                }
            }
        }
        public override BasicTestDataFactory<IThing, ILink> GetFactory() {
            return new BasicThingDataFactory();
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
            Tickers.Resume();
            InitGraphTest ("** remove "+Data.One);
            Graph.Remove(Data.One);
            FullReportGraph (Graph,"Removed:\t" + Data.One);
            IsRemoved (Data.One);
            ReportSummary();
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

    public class BasicThingDataFactory : BasicTestDataFactory<IThing, ILink> {
        private IThingFactory _factory = null;
        public IThingFactory Factory { get { return _factory ?? (_factory = Registry.Factory.Create<IThingFactory>()); } }

        private IThing marker = null;
        protected override void CreateItems() {
            marker = Factory.CreateItem("Marker");
            One = Factory.CreateItem("One");
            Two = Factory.CreateItem("Two");
            Three = Factory.CreateItem("Three");
            Aside = Factory.CreateItem("Aside");
            Single = Factory.CreateItem("Single");
        }
        protected override ILink CreateEdge(IThing root, IThing leaf) {
            return Factory.CreateEdge(root, leaf, marker);
        }
        protected override void CreateEdges() {
            base.CreateEdges();
            foreach (ILink edge in base.Edges) {
                edge.Marker = marker;
            }
        }
        
        // not working in studio 2010:
        //public override IEnumerable<ILink> Edges {
        //    get {
        //        foreach (ILink edge in base.Edges) {
        //            edge.Marker = marker;
        //            yield return edge;
        //        }
        //    }
        //}
    }
}