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
using Limaki.Graphs;
using Limaki.Model;
using Limaki.UnitTest;
using Limaki.Tests.Graph;
using Limaki.Tests.Graph.Model;
using NUnit.Framework;
using System.IO;
using Limaki.Common.Collections;

namespace Limaki.Tests.Graph.MethodTests {
    
    public class TestingSomeGraphSemantics:DomainTest {

        private IList<ISampleGraphFactory<IGraphEntity, IGraphEdge>> _graphs = null;
        public IList<ISampleGraphFactory<IGraphEntity, IGraphEdge>> Graphs {
            get {
                if (_graphs == null) {
                    _graphs = new List<ISampleGraphFactory<IGraphEntity, IGraphEdge>> ();
                    _graphs.Add (new EntityBinaryTreeFactory());
                    _graphs.Add (new EntityBinaryGraphFactory ());
                }
                return _graphs;
            }
            set { _graphs = value; }
        }

        # region BreathFirst Twig
        

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
            foreach (var edge in graph.Edges(source)) {
                if (preOrder)
                    yield return edge;
                if (graph.EdgeIsItem) {
                    var edgeAsItem = (TItem)(object)edge;
                    foreach (var recurse in BreathFirstTwigRecursion(graph, edgeAsItem, preOrder)) {
                        yield return recurse;
                    }
                }
                if (!preOrder)
                    yield return edge;
            }
        }

        public void BreathFirstTwig (IGraph<IGraphEntity, IGraphEdge> graph) {

            foreach (var item in graph) {
                var sQueue = new StringWriter();
                var sRecursion = new StringWriter();
                var queued = new Set<IGraphEdge>();
                var recursed = new Set<IGraphEdge>();
                foreach (var edge in BreathFirstTwigQueue (graph, item, true)) {
                    queued.Add (edge);
                    sQueue.Write (edge.ToString() + " : ");
                }
                foreach (var edge in BreathFirstTwigRecursion (graph, item, true)) {
                    recursed.Add (edge);
                    sRecursion.Write (edge.ToString() + " : ");
                }
                ReportDetail (item.ToString() + "\t(r): " + sQueue.ToString() + "\t==\t(q): " + sRecursion.ToString());
                // remark: testing the strings doesnt work as Set has no stable order of Edges
                // we can only test contains
                Assert.AreEqual (queued.Count, recursed.Count);
                foreach (var edge in queued) {
                    Assert.IsTrue (recursed.Contains (edge));
                }
            }
        }

        [Test]
        public void TestTwig() {
            foreach (var factory in this.Graphs) {
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
        // remark: This Algos form a Clique (see:http://en.wikipedia.org/wiki/Clique_problem)


        IEnumerable<TEdge> Fork<TItem, TEdge>(IGraph<TItem, TEdge> graph, TEdge source)
        where TEdge : IEdge<TItem> {
            if (source.Root is TEdge) {
                TEdge root = (TEdge) (object) source.Root;
                yield return root;
                foreach (TEdge recurse in Fork<TItem,TEdge>(graph,root)) {
                    yield return recurse;
                }
            }            
            if (source.Leaf is TEdge) {
                TEdge leaf = (TEdge)(object)source.Leaf;
                yield return leaf;
                foreach (TEdge recurse in Fork<TItem, TEdge>(graph, leaf)) {
                    yield return recurse;
                }
            }
        }

        IEnumerable<TEdge> Fork<TItem, TEdge>(IGraph<TItem, TEdge> graph, TItem source)
        where TEdge : IEdge<TItem> {
                foreach(TEdge edge in graph.Edges(source)) {
                    yield return edge;
                    foreach (TEdge recurse in Fork<TItem, TEdge>(graph, edge)) {
                        yield return recurse;
                    }
                }
        }

        IEnumerable<TEdge> ForkQueue<TItem, TEdge>(IGraph<TItem, TEdge> graph, TItem source)
        where TEdge : IEdge<TItem> {
            var work = new Queue<TEdge>();
            var done = new Set<TEdge>();
            foreach (var edge in graph.Edges(source)) {
                work.Enqueue (edge);
            }
            if (source is TEdge) {
                var curr = (TEdge) (object) source;
                done.Add(curr);
                if (curr.Root is TEdge) {
                    work.Enqueue((TEdge)(object)curr.Root);
                }
                if (curr.Leaf is TEdge) {
                    work.Enqueue((TEdge)(object)curr.Leaf);
                }
            }
            while (work.Count > 0) {
                TEdge curr = work.Dequeue ();
                if (!done.Contains(curr)) {
                    if (curr.Root is TEdge) {
                        work.Enqueue((TEdge)(object)curr.Root);
                    }
                    if (curr.Leaf is TEdge) {
                        work.Enqueue((TEdge)(object)curr.Leaf);
                    }
                    
                    done.Add(curr);
                    
                    yield return curr;
                }
            }
        }

        [Test]
        public void TestFork() {
            foreach (var factory in this.Graphs) {
                ReportDetail(factory.Name);
                factory.Populate();
                var graph = factory.Graph;
                foreach(IGraphEntity item in graph) {
                    ReportDetail(
                        GraphTestUtils.ReportItemAndEdges<IGraphEntity, IGraphEdge>(
                       item, ForkQueue<IGraphEntity, IGraphEdge>(graph, item))
                       );
                    
                }

            }
        }

        #endregion
    }
}
