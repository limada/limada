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
 * http://www.limada.org
 * 
 */

using Limada.Model;
using Limada.Schemata;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Common;
using Limaki.Visuals;

namespace Limada.VisualThings {
    /// <summary>
    /// extensions of VisualGraphs backed by ThingGraphs
    /// </summary>
    public static class VisualThingGraphExtension {

        public static IThingGraph ThingGraph(this IGraph<IVisual, IVisualEdge> graph) {
            IThingGraph result = null;

            var sourceGraph = graph.Source<IVisual, IVisualEdge, IThing, ILink>();

            if (sourceGraph != null && (sourceGraph.Source is IThingGraph)) {
                result = sourceGraph.Source as IThingGraph;
            }
            return result;
        }

        public static IThingFactory ThingFactory(this IGraph<IVisual, IVisualEdge> graph) {
            IThingFactory result = null;
            var sourceGraph = graph.Source<IVisual, IVisualEdge, IThing, ILink>();

            if (sourceGraph != null) {
                var thingGraph = sourceGraph.Source as IThingGraph;
                var adapter = sourceGraph.Mapper.Transformer as VisualThingTransformer;
                result = adapter.ThingFactory;
            }
            if (result == null)
                result = Registry.Factory.Create<IThingFactory>();
            return result;
        }

        public static IThing ThingOf(this IGraph<IVisual, IVisualEdge> source, IVisual visual) {
            return source.SourceItemOf<IVisual, IVisualEdge, IThing, ILink> (visual);
        }

        public static IThing ThingToDisplay (this IGraph<IThing, ILink> graph, IThing thing) {
            if (graph is SchemaThingGraph) {
                return ((SchemaThingGraph) graph).ThingToDisplay(thing);
            } else {
                return thing;
            }
        }

        public static IVisual VisualOf(this IGraph<IVisual, IVisualEdge> source, IThing thing) {
            return source.SinkItemOf<IVisual, IVisualEdge, IThing, ILink> (thing);
        }

        public static bool ContainsVisualOf (this IGraph<IVisual, IVisualEdge> source, IThing thing) {
            return source.ContainsSinkItemOf<IVisual, IVisualEdge, IThing, ILink> (thing);
        }

        public static object Description(this IGraph<IVisual, IVisualEdge> source, IVisual visual) {
            return source.ThingGraph().Description(source.ThingOf(visual));
        }

        public static object Description(this IGraph<IVisual, IVisualEdge> source, IThing thing) {
            return source.ThingGraph().Description(thing);
        }

        public static bool ToggleFilterOnTwo(this IGraph<IVisual, IVisualEdge> source) {
            bool result = false;
            var graph = source.Source<IVisual, IVisualEdge, IThing, ILink>();

            if (graph != null) {
                if (graph.Source is FilteredGraph<IThing, ILink>) {
                    graph.Source = ((FilteredGraph<IThing, ILink>)graph.Source).Source;
                } else {
                    graph.Source = new SchemaThingGraph(graph.Source as IThingGraph);
                }
                result = graph.Source is FilteredGraph<IThing, ILink>;
                graph.Sink.Clear();
            }
            return result;
        }
    }
}
