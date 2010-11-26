/*
 * Limaki 
 * Version 0.063
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

        public virtual void ReportGraph(Graph<TItem, TEdge> graph, string reportHeader) {
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

        void ResetAndFillGraph(EdgeData<TItem, TEdge> data, Graph<TItem, TEdge> graph) {
            graph.Clear();
            foreach (TEdge edge in data.List) {
                graph.Add(edge);
            }
        }

        public Graph<TItem, TEdge> graph = new Graph<TItem, TEdge>();
        
        public bool dataListReported = false;

        public virtual void InitGraphTest(string TestName) {
            this.ReportMessage(TestName);
            if (!dataListReported) {
                this.ReportMessage ("Data:");
                foreach (TEdge edge in data.List) {
                    this.ReportMessage ("\t" + edge.ToString ());
                }
                dataListReported = true;
            }
            ResetAndFillGraph(data, graph);
            ReportGraph(graph,"Graph with Data:");
        }

        public virtual void FullReportGraph(Graph<TItem, TEdge> graph, string reportHeader) {
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
            graph.Add (data.Nothing);
            FullReportGraph(graph, "Added:\t(default("+typeof(TItem).Name+")");
        }
        
        [Test]
        public virtual void AddSingle() {
            InitGraphTest("** Add " + data.Single.ToString());
            graph.Add(data.Single);
            FullReportGraph(graph, "Added:\t"+data.Single.ToString());
            InitGraphTest("** Remove " + data.Single.ToString());
            graph.Remove(data.Single);
            FullReportGraph(graph, "Removed:\t" + data.Single.ToString());
        }

        [Test]
        public virtual void JustAddingData() {
            InitGraphTest("** JustAddingData");
        }

        [Test]
        public virtual void RemoveLink() {
            InitGraphTest("** Remove link");
            graph.Remove(data.OneAside);
            FullReportGraph(graph, "Removed:\t" + data.OneAside);
        }

        
        public virtual void RemoveItem() {
            InitGraphTest("** Remove item");
            graph.Remove(data.Two);
            FullReportGraph(graph, "Removed:\t"+data.Two.ToString());

            graph.Remove(data.OneAside.Root);
            graph.Remove(data.OneAside.Leaf);
            FullReportGraph(graph, 
                "Removed:\t" + data.OneAside.Root + "\n"+
                "Removed:\t" + data.OneAside.Leaf);

        }
    }
}
