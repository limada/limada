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


using Limaki.Data;
using Limaki.Data.db4o;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Basic;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.View.Visuals;
using NUnit.Framework;

namespace Limaki.Tests.Data.db4o {
    /// <summary>
    /// Tests Limaki.Data.db4o.Graph#TItem,TEdge
    /// </summary>
    public class GraphTest : DomainTest {
        public string FileName;

        private Limaki.Data.db4o.Gateway _gateway = null;
        public Gateway Gateway {
            get {
                if (_gateway == null) {
                    _gateway = new Gateway();

                }
                return _gateway;
            }
            set { _gateway = value; }
        }

        public override void Setup() {
            base.Setup();
            FileName = TestLocations.GraphtestFile+".limo";
        }
        
        [Test]
        public void EdgeIsItemTest() {
            Gateway.Open(Iori.FromFileName(FileName));
            var graphTest = new EdgeIsItemGraphTest();
            Limaki.Data.db4o.Graph<Item<string>, EdgeItem<string>>
            graph = new Limaki.Data.db4o.Graph<Item<string>, EdgeItem<string>>(Gateway);

            graphTest.Graph = graph;
            graphTest.Setup();
            graphTest.AllTests();
            graph.Clear();
            Gateway.Close();


        }

        [Test]
        public void BenchmarkOneTest() {
            var sceneFactory =
                new BenchmarkOneGraphFactory<IGraphEntity, IGraphEdge> ();
            Gateway.Open(Iori.FromFileName(FileName));

            var graph =
                new Limaki.Data.db4o.Graph<IGraphEntity, IGraphEdge>(Gateway);
            sceneFactory.Count = 10;

            sceneFactory.Populate(graph);
            //graph.Clear();
            Gateway.Close();
            ReportSummary();
        }


        public override void TearDown() {
            base.TearDown();
        }

        [Test]
        [Ignore ("int as item is currently not supported")]
        public void IntGraphTest () {
            Gateway.Open (Iori.FromFileName (FileName));
            var graphTest = new IntGraphTest ();
            Limaki.Data.db4o.Graph<int, Edge<int>>
            graph = new Limaki.Data.db4o.Graph<int, Edge<int>> (Gateway);

            graphTest.Graph = graph;
            graphTest.Setup ();
            graphTest.AllTests ();
            graph.Clear ();
            Gateway.Close ();


        }

        [Test]
        [Ignore ("string as item is currently not supported")]
        public void StringGraphTest () {
            var graphTest = new StringGraphTest ();

            Gateway.Open (Iori.FromFileName (FileName));
            Limaki.Data.db4o.Graph<string, Edge<string>> graph =
                new Limaki.Data.db4o.Graph<string, Edge<string>> (Gateway);

            graphTest.Graph = graph;
            graphTest.Setup ();
            graphTest.AllTests ();
            graph.Clear ();

            Gateway.Close ();


        }
        
    }
}
