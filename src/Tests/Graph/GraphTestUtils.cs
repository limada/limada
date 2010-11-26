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
using System.Text;
using Limaki.Graphs;
using System.IO;
using Limaki.Common.Collections;

namespace Limaki.Tests.Graph {
    public class GraphTestUtils {
        public static  string ReportGraph<TItem,TEdge>(IGraph<TItem, TEdge> graph, string reportHeader)
        where TEdge : IEdge<TItem> {
            StringWriter report = new StringWriter ();
            report.WriteLine(reportHeader);
            Set<TItem> done = new Set<TItem>();
            foreach (KeyValuePair<TItem, ICollection<TEdge>> kvp in graph.ItemsWithEdges()) {
                bool first = true;
                string emptyString = new string(' ', kvp.Key.ToString().Length);
                foreach (TEdge edge in kvp.Value) {
                    if (first) {
                        if (edge != null)
                            report.WriteLine("\t" + kvp.Key.ToString() + "\t: " + edge.ToString());
                        else
                            report.WriteLine("\t" + kvp.Key.ToString() + "\t: <null>");
                        first = false;
                    } else {
                        report.WriteLine("\t" + emptyString + "\t: " + edge.ToString());
                    }
                }
                done.Add (kvp.Key);
            }
            foreach (TItem item in graph) {
                if (!done.Contains(item)) {
                    report.WriteLine ("\t" + item.ToString ());
                }
            }
            return report.ToString ();
        }

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
                root = edge.Leaf.ToString();
            }
            return String.Format("[{0}->{1}]", root, leaf);

        }
    }
}
