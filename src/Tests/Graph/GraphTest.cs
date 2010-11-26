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
    public abstract class GraphTest<TItem, TEdge> : TestBase
        where TEdge : IEdge<TItem>  {
        public abstract EdgeData<TItem, TEdge> createData();

        EdgeData<TItem, TEdge> _data = null;
        public virtual EdgeData<TItem, TEdge> data {
            get {
                if (_data == null) {
                    _data = createData ();
                }
                return _data;
            }
        }

        protected IGraph<TItem, TEdge> _graph = null;
        public virtual IGraph<TItem, TEdge> Graph {
            get {
                if (_graph == null) {
                    _graph = new Graph<TItem, TEdge>(); 
                }
                return _graph;
            }
            set { _graph = value; }
        }

        public virtual void ReportGraph(IGraph<TItem, TEdge> graph, string reportHeader) {
            this.ReportMessage(reportHeader);
            foreach (System.Collections.Generic.KeyValuePair<TItem, ICollection<TEdge>> kvp in graph.ItemsWithEdges()) {
                bool first = true;
                string emptyString = new string(' ', kvp.Key.ToString().Length);
                foreach (TEdge edge in kvp.Value) {
                    if (first) {
                        if (edge != null)
                            this.ReportMessage("\t" + kvp.Key.ToString() + "\t: " + edge.ToString());
                        else
                            this.ReportMessage("\t" + kvp.Key.ToString() + "\t: <null>");
                        first = false;
                    } else {
                        this.ReportMessage("\t" + emptyString + "\t: " + edge.ToString());
                    }
                }
            }
        }

        void ResetAndFillGraph(EdgeData<TItem, TEdge> data, IGraph<TItem, TEdge> graph) {
            graph.Clear();
            foreach (TEdge edge in data.Edges) {
                graph.Add(edge);
            }
        }

        
        
        public bool dataListReported = false;

        public virtual void InitGraphTest(string TestName) {
            this.ReportMessage(TestName);
            if (!dataListReported) {
                this.ReportMessage ("Data:");
                foreach (TEdge edge in data.Edges) {
                    this.ReportMessage ("\t" + edge.ToString ());
                }
                dataListReported = true;
            }
            ResetAndFillGraph(data, Graph);
            ReportGraph(Graph,"Graph with Data:");
        }

        public virtual void FullReportGraph(IGraph<TItem, TEdge> graph, string reportHeader) {
            ReportGraph(graph, reportHeader);
            this.ReportMessage("Foreach Item in graph");
            foreach (TItem item in graph) {
                this.ReportMessage("\t" + item);
            }
            this.ReportMessage("Foreach Edge in graph.Edges() ");
            foreach (TEdge edge in graph.Edges()) {
                this.ReportMessage("\t" + edge);
            }
            this.ReportMessage("Foreach Item Foreach Edge in graph.Edges(Item)");
            foreach (TItem item in graph) {
                bool first = true;
                string placeHolder = "default("+typeof(TItem).Name+")";
                if (item!=null)
                    placeHolder = new string(' ', item.ToString().Length);
                foreach (TEdge edge in graph.Edges(item)) {
                    if (first) {
                        this.ReportMessage("\t" + item + "\t: " + edge.ToString());
                        first = false;
                    } else {
                        this.ReportMessage("\t" + placeHolder + "\t: " + edge.ToString());
                    }
                }
                if (first) {
                    this.ReportMessage("\t" + item + "\t: <no edges>");
                }
            }
        }

        [Test]
        public virtual void AddNothing() {
            InitGraphTest("** Add (default("+typeof(TItem).Name+")");
            Graph.Add (data.Nothing);
            FullReportGraph(Graph, "Added:\t(default("+typeof(TItem).Name+")");
            Graph.Remove(data.Nothing);
            FullReportGraph(Graph, "Removed:\t(default(" + typeof(TItem).Name + ")");

        }
        
        [Test]
        public virtual void AddSingle() {
            InitGraphTest("** Add " + data.Single.ToString());
            Graph.Add(data.Single);
            FullReportGraph(Graph, "Added:\t"+data.Single.ToString());
            if (Graph.ItemIsStorable) {
                Assert.IsTrue (Graph.Contains (data.Single));
            }
            InitGraphTest("** Remove " + data.Single.ToString());
            Graph.Remove(data.Single);
            FullReportGraph(Graph, "Removed:\t" + data.Single.ToString());
            IsRemoved(data.Single);
        }

        [Test]
        public virtual void JustAddingData() {
            InitGraphTest("** JustAddingData");


            Assert.IsTrue (Graph.Contains (data.One));
            Assert.IsTrue(Graph.Contains(data.Two));
            Assert.IsTrue(Graph.Contains(data.Three));
            Assert.IsTrue(Graph.Contains(data.Aside));

            foreach(TEdge edge in data.Edges) {
                Assert.IsTrue(Graph.Contains(edge));
            }

        }

        [Test]
        public virtual void RemoveLink() {
            InitGraphTest("** Remove link");
            Graph.Remove(data.OneAside);
            FullReportGraph(Graph, "Removed:\t" + data.OneAside);
            Assert.IsFalse (Graph.Contains (data.OneAside));
            if (Graph.EdgeIsItem) {
                IsRemoved ((TItem)(object)data.OneAside);
            }
            if (Graph.ItemIsStorable) {
                Assert.IsTrue(Graph.Contains(data.OneAside.Root));
                Assert.IsTrue(Graph.Contains(data.OneAside.Leaf));    
            }
        }

        public virtual void IsRemoved(TItem item) {
            if (Graph.ItemIsStorable) {
                Assert.IsFalse(Graph.Contains(item));
            }

            int count = 0;
            foreach (TEdge edge in Graph.Edges(item)) {
                count++;
            }
            Assert.IsTrue (count == 0);
            
        }
        [Test]
        public virtual void RemoveItem() {
            InitGraphTest("** Remove item");
            Graph.Remove(data.Two);
            FullReportGraph(Graph, "Removed:\t"+data.Two.ToString());
            IsRemoved (data.Two);

            Graph.Remove(data.OneAside.Root);
            Graph.Remove(data.OneAside.Leaf);
            FullReportGraph(Graph, 
                "Removed:\t" + data.OneAside.Root + "\n"+
                "Removed:\t" + data.OneAside.Leaf);
            IsRemoved (data.OneAside.Root);
            IsRemoved (data.OneAside.Leaf);
        }
    }
}
