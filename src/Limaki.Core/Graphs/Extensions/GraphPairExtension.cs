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

namespace Limaki.Graphs.Extensions {
    public static class GraphPairExtension {
        public static IGraphPair<TItem, TItem, TEdge, TEdge> RootSource<TItem, TEdge>(this IGraph<TItem, TEdge> graph)
            where TEdge : IEdge<TItem>, TItem {
            var result = graph as IGraphPair<TItem, TItem, TEdge, TEdge>;
            if (result != null) {
                while (result.Two is IGraphPair<TItem, TItem, TEdge, TEdge>) {
                    result = (IGraphPair<TItem, TItem, TEdge, TEdge>)result.Two;
                }
            }
            return result;
        }
        

        public static IGraphPair<TItem, TTwo, TEdge, TEdgeTwo> Source<TItem, TEdge, TTwo, TEdgeTwo>(this IGraph<TItem, TEdge> graph)
            where TEdge : IEdge<TItem>, TItem
            where TEdgeTwo : IEdge<TTwo>, TTwo {

            var result = graph as IGraphPair<TItem, TItem, TEdge, TEdge>;

            if (result != null) {
                result = graph.RootSource();

                if (result != null && result.Two is IGraphPair<TItem, TTwo, TEdge, TEdgeTwo>) {
                    return (IGraphPair<TItem, TTwo, TEdge, TEdgeTwo>)result.Two;
                }
                if (result != null && result is IGraphPair<TItem, TTwo, TEdge, TEdgeTwo>) {
                    return (IGraphPair<TItem, TTwo, TEdge, TEdgeTwo>)result;
                }
            } else if (graph is IGraphPair<TItem, TTwo, TEdge, TEdgeTwo>) {
                return (IGraphPair<TItem, TTwo, TEdge, TEdgeTwo>)graph;
            }
            return null;
        }



        public static IEnumerable<IGraph<TItem, TEdge>> Graphs<TItem, TEdge>(this IGraph<TItem, TEdge> graph)
        where TEdge : IEdge<TItem>, TItem {
            var result = graph as IGraphPair<TItem, TItem, TEdge, TEdge>;
            if (result == null)
                yield return graph;
            else {
                while (result is IGraphPair<TItem, TItem, TEdge, TEdge>) {
                    yield return result.One;
                    if (result.Two is IGraphPair<TItem, TItem, TEdge, TEdge>) {
                        result = (IGraphPair<TItem, TItem, TEdge, TEdge>)result.Two;
                    } else {
                        if (result is IBaseGraphPair<TItem, TEdge>) {
                            yield return ((IBaseGraphPair<TItem, TEdge>)result).One;
                        } else {
                            yield return result.Two;
                        }
                        yield break;
                    }
                }
            }
        }



        public static TItem LookUp<TItem, TEdge, TTwo, TEdgeTwo>(this IGraphPair<TItem, TItem, TEdge, TEdge> graphPair1,
            IGraphPair<TItem, TItem, TEdge, TEdge> graphPair2,
            TItem item)
            where TEdge : IEdge<TItem>, TItem
            where TEdgeTwo : IEdge<TTwo>, TTwo {
            var back = default(TItem);
            var source1 = graphPair1.Source<TItem, TEdge, TTwo, TEdgeTwo>();
            var source2 = graphPair2.Source<TItem, TEdge, TTwo, TEdgeTwo>();
            if (source1 != null && source2 != null) {
                var ping = source1.Get(item);
                if (ping != null) {
                    back = source2.Get(ping);
                }
            }
            return back;
        }

        public static int InEdgeCount<TItem, TEdge>(this IGraph<TItem, TEdge> graph, TItem item)
        where TEdge : IEdge<TItem>, TItem {
            return graph.Edges(item).Count(edge => edge.Leaf.Equals(item));
        }

        public static IEnumerable<TItem> FindRoots<TItem, TEdge>(this IGraph<TItem, TEdge> graph, TItem focused)
        where TEdge : IEdge<TItem>, TItem {
            if (graph != null) {
                var graphroots = new List<Pair<TItem, int>>();
                var walker = new Walker<TItem, TEdge>(graph);

                var focusedroot = default(Pair<TItem, int>);

                foreach (var item in graph) {
                    if (!walker.visited.Contains(item)) {
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

        public static void PopulateWithRoots<TItem, TEdge, TTwo, TEdgeTwo>(this GraphView<TItem, TEdge> graphView)
            where TEdge : IEdge<TItem>, TItem
            where TEdgeTwo : IEdge<TTwo>, TTwo {
            IGraphPair<TItem, TTwo, TEdge, TEdgeTwo> source = graphView.Source<TItem, TEdge, TTwo, TEdgeTwo>();
            if (source != null) {
                foreach (TTwo item in source.Two.FindRoots<TTwo, TEdgeTwo>(default(TTwo))) {
                    graphView.One.Add(source.Get(item));
                }
            }

        }
    }

   
}
