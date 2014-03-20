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
using System.Linq;
using Limaki.Common;

namespace Limaki.Graphs {

    public static class GraphPairExtension {

        /// <summary>
        /// iterates through a GraphPair tree 
        /// while graph is GraphPair TItem, TItem, TEdge, TEdge
        ///     result = result.Source
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TEdge"></typeparam>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static IGraphPair<TItem, TItem, TEdge, TEdge> 
            RootSource<TItem, TEdge>(this IGraph<TItem, TEdge> graph) where TEdge : IEdge<TItem>, TItem {

            var result = graph as IGraphPair<TItem, TItem, TEdge, TEdge>;
            if (result != null) {
                while (result.Source is IGraphPair<TItem, TItem, TEdge, TEdge>) {
                    result = (IGraphPair<TItem, TItem, TEdge, TEdge>)result.Source;
                }
            }
            return result;
        }

        /// <summary>
        /// iterates through a GraphPair tree 
        /// while graph is GraphPair TItem, TItem, TEdge, TEdge
        ///     result = result.Source
        /// then gives back GraphPair TSinkItem, TSourceItem, TSinkEdge, TSourceEdge
        ///     if result.Source or result
        /// </summary>
        public static IGraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> 
            Source<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> 
            (this IGraph<TSinkItem, TSinkEdge> graph) where TSinkEdge : IEdge<TSinkItem>, TSinkItem where TSourceEdge : IEdge<TSourceItem>, TSourceItem {

            var result = graph as IGraphPair<TSinkItem, TSinkItem, TSinkEdge, TSinkEdge>;

            if (result != null) {
                result = graph.RootSource();

                if (result != null && result.Source is IGraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>) {
                    return (IGraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>)result.Source;
                }
                if (result != null && result is IGraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>) {
                    return (IGraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>)result;
                }
            } else if (graph is IGraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>) {
                return (IGraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>)graph;
            }
            return null;
        }


        /// <summary>
        /// iterates through a GraphPair tree 
        /// while graph is GraphPair TItem, TItem, TEdge, TEdge
        ///     root = root.Source
        /// returns root.Source as ISinkGraph (sink part of a GraphPair)
        /// </summary>
        public static ISinkGraph<TSinkItem, TSinkEdge> 
            RootSink<TSinkItem, TSinkEdge> (this IGraph<TSinkItem, TSinkEdge> graph) where TSinkEdge : IEdge<TSinkItem>, TSinkItem {

            var root = graph.RootSource ();
            if (root != null)
                return root.Source as ISinkGraph<TSinkItem, TSinkEdge>;
            return null;
        }

        /// <summary>
        /// give back the generic type parameters of graph
        /// if graph is a <see cref="IGraphPair{TSinkItem, TSourceItem, TSinkEdge, TSourceEdge}"/> 
        /// </summary>
        /// <typeparam name="TSinkItem"></typeparam>
        /// <typeparam name="TSinkEdge"></typeparam>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static Type[]
           GraphPairTypes<TSinkItem, TSinkEdge> (this IGraph<TSinkItem, TSinkEdge> graph) where TSinkEdge : IEdge<TSinkItem>, TSinkItem {

            if (graph == null)
                return null;
            var graphPairType = graph.GetType().GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof (IGraphPair<,,,>)).FirstOrDefault();
            if (graphPairType != null)
                return graphPairType.GetGenericArguments();
            return null;
        }

        public static IEnumerable<IGraph<TItem, TEdge>>
            Graphs<TItem, TEdge> (this IGraph<TItem, TEdge> graph) where TEdge : IEdge<TItem>, TItem {

            // TODO: don't ingnore IWrappedGraph

            var result = graph as IGraphPair<TItem, TItem, TEdge, TEdge>;
            if (result == null)
                yield return graph;
            else {
                while (result is IGraphPair<TItem, TItem, TEdge, TEdge>) {
                    yield return result.Sink;
                    if (result.Source is IGraphPair<TItem, TItem, TEdge, TEdge>) {
                        result = (IGraphPair<TItem, TItem, TEdge, TEdge>)result.Source;
                    } else {
                        if (result is ISinkGraph<TItem, TEdge>) {
                            yield return ((ISinkGraph<TItem, TEdge>)result).Sink;
                        } else {
                            yield return result.Source;
                        }
                        yield break;
                    }
                }
            }
        }


