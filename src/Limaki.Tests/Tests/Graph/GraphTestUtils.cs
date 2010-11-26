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
using System.Collections.Generic;
using System.Text;
using Limaki.Graphs;
using System.IO;
using Limaki.Common.Collections;

namespace Limaki.Tests.Graph {
    public class GraphTestUtils {

        public static string ReportItemAndEdges<TItem,TEdge>(TItem item, IEnumerable<TEdge> edges) {
            StringWriter report = new StringWriter();
            bool first = true;
            string emptyString = new string(' ', item.ToString().Length);
            foreach (TEdge edge in edges) {
                if (first) {
                    if (edge != null)
                        report.WriteLine("\t" + item.ToString() + "\t: " + edge.ToString());
                    else
                        report.WriteLine("\t" + item.ToString() + "\t: <null>");
                    first = false;
                } else {
                    report.WriteLine("\t" + emptyString + "\t: " + edge.ToString());
                }
            }
            return report.ToString ();
        }
        public static  string ReportGraph<TItem,TEdge>(IGraph<TItem, TEdge> graph, string reportHeader)
        where TEdge : IEdge<TItem> {
            StringWriter report = new StringWriter ();
            report.WriteLine(reportHeader);
            Set<TItem> done = new Set<TItem>();
            foreach (KeyValuePair<TItem, ICollection<TEdge>> kvp in graph.ItemsWithEdges()) {
                report.Write (ReportItemAndEdges<TItem, TEdge> (kvp.Key, kvp.Value));
                done.Add (kvp.Key);
            }
            foreach (TItem item in graph) {
                if (!done.Contains(item)) {
                    report.WriteLine ("\t" + item.ToString ());
                }
            }
            return report.ToString ();
        }
    }
}
