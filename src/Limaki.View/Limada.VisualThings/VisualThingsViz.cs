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
using Limaki.Visuals;

namespace Limada.Usecases {

    /// <summary>
    /// helper class to manage VisualGraphs backed by ThingGraphs
    /// </summary>
    public class VisualThingsViz {

        public void DependencyVisitor (GraphCursor<IVisual, IVisualEdge> source, Action<IVisual> visit, GraphEventType eventType) {

            var things = new Queue<IThing> ();

            var dependencies = Registry.Pool.TryGetCreate<GraphDepencencies<IThing, ILink>> ();
            dependencies.VisitItems (
                GraphCursor.Create (source.Graph.ThingGraph (), source.Graph.ThingOf (source.Cursor)),
                t => things.Enqueue (t), eventType);

            things
                .Where (t => source.Graph.ContainsVisualOf (t))
                .Select (t => source.Graph.VisualOf (t))
                .ForEach (visit);
        }


    }
}