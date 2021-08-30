/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2017 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using Limaki.Graphs;
using System.Linq;
using Limaki.Common;

namespace Limaki.Playground.View {

    public class Walker1<TItem, TEdge> : WalkerBase<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {

        public Walker1 (IGraph<TItem, TEdge> graph) : base (graph) { }

        public bool Trace { get; set; }

        protected void DoTrace (string m) {
            if (Trace)
                Console.WriteLine (m);
        }

        public IComparer<TItem> Comparer { get; set; }

        bool linqReverse = true;

        protected IEnumerable<TEdge> Ordered (IEnumerable<TEdge> edges, TItem path) {

            if (Comparer == null)
                return edges;

            if (false) {
                // order shout be like this:
                // if adjacent is not edge => sort, a
                // else order shoult be stable

                var adjacentIsItem = edges.Where (e => !(e.Adjacent (path) is IEdge<TItem>));
                var adjacentIsEdge = edges.Where (e => (e.Adjacent (path) is IEdge<TItem>));

                adjacentIsItem = adjacentIsItem.OrderBy (e => e.Adjacent (path), Comparer);
                if (linqReverse)
                    adjacentIsItem = adjacentIsItem.Reverse ();

                return adjacentIsEdge.Union (adjacentIsItem);
            } else { 
                var result = edges.OrderBy (e => e.Adjacent (path), Comparer);
                return linqReverse ? result.Reverse () : result;
            }

        }

        protected Tuple<TItem,TItem> Ordered (TEdge edge) {

            if (Comparer == null)
                return Tuple.Create (edge.Root, edge.Leaf);
            var comp = Comparer.Compare (edge.Root, edge.Leaf);
            return comp > 0 ? Tuple.Create (edge.Root, edge.Leaf) : Tuple.Create (edge.Leaf, edge.Root);
        }

        public virtual IEnumerable<LevelItem<TItem>> DeepWalk (TItem start, int startLevel, Func<LevelItem<TItem>, bool> predicate, bool breathFirst) {

            Action<TItem, TItem, int,bool> put = null;
            Action<LevelItem<TItem>> add = null;
            Action<LevelItem<TItem>> addReverse = null;
            Func<LevelItem<TItem>> get = null;
            Func<int> loopCount = null;

            var depthFirst = !breathFirst;
            var queued = breathFirst;
            var splitNonAdjacent = depthFirst;
            var edgeEdgesFirst = depthFirst;

            DoTrace ($"{nameof (DeepWalk)}:{nameof (breathFirst)} {breathFirst}\t{nameof (queued)} {queued}\t");
            var list = new LinkedList<LevelItem<TItem>> ();
            var llNode = list.Last;
            if (queued) {
                // first in first out
                loopCount = () => list.Count;
                get = () => { var item = list.First.Value; list.RemoveFirst (); DoTrace ($"\t{nameof (get)} {item}"); return item; };
                add = item => { list.AddLast (item); DoTrace ($"\t{nameof (add)} {item}"); };
                addReverse = add;
            } else {
                // last in last out
                loopCount = () => list.Count;

                get = () => { var item = list.Last.Value; list.RemoveLast (); DoTrace ($"\t{nameof (get)} {item}"); return item; };
                add = item => { llNode = null; list.AddLast (item); DoTrace ($"\t{nameof (add)} {item}"); };
                addReverse = item => { 
                    if (llNode!=null) 
                        llNode = list.AddBefore (llNode, item); 
                    else 
                        llNode = list.AddLast (item);
                    DoTrace ($"\t{nameof (addReverse)} {item}");

                };

                if (linqReverse)
                    addReverse = add;
            }

            put = (node, path, l, reverse) => {
                if (!Visited.Contains (node)) {
                    var item = new LevelItem<TItem> (node, path, l);
                    DoTrace ($"\t{nameof (put)} {item}");

                    Visited.Add (node);

                    if (predicate == null || predicate (item)) {
                        if (reverse) addReverse (item); else add (item);
                    }
                }
            };

            put (start, default (TItem), startLevel, false);

            while (loopCount () > 0) {

                var item = get ();
                if (true) {
                    DoTrace ($"\tyield {item}");
                    yield return item;
                }

                var level = item.Level + 1;
                if (item.Node is TEdge) {

                    var edge = (TEdge)item.Node;

                    var edgeTuple = Tuple.Create (edge.Leaf, edge.Root);

                    var adjacent = graph.Adjacent (edge, item.Path);

                    if (adjacent == null) {

                       edgeTuple = Ordered (edge);

                        if(splitNonAdjacent)
                            put (edgeTuple.Item1, edge, level, false);

                    }

                    Action putEdgeEdges = () => { 
                        // follow links of edge:
                        foreach (var edge_edge in Ordered (graph.Edges (edge), edge))
                            put (edge_edge, edge, level, true);
                    };

                    if (edgeEdgesFirst) putEdgeEdges ();

                    if (adjacent != null) {
                        // follow adjacent of node:
                        put (adjacent, edge, level, false);
                    } else {
                        if (splitNonAdjacent)
                            put (edgeTuple.Item2, edge, level, false);
                        else {
                            put (edgeTuple.Item1, edge, level, false);
                            put (edgeTuple.Item2, edge, level, false);
                        }
                    }

                    if (!edgeEdgesFirst) putEdgeEdges ();
                        
                } else {

                    // follow links of node:
                    foreach (var edge in Ordered(graph.Edges (item.Node), item.Node))
                        put (edge, item.Node, level, true);

                }

                if (false) {
                    DoTrace ($"\tyield {item}");
                    yield return item;
                }
            }
        }
    }
    
}