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
using Limaki.Common;
using Limaki.View.Visuals;

namespace Limada.View.VisualThings {
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
            } 
            return thing;
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

        /// <summary>
        /// if source of sink is wrapped, then unwrap it
        /// if not, then wrap it with a schemagraph
        /// </summary>
        /// <param name="sink"></param>
        /// <returns></returns>
        public static bool ToggleFilterOnSource(this IGraph<IVisual, IVisualEdge> sink) {
            bool result = false;
            var graphPair = sink.Source<IVisual, IVisualEdge, IThing, ILink>();

            if (graphPair != null) {
                if (graphPair.Source is IWrappedGraph<IThing, ILink>) {
                    graphPair.Source = graphPair.Source.Unwrap();
                } else {
                    graphPair.Source = new SchemaThingGraph(graphPair.Source as IThingGraph);
                }
                result = graphPair.Source is IWrappedGraph<IThing, ILink>;
                graphPair.Sink.Clear();
            }
            return result;
        }
    }
}
