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
using Limaki.Graphs;
using Limaki.UnitTest;
using Limaki.Tests.Graph;
using Limaki.Tests.Graph.Model;
using NUnit.Framework;
using System.IO;
using Limaki.Common.Collections;

namespace Limaki.Tests.Graph.MethodTests {
    
    public class TestingSomeGraphSemantics:TestBase {
        private IList<BinaryGraphFactoryBase> _graphs = null;
        public IList<BinaryGraphFactoryBase> Graphs {
            get {
                if (_graphs == null) {
                    _graphs = new List<BinaryGraphFactoryBase>();
                    _graphs.Add (new BinaryTreeFactory());
                    _graphs.Add (new BinaryGraphFactory ());
                }
                return _graphs;
            }
            set { _graphs = value; }
        }

        # region BreathFirst Twig
        // remark: NO! not true! this Algos form a Clique (see:http://en.wikipedia.org/wiki/Clique_problem)
        

        IEnumerable<TEdge> BreathFirstTwigQueue<TItem, TEdge>(IGraph<TItem, TEdge> graph, TItem source, bool preOrder)
        where TEdge : IEdge<TItem> {
            Queue<IEnumerable<TEdge>> work = new Queue<IEnumerable<TEdge>>();
            work.Enqueue(graph.Edges(source));
            while (work.Count > 0) {
                IEnumerable<TEdge> curr = work.Dequeue();
                foreach (TEdge edge in curr) {
                    if (graph.EdgeIsItem) {
                        TItem edgeAsItem = (TItem)(object)edge;
                        work.Enqueue(graph.Edges(edgeAsItem));
                    }
                    yield return edge;
                }
            }
        }

        IEnumerable<TEdge> BreathFirstTwigRecursion<TItem, TEdge>(IGraph<TItem, TEdge> graph, TItem source, bool preOrder)
        where TEdge : IEdge<TItem> {
            foreach (TEdge edge in graph.Edges(source)) {
                if (preOrder)
                    yield return edge;
                if (graph.EdgeIsItem) {
                    TItem edgeAsItem = (TItem)(object)edge;
                    foreach (TEdge recurse in BreathFirstTwigRecursion<TItem, TEdge>(graph, edgeAsItem, preOrder)) {
                        yield return recurse;
                    }
                }
                if (!preOrder)
                    yield return edge;
            }
        }

        public void BreathFirstTwig(IGraph<IGraphItem, IGraphEdge> graph) {
            foreach(IGraphItem item in graph) {
                StringWriter sQueue = new StringWriter ();
                StringWriter sRecursion = new StringWriter();
                Set<IGraphEdge> queued = new Set<IGraphEdge> ();
                Set<IGraphEdge> recursed = new Set<IGraphEdge>();
                foreach(IGraphEdge edge in BreathFirstTwigQueue<IGraphItem,IGraphEdge>(graph,item,true)) {
                    queued.Add (edge);
                    sQueue.Write(edge.ToString() + " : ");
                } 
                foreach (IGraphEdge edge in BreathFirstTwigRecursion<IGraphItem, IGraphEdge>(graph, item, true)) {
                    recursed.Add (edge);
                    sRecursion.Write(edge.ToString() + " : ");
                }
                ReportDetail(item.ToString() + "\t(r): " + sQueue.ToString() + "\t==\t(q): " + sRecursion.ToString());
                // remark: testing the strings doesnt work as Set has no stable order of Edges
                // we can only test contains
                Assert.AreEqual (queued.Count, recursed.Count);
                foreach(IGraphEdge edge in queued) {
                    Assert.IsTrue (recursed.Contains (edge));
                }
            }
        }

        [Test]
        public void TestTwig() {
            foreach (BinaryGraphFactoryBase factory in this.Graphs) {
                ReportDetail (factory.Name);
                factory.Count = 15;
                factory.AddDensity = false;
                factory.Populate ();
                BreathFirstTwig (factory.Graph);
                
            }
        }

        #endregion
        #region Fork and Clique
        // Fork: alle Edges wich are connected without touching a Item 
        // CliqueEdges: alle Edges wich are connected including touching Items




        #endregion
    }
}
