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
 */


using System;
using System.Collections.Generic;
using System.Linq;
using Limada.Model;
using Limada.VisualThings;
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Visuals;

namespace Limada.Usecases {

    /// <summary>
    /// helper class to manage VisualGraphs backed by ThingGraphs
    /// </summary>
    public class VisualThingsViz {

        public void DependencyVisitor (GraphCursor<IVisual, IVisualEdge> sink, Action<IVisual> visit, GraphEventType eventType) {

            var rs = sink.Graph.Source<IVisual, IVisualEdge, IThing, ILink> ();
            if (rs == null)
                return;
            var sg = rs.Source;
            var sourceItem = sink.Graph.SourceItemOf<IVisual, IVisualEdge, IThing, ILink> (sink.Cursor);
            if (sg == null || sourceItem == null)
                return;

            var things = new Queue<IThing> ();

            var dependencies = Registry.Pool.TryGetCreate<GraphDepencencies<IThing, ILink>> ();
            dependencies.VisitItems (
                GraphCursor.Create (sink.Graph.ThingGraph(), sink.Graph.ThingOf (sink.Cursor)),
                t => things.Enqueue (t), eventType);

            things
                .Where (t => sink.Graph.ContainsVisualOf (t))
                .Select (t => sink.Graph.VisualOf (t))
                .ForEach (visit);
        }


    }
}