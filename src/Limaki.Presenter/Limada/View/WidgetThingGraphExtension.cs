/*
 * Limada 
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
using Limada.Model;
using Limada.Schemata;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Widgets;
using Limaki.Common;

namespace Limada.View {

    public static class WidgetThingGraphExtension {

        public static IThingGraph ThingGraph(this IGraph<IWidget, IEdgeWidget> graph) {
            IThingGraph result = null;

            var sourceGraph = new GraphPairFacade<IWidget, IEdgeWidget>().Source<IThing, ILink>(graph);


            if (sourceGraph != null && (sourceGraph.Two is IThingGraph)) {
                result = sourceGraph.Two as IThingGraph;
            }
            return result;
        }

        public static IThingFactory ThingFactory(this IGraph<IWidget, IEdgeWidget> graph) {
            IThingFactory result = null;
            var sourceGraph = new GraphPairFacade<IWidget, IEdgeWidget>().Source<IThing, ILink>(graph);

            if (sourceGraph != null) {
                var thingGraph = sourceGraph.Two as IThingGraph;
                var adapter = sourceGraph.Mapper.Adapter as WidgetThingAdapter;
                result = adapter.ThingFactory;
            }
            if (result == null)
                result = Registry.Factory.Create<IThingFactory>();
            return result;
        }

        public static IThing ThingOf(this IGraph<IWidget, IEdgeWidget> source, IWidget widget) {
            var graph = new GraphPairFacade<IWidget, IEdgeWidget>().Source<IThing, ILink>(source);
            if (widget != null && graph != null) {
                return  graph.Get (widget);
            }
            return null;
        }

        public static object Description(this IGraph<IWidget, IEdgeWidget> source, IWidget widget) {
            return source.ThingGraph().Description(source.ThingOf(widget));
        }

        public static object Description(this IGraph<IWidget, IEdgeWidget> source, IThing thing) {
            return source.ThingGraph().Description(thing);
        }

        public static bool ToggleFilterOnTwo(this IGraph<IWidget, IEdgeWidget> source) {
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
