/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using Limada.Model;
using Limada.Schemata;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Visuals;
using Limaki.Common;

namespace Limada.View {

    public static class VisualThingGraphExtension {

        public static IThingGraph ThingGraph(this IGraph<IVisual, IVisualEdge> graph) {
            IThingGraph result = null;

            var sourceGraph = GraphPairExtension<IVisual, IVisualEdge>.Source<IThing, ILink>(graph);


            if (sourceGraph != null && (sourceGraph.Two is IThingGraph)) {
                result = sourceGraph.Two as IThingGraph;
            }
            return result;
        }

        public static IThingFactory ThingFactory(this IGraph<IVisual, IVisualEdge> graph) {
            IThingFactory result = null;
            var sourceGraph = GraphPairExtension<IVisual, IVisualEdge>.Source<IThing, ILink>(graph);

            if (sourceGraph != null) {
                var thingGraph = sourceGraph.Two as IThingGraph;
                var adapter = sourceGraph.Mapper.Adapter as VisualThingAdapter;
                result = adapter.ThingFactory;
            }
            if (result == null)
                result = Registry.Factory.Create<IThingFactory>();
            return result;
        }

        public static IThing ThingOf(this IGraph<IVisual, IVisualEdge> source, IVisual visual) {
            var graph = GraphPairExtension<IVisual, IVisualEdge>.Source<IThing, ILink>(source);
            if (visual != null && graph != null) {
                return  graph.Get (visual);
            }
            return null;
        }

        public static IVisual VisualOf(this IGraph<IVisual, IVisualEdge> source, IThing thing) {
            var graph = GraphPairExtension<IVisual, IVisualEdge>.Source<IThing, ILink>(source);
            if (thing != null && graph != null) {
                return graph.Get(thing);
            }
            return null;
        }
        public static object Description(this IGraph<IVisual, IVisualEdge> source, IVisual visual) {
            return source.ThingGraph().Description(source.ThingOf(visual));
        }

        public static object Description(this IGraph<IVisual, IVisualEdge> source, IThing thing) {
            return source.ThingGraph().Description(thing);
        }

        public static bool ToggleFilterOnTwo(this IGraph<IVisual, IVisualEdge> source) {
            bool result = false;
            var graph = GraphPairExtension<IVisual, IVisualEdge>
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
