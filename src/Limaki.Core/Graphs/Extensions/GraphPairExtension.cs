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
 * http://limada.sourceforge.net
 * 
 */


using System;
using System.Collections.Generic;
using Limaki.Common;

namespace Limaki.Graphs.Extensions {
    public static class GraphPairExtension<TItem, TEdge>
        where TEdge : IEdge<TItem>, TItem {

        //public IGraphPair<TItem, TTwo, TEdge, TEdgeTwo> Source<TTwo, TEdgeTwo>(IGraphPair<TItem, TItem, TEdge, TEdge> graphPair)
        //    where TEdgeTwo : IEdge<TTwo>, TTwo {
        //    IGraphPair<TItem, TItem, TEdge, TEdge> result = this.Source(graphPair);
        //    if (result != null && result.Two is IGraphPair<TItem, TTwo, TEdge, TEdgeTwo>) {
        //        return (IGraphPair<TItem, TTwo, TEdge, TEdgeTwo>)result.Two;
        //    }
        //    if (result != null && result is IGraphPair<TItem, TTwo, TEdge, TEdgeTwo>) {
        //        return (IGraphPair<TItem, TTwo, TEdge, TEdgeTwo>)result;
        //    }
        //    return null;
        //}

        public static IGraphPair<TItem, TTwo, TEdge, TEdgeTwo> Source<TTwo, TEdgeTwo>(IGraph<TItem, TEdge> graph)
            where TEdgeTwo : IEdge<TTwo>, TTwo {
            
            var result = graph as IGraphPair<TItem, TItem, TEdge, TEdge>;
            
            if (result != null) {
                result = GraphPairExtension<TItem, TEdge>.Source(graph);

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

        //public IGraphPair<TItem, TItem, TEdge, TEdge> Source(IGraphPair<TItem, TItem, TEdge, TEdge> graphPair) {

        //    IGraphPair<TItem, TItem, TEdge, TEdge> result = graphPair;
        //    if (result != null)
        //        while (result.Two is IGraphPair<TItem, TItem, TEdge, TEdge>) {
        //            result = (IGraphPair<TItem, TItem, TEdge, TEdge>)result.Two;
        //        }
        //    return result;
        //}

        public static IGraphPair<TItem, TItem, TEdge, TEdge> Source(IGraph<TItem, TEdge> graph) {
            var result = graph as IGraphPair<TItem, TItem, TEdge, TEdge>;
            if (result != null) {
                while (result.Two is IGraphPair<TItem, TItem, TEdge, TEdge>) {
                    result = (IGraphPair<TItem, TItem, TEdge, TEdge>)result.Two;
                }
            }
            return result;
        }

        public static IEnumerable<IGraph<TItem, TEdge>> Graphs(IGraph<TItem, TEdge> graph) {
            IGraphPair<TItem, TItem, TEdge, TEdge> result = 
                graph as IGraphPair<TItem, TItem, TEdge, TEdge>;
            if (result == null)
                yield return graph;
            else {
                while (result is IGraphPair<TItem, TItem, TEdge, TEdge>) {
                    yield return result.One;
                    if (result.Two is IGraphPair<TItem, TItem, TEdge, TEdge>) {
                        result = (IGraphPair<TItem, TItem, TEdge, TEdge>) result.Two;
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



        public static TItem LookUp<TTwo, TEdgeTwo>(
            IGraphPair<TItem, TItem, TEdge, TEdge> graphPair1,
            IGraphPair<TItem, TItem, TEdge, TEdge> graphPair2,
            TItem item)
            where TEdgeTwo : IEdge<TTwo>, TTwo {
            TItem back = default(TItem);
            IGraphPair<TItem, TTwo, TEdge, TEdgeTwo> source1 = Source<TTwo, TEdgeTwo>(graphPair1);
            IGraphPair<TItem, TTwo, TEdge, TEdgeTwo> source2 = Source<TTwo, TEdgeTwo>(graphPair2);
            if (source1 != null && source2 != null) {
                TTwo ping = source1.Get(item);
                if (ping != null) {
                    back = source2.Get(ping);
                }
            }
            return back;
        }

        public static int InEdgeCount(IGraph<TItem, TEdge> graph, TItem item) {
            int result = 0;
            foreach(TEdge edge in graph.Edges(item)) {
                if (edge.Leaf.Equals(item))
                    result++;
            }
            return result;
        }

        public static IEnumerable<TItem> FindRoots(IGraph<TItem, TEdge> graph, TItem focused) {
            if (graph != null) {
                List<Pair<TItem, int>> graphroots = new List<Pair<TItem, int>>();
                Walker<TItem, TEdge> walker = new Walker<TItem, TEdge>(graph);

                Pair<TItem, int> focusedroot = default(Pair<TItem, int>);
                
                foreach (TItem item in graph) {
                    if (!walker.visited.Contains(item)) {
                        bool selected = false;
                        int count = 0;
                        Pair<TItem, int> root = new Pair<TItem, int>(item, int.MaxValue);
                        foreach (LevelItem<TItem> levelItem in walker.DeepWalk(item, 0)) {
                            if (!(levelItem.Node is IEdge<TItem>)) {
                                count = InEdgeCount(graph, levelItem.Node);
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
                Comparison<Pair<TItem, int>> comparision = delegate(Pair<TItem, int> a, Pair<TItem, int> b) {
                    return -a.Two.CompareTo(b.Two);
                };
                graphroots.Sort(comparision);


                if (graphroots.Count > 0) {
                    if (focusedroot.One == null) {
                        focusedroot = graphroots[0];
                    }
                    yield return focusedroot.One;
                }
                foreach (Pair<TItem, int> item in graphroots) {
                    if (!item.Equals(focusedroot))
                        yield return item.One;
                }
            }

        }

        public static void PopulateWithRoots<TTwo, TEdgeTwo>(GraphView<TItem, TEdge> graphView)
            where TEdgeTwo : IEdge<TTwo>, TTwo {
            IGraphPair<TItem, TTwo, TEdge, TEdgeTwo> source = Source<TTwo, TEdgeTwo>(graphView);
            if (source != null) {
                foreach (TTwo item in GraphPairExtension<TTwo, TEdgeTwo>.FindRoots(source.Two, default(TTwo))) {
                    graphView.One.Add(source.Get(item));
                }
            }

        }
    }
}
