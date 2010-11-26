/*
 * Limaki 
 * Version 0.071
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
using Limaki.Data.db4o;
using Limaki.Tests.Graph.Basic;
using Limaki.Tests.Graph.Model;
using NUnit.Framework;
using Limaki.Common;
using Limaki.Graphs;
using Limaki.UnitTest;
using Limaki.Tests.Graph;
using Limaki.Data;
using Limaki.Widgets;
using Limaki.Tests.Widget;
using WidgetGraph=Limaki.Data.db4o.WidgetGraph;

namespace Limaki.Tests.Data.db4o {
    /// <summary>
    /// Tests Limaki.Data.db4o.Graph#TItem,TEdge
    /// </summary>
    public class GraphTest : TestBase {
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
            FileName = @"E:\testdata\txbProjekt\Limaki\graphtest.limo";
        }



        [Test]
        public void StringGraphTest() {
            StringGraphTest graphTest = new StringGraphTest();

            Gateway.Open(DataBaseInfo.FromFileName(FileName));
            Limaki.Data.db4o.Graph<string, Edge<string>> graph =
                new Limaki.Data.db4o.Graph<string, Edge<string>>(Gateway);

            graphTest.Graph = graph;
            graphTest.Setup();
            graphTest.AllTests();
            graph.Clear();

            Gateway.Close();


        }
        [Test]
        public void EdgeIsItemTest() {
            Gateway.Open(DataBaseInfo.FromFileName(FileName));
            EdgeIsItemGraphTest graphTest = new EdgeIsItemGraphTest();
            Limaki.Data.db4o.Graph<Item<string>, EdgeItem<string>>
            graph = new Limaki.Data.db4o.Graph<Item<string>, EdgeItem<string>>(Gateway);

            graphTest.Graph = graph;
            graphTest.Setup();
            graphTest.AllTests();
            graph.Clear();
            Gateway.Close();


        }

        [Test]
        public void IntGraphTest () {
            Gateway.Open(DataBaseInfo.FromFileName(FileName));
            IntGraphTest graphTest = new IntGraphTest();
            Limaki.Data.db4o.Graph<int, Edge<int>>
            graph = new Limaki.Data.db4o.Graph<int, Edge<int>>(Gateway);

            graphTest.Graph = graph;
            graphTest.Setup();
            graphTest.AllTests();
            graph.Clear();
            Gateway.Close();


        }
        [Test]
        public void BenchmarkOneTest() {
            ISceneFactory sceneFactory = new BenchmarkOneSceneFactory();
            Gateway.Open(DataBaseInfo.FromFileName(FileName));

            WidgetGraph graph = new WidgetGraph(Gateway);
            sceneFactory.Count = 10;

            sceneFactory.Graph = graph;
            Scene scene = sceneFactory.Scene;
            //graph.Clear();
            Gateway.Close();
            ReportSummary();
        }


        public override void TearDown() {
            base.TearDown();
        }
    }
}
