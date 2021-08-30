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


using System.Collections.Generic;
using System;
using Limaki.Common;

namespace Limaki.Graphs {

    /// <summary>
    /// iterates through a <see cref="IGraph{TItem, TEdge}"/>
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public class Walker<TItem, TEdge> : WalkerBase<TItem, TEdge>
        where TEdge : IEdge<TItem>, TItem {

        public Walker (IGraph<TItem, TEdge> graph) : base (graph) { }

        /// <summary>
        /// Iterates over all items and links which are connected to each other
        /// beginning with item start
        /// items and links are distinct (they dont repeat if there are circles)
        /// </summary>
        /// <remarks>the clique containing the start-item, but with multiple egdes allowed</remarks>
        /// <param name="start"></param>
        /// <param name="startLevel"></param>
        /// <returns></returns>
        public virtual IEnumerable<LevelItem<TItem>> DeepWalk (TItem start, int startLevel, Func<LevelItem<TItem>, bool> predicate, bool breathFirst) {

            Action<TItem, TItem, int> put = null; ;
            Func<LevelItem<TItem>> get = null;
            Func<int> loopCount = null;

            if (breathFirst) {
                var queue = new Queue<LevelItem<TItem>> ();
                loopCount = () => queue.Count;

                put = (node, path, l) => {
                    if (!Visited.Contains (node)) {
                        var item = new LevelItem<TItem> (node, path, l);
                        if (predicate == null || predicate (item))
                            queue.Enqueue (item);
                        Visited.Add (node);
                    }
                };

                get = () => queue.Dequeue ();
            } else {
                var stack = new Stack<LevelItem<TItem>> ();
                loopCount = () => stack.Count;

                put = (node, path, l) => {
                    if (!Visited.Contains (node)) {
                        var item = new LevelItem<TItem> (node, path, l);
                        if (predicate == null || predicate (item))
                            stack.Push (item);
                        Visited.Add (node);
                    }
                };

                get = () => stack.Pop ();
            }

            put (start, default (TItem), startLevel);

            while (loopCount () > 0) {

                var item = get ();
                yield return item;

                var level = item.Level + 1;

                if (item.Node is TEdge) {
                    var edge = (TEdge)item.Node;

                    // follow links of edge:
                    foreach (var edge_edge in graph.Edges (edge))
                        put (edge_edge, edge, level);

                    var adjacent = graph.Adjacent (edge, item.Path);

                    if (adjacent != null) {
                        // follow adjacent of node:
                        put (adjacent, edge, level);
                    } else {
                        put (edge.Root, edge, level);
                        put (edge.Leaf, edge, level);
                    }

                } else {
                    // follow links of node:
                    foreach (var edge in graph.Edges (item.Node))
                        put (edge, item.Node, level);

                }
            }
        }

        public virtual IEnumerable<LevelItem<TItem>> DeepWalk (TItem start, int level, Func<LevelItem<TItem>, bool> predicate) {
            return DeepWalk (start, level, predicate, true);
        }

        public virtual IEnumerable<LevelItem<TItem>> DeepWalk (TItem start, int level) {
            return DeepWalk (start, level, null, true);
        }

        /// <summary>
        /// iterates over all edges and nodes connected with start
        /// iterates over all edges of edges recursivly
        /// if edge.Root or edge.Leaf is an edge, it is iterated too
        /// </summary>
        /// <param name="start"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public virtual IEnumerable<LevelItem<TItem>> Walk (TItem start, int level) {
            return Walk (start, level, null);
        }

