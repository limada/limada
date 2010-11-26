﻿/*
 * Limada 
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

using Limada.Model;
using Limada.Schemata;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Widgets;

namespace Limada.View {
    public class WidgetThingGraphExtension {
        public static IThingGraph GetThingGraph(IGraph<IWidget, IEdgeWidget> graph) {
            IThingGraph result = null;

            var sourceGraph = new GraphPairFacade<IWidget, IEdgeWidget>().Source<IThing, ILink>(graph);

            if (sourceGraph != null && (sourceGraph.Two is IThingGraph)) {
                result = sourceGraph.Two as IThingGraph;
            }
            return result;
        }

        public static ThingFactory GetThingFactory(IGraph<IWidget, IEdgeWidget> graph) {
            ThingFactory result = null;
            var sourceGraph = new GraphPairFacade<IWidget, IEdgeWidget>().Source<IThing, ILink>(graph);

            if (sourceGraph != null) {
                var thingGraph = sourceGraph.Two as IThingGraph;
                var adapter = sourceGraph.Mapper.Adapter as WidgetThingAdapter;
                result = adapter.ThingFactory;
            }
            return result;
        }

        public static IThing GetThing(IGraph<IWidget, IEdgeWidget> source, IWidget widget) {
            var graph = new GraphPairFacade<IWidget, IEdgeWidget>().Source<IThing, ILink>(source);
            if (widget != null && graph != null) {
                return  graph.Get (widget);
            }
            return null;
        }

        public static object GetDescription(IGraph<IWidget, IEdgeWidget> source, IWidget widget) {
            return ThingGraphUtils.GetDescription (WidgetThingGraphExtension.GetThingGraph (source), GetThing (source, widget));
        }

        public static object GetDescription(IGraph<IWidget, IEdgeWidget> source, IThing thing) {
            return ThingGraphUtils.GetDescription(WidgetThingGraphExtension.GetThingGraph(source), thing);
        }



        public static bool ToggleFilterOnTwo(IGraph<IWidget, IEdgeWidget> source) {
            bool result = false;
            var graph = new GraphPairFacade<IWidget, IEdgeWidget>()
                        .Source<IThing, ILink>(source);

            if (graph != null) {
                if (graph.Two is FilteredGraph<IThing, ILink>) {
                    graph.Two = ((FilteredGraph<IThing, ILink>)graph.Two).Source;
                } else {
                    graph.Two = new SchemaThingGraph(graph.Two as IThingGraph);
                }
                result = graph.Two is FilteredGraph<IThing, ILink>;
                graph.One.Clear();
            }
            return result;
        }
    }
}