        public static TItem 
            LookUp<TItem, TEdge, TSourceItem, TSourceEdge> (
                this IGraphPair<TItem, TItem, TEdge, TEdge> graphPair1,
                IGraphPair<TItem, TItem, TEdge, TEdge> graphPair2,
                TItem item) where TEdge : IEdge<TItem>, TItem
                            where TSourceEdge : IEdge<TSourceItem>, TSourceItem {

            var back = default(TItem);
            var source1 = graphPair1.Source<TItem, TEdge, TSourceItem, TSourceEdge>();
            var source2 = graphPair2.Source<TItem, TEdge, TSourceItem, TSourceEdge>();
            if (source1 != null && source2 != null) {
                var ping = source1.Get(item);
                if (ping != null) {
                    var contains = ping is TSourceEdge ? source2.Source.Contains((TSourceEdge)ping) : source2.Source.Contains(ping);
                    if (contains)
                        back = source2.Get(ping);
                }
            }
            return back;
        }

        public static int
            InEdgeCount<TItem, TEdge> (
                this IGraph<TItem, TEdge> graph, TItem item) where TEdge : IEdge<TItem>, TItem {

            return graph.Edges (item).Count (edge => edge.Leaf.Equals (item));
        }

        public static IEnumerable<TItem>
            FindRoots<TItem, TEdge> (this IGraph<TItem, TEdge> graph, TItem focused) where TEdge : IEdge<TItem>, TItem {

            if (graph != null) {
                var graphroots = new List<Pair<TItem, int>>();
                var walker = new Walker<TItem, TEdge>(graph);

                var focusedroot = default(Pair<TItem, int>);

                foreach (var item in graph) {
                    if (!walker.Visited.Contains(item)) {
                        bool selected = false;
                        int count = 0;
                        var root = new Pair<TItem, int>(item, int.MaxValue);
                        foreach (var levelItem in walker.DeepWalk(item, 0)) {
                            if (!(levelItem.Node is IEdge<TItem>)) {
                                count = graph.InEdgeCount(levelItem.Node);
                                if (root.Two > count) {
                                    root = new Pair<TItem, int>((TItem)levelItem.Node, count);
                                }
                            }
                            //else { // this is a failed attempt to avoid InEdgeCount
                            //    TEdge edge = (TEdge) levelItem.Node;
                            //    if (edge.Leaf.Equals(levelItem.Path)) {
                            //        count++;
                            //    }

                            //}
                            if (levelItem.Node.Equals(focused)) {
                                selected = true;
                            }
                        }
                        if (selected) {
                            root = new Pair<TItem, int>(focused, 0);
                            focusedroot = root;
                        }
                        graphroots.Add(root);
                    }
                }
                Comparison<Pair<TItem, int>> comparision = (a, b) => -a.Two.CompareTo(b.Two);
                graphroots.Sort(comparision);


                if (graphroots.Count > 0) {
                    if (focusedroot.One == null) {
                        focusedroot = graphroots[0];
                    }
                    yield return focusedroot.One;
                }
                foreach (var item in graphroots) {
                    if (!item.Equals(focusedroot))
                        yield return item.One;
                }
            }

        }

        public static void
            PopulateWithRoots<TItem, TEdge, TSourceItem, TSourceEdge> (
                this SubGraph<TItem, TEdge> subGraph)
                    where TEdge : IEdge<TItem>, TItem where TSourceEdge : IEdge<TSourceItem>, TSourceItem {

            var source = subGraph.Source<TItem, TEdge, TSourceItem, TSourceEdge> ();
            if (source != null) {
                foreach (var item in source.Source.FindRoots<TSourceItem, TSourceEdge> (default (TSourceItem))) {
                    subGraph.Sink.Add (source.Get (item));
                }
            }

        }

        public static TSinkItem
            SinkItemOf<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> (
                this IGraph<TSinkItem, TSinkEdge> sinkGraph, TSourceItem sourceItem)
                        where TSinkEdge : IEdge<TSinkItem>, TSinkItem where TSourceEdge : IEdge<TSourceItem>, TSourceItem {
            
            var graph = sinkGraph.Source<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> ();
            if (sourceItem != null && graph != null) {
                return graph.Get (sourceItem);
            }
            return default (TSinkItem);
        }

        public static bool 
            ContainsSinkItemOf<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> (
                this IGraph<TSinkItem, TSinkEdge> sinkGraph, TSourceItem sourceItem)
                    where TSinkEdge : IEdge<TSinkItem>, TSinkItem where TSourceEdge : IEdge<TSourceItem>, TSourceItem {
            
            var graph = sinkGraph.Source<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> ();
            if (sourceItem != null && graph != null) {
                return graph.Source2Sink.ContainsKey (sourceItem);
            }
            return false;
        }

        public static TSourceItem 
            SourceItemOf<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> (
                this IGraph<TSinkItem, TSinkEdge> sinkGraph, 
                TSinkItem sinkItem)
                    where TSinkEdge : IEdge<TSinkItem>, TSinkItem where TSourceEdge : IEdge<TSourceItem>, TSourceItem {
            
            var graph = sinkGraph.Source<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> ();
            if (sinkItem != null && graph != null) {
                return graph.Get (sinkItem);
            }

            return default (TSourceItem);
        }
    }

   
}
