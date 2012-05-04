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
using Limaki.Graphs.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Limaki.Graphs {
    public static class GraphExtensions {
        public static string EdgeString<TItem, TEdge>( TEdge edge ) 
            where TEdge : IEdge<TItem> {
            string root = string.Empty;
            string leaf = string.Empty;
            if (edge.Root is TEdge) {
                root = EdgeString<TItem, TEdge>((TEdge)(object)edge.Root);
            } else {
                root = edge.Root.ToString();
            }
            if (edge.Leaf is TEdge) {
                leaf = EdgeString<TItem, TEdge>((TEdge)(object)edge.Leaf);
            } else {
                leaf = edge.Leaf.ToString();
            }
            return String.Format("[{0}->{1}]", root, leaf);
        }

        public static void MergeInto<TItem, TEdge>(this IGraph<TItem, TEdge> source, IGraph<TItem, TEdge> target)
        where TEdge:IEdge<TItem>,TItem { source.MergeInto(target, null, null,null); }

        public static void MergeInto<TItem, TEdge>(this IGraph<TItem, TEdge> source, IGraph<TItem, TEdge> target, Action<TItem> message)
        where TEdge : IEdge<TItem>, TItem { source.MergeInto(target, null, null, message); }

        public static void MergeInto<TItem, TEdge>(this IGraph<TItem, TEdge> source, IGraph<TItem, TEdge> target,
                                                     Func<TItem, bool> whereItem, Func<TEdge, bool> whereEdge, Action<TItem> message) 
        where TEdge:IEdge<TItem>,TItem {
            if (source != null && target != null) {
                Func<TEdge, bool> checkEdge = e => {
                    var result = source.ValidEdge(e);
                    // TODO: show warning here
                    return result;
                };
                Func<TItem, bool> checkItem = i => {
                    TEdge e = default(TEdge);
                    if(i is TEdge) e = (TEdge)i;
                    var result = e==null || checkEdge(e);
                    return result;                                                  
                };
                Action<TItem> state = i => {
                    if (message != null)
                        message(i);
                };
                Walker<TItem, TEdge> walker = new Walker<TItem, TEdge>(source);
                foreach (TItem item in source) {
                    if (!walker.visited.Contains(item)) {
                        state(item);
                        if (checkItem(item) && (whereItem == null || whereItem(item))) {
                            target.Add(item);
                        }
                        foreach (LevelItem<TItem> levelItem in walker.DeepWalk(item, 0)) {
                            state(levelItem.Node);
                            if (levelItem.Node is TEdge) {
                                var edge = (TEdge)levelItem.Node;
                                if (checkEdge(edge) && (whereEdge == null || whereEdge(edge))) {
                                    target.Add(edge);
                                }

                            } else
                                if (checkItem(levelItem.Node) && (whereItem == null || whereItem(levelItem.Node))) {
                                    target.Add(levelItem.Node);
                                }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// items and edges
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TEdge"></typeparam>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static IEnumerable<TItem> Elements<TItem, TEdge>(this IGraph<TItem, TEdge> graph) 
        where TEdge:IEdge<TItem>,TItem {
            foreach(var item in graph) {
                if (!(item is TEdge))
                    yield return item;
            }
            foreach (var item in graph.Edges()) {
                    yield return item;
            }
        }
    }

}
