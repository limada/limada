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
using System.IO;
using Limada.Model;
using Limada.View;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model.Streams;
using Limaki.Visuals;
using Limada.Schemata;

namespace Limada.View {
    public class VisualThingStreamHelper {
        public virtual IVisual CreateFromStream( IGraph<IVisual, IVisualEdge> graph, StreamInfo<Stream> streamInfo ) {
            
            IVisual result = null;
            var sourceGraph =
                GraphPairExtension<IVisual, IVisualEdge>.Source<IThing, ILink>(graph);

            if (sourceGraph != null) {
                var thingGraph = graph.ThingGraph();
                var factory = graph.ThingFactory();
                
                IThing thing = new ThingStreamFacade(factory).SetStream(thingGraph, null, streamInfo);
                
                result = sourceGraph.Get(thing);
            }
            return result;
        }

        public IThing SetStream(IGraph<IVisual,IVisualEdge> graph, IThing thing, StreamInfo<Stream> streamInfo) {
            var thingGraph = graph.ThingGraph();
            var factory = graph.ThingFactory();
            var streamThing = thing as IStreamThing;
            if ((streamThing==null && thing!=null) || (thingGraph==null)||(factory ==null)){
                throw new ArgumentException ("stream can not be set");
            }

            return new ThingStreamFacade(factory).SetStream(thingGraph, streamThing, streamInfo);
        }

        public StreamInfo<Stream> GetStream(IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
             var sourceGraph =
                GraphPairExtension<IVisual, IVisualEdge>.Source<IThing, ILink>(graph);

             if (sourceGraph != null) {
                 var thingGraph = graph.ThingGraph();
                 return ThingStreamFacade.GetStreamInfo (
                     thingGraph,
                     sourceGraph.Get(visual));
             }
             return null;
        }
    }
}