        public virtual IEnumerable<LevelItem<TItem>> Walk (TItem start, int level, Func<LevelItem<TItem>, bool> predicate) {
            var queue = new Queue<LevelItem<TItem>> ();
            Func<TItem, TItem, int, LevelItem<TItem>> take = (node, path, l) => {
                if (!Visited.Contains (node)) {
                    var item = new LevelItem<TItem> (node, path, l);
                    if (predicate == null || predicate (item)) {
                        if (node is TEdge)
                            queue.Enqueue (item);
                        else
                            return item;
                    }
                    Visited.Add (node);
                }
                return null;
            };

            Func<TItem, TItem, int, LevelItem<TItem>> enqueue = (node, path, l) => {
                if (!Visited.Contains (node)) {
                    var item = new LevelItem<TItem> (node, path, l);
                    if (predicate == null || predicate (item))
                        queue.Enqueue (item);
                    Visited.Add (node);
                }
                return null;
            };

            enqueue (start, default (TItem), level);

            while (queue.Count > 0) {
                var item = queue.Dequeue ();
                yield return item;

                level = item.Level + 1;
                if (item.Node is TEdge) {

                    var edge = (TEdge)item.Node;

                    // follow link of links  // Fork!?
                    foreach (var edge_edge in graph.Edges (edge))
                        enqueue (edge_edge, edge, level);

                    var adjacent = graph.Adjacent (edge, item.Path);
                    if (adjacent != null) {
                        // follow adjacent of node:
                        var result = take (adjacent, edge, level);
                        if (result != null)
                            yield return result;

                    } else {
                        var result = take (edge.Root, edge, level);
                        if (result != null)
                            yield return result;
                        result = take (edge.Leaf, edge, level);
                        if (result != null)
                            yield return result;
                    }
                } else {
                    foreach (var edge in graph.Edges (item.Node))
                        enqueue (edge, item.Node, level);
                }
            }
        }

        public virtual IEnumerable<LevelItem<TItem>> ExpandWalk (TItem start, int level) {
            return ExpandWalk (start, level, null);
        }

        public virtual IEnumerable<LevelItem<TItem>> ExpandWalk (TItem start, int level, Func<LevelItem<TItem>, bool> predicate) {

            var queue = new Queue<LevelItem<TItem>> ();
            Action<TItem, TItem, int> enqueue = (node, path, l) => {
                if (!Visited.Contains (node)) {
                    var item = new LevelItem<TItem> (node, path, l);
                    if (predicate == null || predicate (item))
                        queue.Enqueue (item);
                    Visited.Add (node);
                }
            };

            enqueue (start, default (TItem), level);

            while (queue.Count > 0) {
                var item = queue.Dequeue ();
                yield return item;

                level = item.Level;

                if (item.Node is TEdge) {

                    var edge = (TEdge)item.Node;

                    var adjacent = graph.Adjacent (edge, start);
                    if (adjacent != null || (edge.Equals (start)) || (graph.RootIsEdge (edge) && graph.LeafIsEdge (edge))) {
                        // follow link of links
                        foreach (var edge_edge in graph.Edges (edge))
                            enqueue (edge_edge, edge, level + 1);
                    }

                    if (adjacent != null) { // follow adjacent of start:
                        enqueue (adjacent, edge, level);
                    } else {
                        enqueue (edge.Root, edge, level);
                        enqueue (edge.Leaf, edge, level);

                    }
                } else if (item.Node.Equals (start)) {
                    foreach (var edge in graph.Edges (item.Node))
                        enqueue (edge, item.Node, level + 1);

                }
            }
        }

        public virtual IEnumerable<LevelItem<TItem>> CollapseWalk (TItem start, int level) {
            var queue = new Queue<TItem> ();
            foreach (var edge in graph.Edges (start)) {
                foreach (var subedge in graph.Edges (edge)) {
                    if (!Visited.Contains (subedge)) {
                        if (subedge.Leaf.Equals (edge)) {
                            continue;
                        }
                        queue.Enqueue (subedge);
                        Visited.Add (subedge);
                        if (!Visited.Contains (subedge.Leaf)) {
                            queue.Enqueue (subedge.Leaf);
                            Visited.Add (subedge.Leaf);
                        }
                    }
                }

                if (!Visited.Contains (edge)) {
                    if (edge.Leaf.Equals (start)) {
                        continue;
                    }
                    queue.Enqueue (edge);
                    Visited.Add (edge);
                    if (!Visited.Contains (edge.Leaf)) {
                        queue.Enqueue (edge.Leaf);
                        Visited.Add (edge.Leaf);
                    }
                }
            }

            while (queue.Count > 0) {
                var item = queue.Dequeue ();
                yield return new LevelItem<TItem> (item, default (TItem), 0);
                foreach (var edge in graph.Twig (item)) {
                    if (!Visited.Contains (edge)) {
                        Visited.Add (edge);
                        yield return new LevelItem<TItem> (edge, default (TItem), 0);
                    }
                }
            }
        }

