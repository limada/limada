/*
 * Limaki 
 * Version 0.081
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
using Limaki.Graphs.Extensions;
using System.Collections;
using System.Collections.Generic;

namespace Limaki.Graphs {
    public class GraphUtils {
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

        public static void MergeGraphs<TItem, TEdge>(IGraph<TItem, TEdge> source, IGraph<TItem, TEdge> target)
        where TEdge:IEdge<TItem>,TItem { MergeGraphs<TItem, TEdge>(source, target, null, null); }

        public static void MergeGraphs<TItem, TEdge>(IGraph<TItem, TEdge> source, IGraph<TItem, TEdge> target,
                                                     Func<TItem, bool> whereItem, Func<TEdge, bool> whereEdge) 
        where TEdge:IEdge<TItem>,TItem {
            if (source != null && target != null) {
                Walker<TItem, TEdge> walker = new Walker<TItem, TEdge>(source);
                foreach (TItem item in source) {
                    if (!walker.visited.Contains(item)) {
                        if (whereItem ==null || whereItem(item))
                            target.Add(item);
                        foreach (LevelItem<TItem> levelItem in walker.DeepWalk(item, 0)) {
                            if (levelItem.Node is TEdge) {
                                var edge = (TEdge)levelItem.Node;
                                if (whereEdge == null || whereEdge(edge))
                                    target.Add(edge);
                            } else
                                if (whereItem == null || whereItem(levelItem.Node))
                                    target.Add(levelItem.Node);
                        }
                    }
                }
            }
        }

        public static IEnumerable<TItem> Elements<TItem, TEdge>(IGraph<TItem, TEdge> graph) 
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
