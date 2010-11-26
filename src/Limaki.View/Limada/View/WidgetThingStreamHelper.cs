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
using System.IO;
using Limada.Model;
using Limada.View;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model.Streams;
using Limaki.Widgets;
using Limada.Schemata;

namespace Limada.View {
    public class WidgetThingStreamHelper {
        public virtual IWidget CreateFromStream( IGraph<IWidget, IEdgeWidget> graph, StreamInfo<Stream> streamInfo ) {
            
            IWidget result = null;
            var sourceGraph =
                new GraphPairFacade<IWidget, IEdgeWidget>().Source<IThing, ILink>(graph);

            if (sourceGraph != null) {
                var thingGraph = WidgetThingGraphExtension.GetThingGraph(graph);
                var factory = WidgetThingGraphExtension.GetThingFactory (graph);
                
                IThing thing = new ThingStreamFacade(factory).SetStream(thingGraph, null, streamInfo);
                
                result = sourceGraph.Get(thing);
            }
            return result;
        }

        public IThing SetStream(IGraph<IWidget,IEdgeWidget> graph, IThing thing, StreamInfo<Stream> streamInfo) {
            var thingGraph = WidgetThingGraphExtension.GetThingGraph(graph);
            var factory = WidgetThingGraphExtension.GetThingFactory (graph);
            var streamThing = thing as IStreamThing;
            if ((streamThing==null && thing!=null) || (thingGraph==null)||(factory ==null)){
                throw new ArgumentException ("stream can not be set");
            }

            return new ThingStreamFacade(factory).SetStream(thingGraph, streamThing, streamInfo);
        }

        public static StreamInfo<Stream> GetStream(WidgetThingGraph graph, IWidget widget) {
            return ThingStreamFacade.GetStreamInfo(
                    WidgetThingGraphExtension.GetThingGraph(graph) as SchemaThingGraph,
                    graph.Get(widget));
        }
    }
}