/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limada.Model;
using Limada.Schemata;
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Contents;
using Limaki.Data;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Limada.UseCases {

    public class ThingGraphUseCases {

        public IEnumerable<IThing> DateTimesOf (IGraph<IThing, ILink> graph) {
            graph = graph.Unwrap();
            foreach (var th in graph.Yield().Where (t => t is IThing<string> && t.Data.ToString () != null)) {
                var d = default (DateTime);
                if (DateTime.TryParse (th.Data.ToString (), out d))
                    yield return th;
            }

        }

        public IEnumerable<IThing> TimeLine (IGraph<IThing, ILink> graph) {
            var schemaGraph = graph as SchemaThingGraph;
            graph = graph.Unwrap ();
            var done = new HashSet<IThing> ();
            foreach (var t in DateTimesOf (graph)
                .OrderBy (t => DateTime.Parse (t.Data.ToString ()))) {
                    if (!done.Contains (t)) {
                        yield return t;
                        done.Add (t);
                    }
            }

        }
    }
}