        /// <summary>
        /// under construction; should be a more clear algo than CollapseWalk
        /// </summary>
        /// <param name="start"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public virtual IEnumerable<LevelItem<TItem>> CollapseWalk1 (TItem start, int level) {
            var queue = new Queue<TItem> ();
            queue.Enqueue (start);

            while (queue.Count > 0) {
                var item = queue.Dequeue ();
                yield return new LevelItem<TItem> (item, default (TItem), 0);
                foreach (var edge in graph.Twig (item)) {
                    if (!Visited.Contains (edge)) {

                        queue.Enqueue (edge);
                        Visited.Add (edge);

                        if (edge.Leaf.Equals (item)) {
                            continue;
                        }

                        if (graph.Adjacent (edge, item) != null) {
                            //if (!done.Contains(edge.Root)) {
                            //    queue.Enqueue(edge.Root);
                            //    done.Add(edge.Root);
                            //} 
                            if (!Visited.Contains (edge.Leaf)) {
                                queue.Enqueue (edge.Leaf);
                                Visited.Add (edge.Leaf);
                            }
                        }
                    }
                }
            }
        }

    }

    public static class Walk {

        public static Func<LevelItem<TItem>, bool> Leafs<TItem, TEdge> () where TEdge : IEdge<TItem>, TItem {
            return e => {
                var edge = e.Node is TEdge ? (TEdge)e.Node : default (TEdge);
                if (edge == null || e.Path == null)
                    return true;
                if (edge.Root.Equals (e.Path))
                    return true;
                var pathEdge = e.Path is TEdge ? (TEdge)e.Path : default (TEdge);
                if (pathEdge != null && pathEdge.Leaf.Equals (e.Node))
                    return true;
                return false;
            };

        }

        public static Func<LevelItem<TItem>, bool> Roots<TItem, TEdge> () where TEdge : IEdge<TItem>, TItem {
            return e => {
                var edge = e.Node is TEdge ? (TEdge)e.Node : default (TEdge);
                if (edge == null || e.Path == null)
                    return true;
                if (edge.Leaf.Equals (e.Path))
                    return true;
                var pathEdge = e.Path is TEdge ? (TEdge)e.Path : default (TEdge);
                if (pathEdge != null && pathEdge.Root.Equals (e.Node))
                    return true;
                return false;
            };

        }
    }

    public static class WalkerExtensions {

        public static IEnumerable<TItem> Items<TItem> (this IEnumerable<LevelItem<TItem>> items) {
            foreach (var item in items)
                yield return item.Node;

        }

        public static IEnumerable<TEdge> Edges<TItem, TEdge> (this IEnumerable<LevelItem<TItem>> items) where TEdge : IEdge<TItem>, TItem {
            foreach (var item in items)
                if (item.Node is TEdge)
                    yield return (TEdge)item.Node;

        }

        public static Walker<TItem, TEdge> Walk<TItem, TEdge> (this IGraph<TItem, TEdge> graph) where TEdge : IEdge<TItem>, TItem => new Walker<TItem, TEdge> (graph);

        /// <summary>
        /// node is edge and path is either root or leaf of node
        /// </summary>
        public static bool StepIsAdjacent<TItem> (this LevelItem<TItem> step) => (step.Node is IEdge<TItem>) && (((IEdge<TItem>)step.Node).Adjacent (step.Path) != null);

        /// <summary>
        /// node is edge and path is edge
        /// </summary>
        public static bool StepIsEdgeToEdge<TItem> (this LevelItem<TItem> step)  => (step.Node is IEdge<TItem>) && (step.Path is IEdge<TItem>);


    }

    public class LevelItemComparer<T> : Comparer<LevelItem<T>> {
        
        public virtual DataComparer<T> OrderBy { get; set; }

        public override int Compare (LevelItem<T> x, LevelItem<T> y) {
            if (x.Level == y.Level) {
                return OrderBy.Compare (x.Node, y.Node);
            }
            return x.Level.CompareTo (y.Level);
        }
    }


}