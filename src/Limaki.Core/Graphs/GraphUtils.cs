/*
 * Limaki 
 * Version 0.08
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
        where TEdge:IEdge<TItem>,TItem {
            if (source != null && target != null) {
                Walker<TItem, TEdge> walker = new Walker<TItem, TEdge>(source);
                foreach (TItem item in source) {
                    if (!walker.visited.Contains(item)) {
                        target.Add (item);
                        foreach (LevelItem<TItem> levelItem in walker.DeepWalk(item, 0)) {
                            if (levelItem.Node is TEdge)
                                target.Add((TEdge)levelItem.Node);
                            else
                                target.Add(levelItem.Node);
                        }
                    }
                }
            }
        }
    }

}
