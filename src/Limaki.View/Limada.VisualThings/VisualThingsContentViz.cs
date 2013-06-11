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

using System;
using System.IO;
using Limada.Model;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model.Content;
using Limaki.Visuals;

namespace Limada.VisualThings {

    /// <summary>
    /// helper class to manage VisualGraphs backed by StreamThings
    /// </summary>
    public class VisualThingsContentViz {

        /// <summary>
        /// creates a visual, backed by a
        /// StreamThing, created and assigned with content
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual IVisual VisualOfContent( IGraph<IVisual, IVisualEdge> graph, Content<Stream> content ) {
            
            IVisual result = null;
            var sourceGraph = graph.Source<IVisual, IVisualEdge, IThing, ILink>();
            if (sourceGraph != null) {
                var thingGraph = graph.ThingGraph();
                var factory = graph.ThingFactory();
                
                var thing = new ThingContentFacade(factory).AssignContent(thingGraph, null, content);
                
                result = sourceGraph.Get(thing);
            }
            return result;
        }

        /// <summary>
        /// assigns a content to the 
        /// thing (if its a StreamThing)
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="thing"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public IThing AssignContent(IGraph<IVisual,IVisualEdge> graph, IThing thing, Content<Stream> content) {
            var thingGraph = graph.ThingGraph();
            var factory = graph.ThingFactory();
            var streamThing = thing as IStreamThing;
            if ((streamThing==null && thing!=null) || (thingGraph==null)||(factory ==null)){
                throw new ArgumentException ("stream can not be set");
            }

            return new ThingContentFacade(factory).AssignContent(thingGraph, streamThing, content);
        }

        /// <summary>
        /// gives back the content of the
        /// visual if this is backed by a StreamThing
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="visual"></param>
        /// <returns></returns>
        public Content<Stream> ContentOf(IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            var sourceGraph = graph.Source<IVisual, IVisualEdge, IThing, ILink>();
             if (sourceGraph != null) {
                 var thingGraph = graph.ThingGraph();
                 return ThingContentFacade.ConentOf (
                     thingGraph,
                     sourceGraph.Get(visual));
             }
             return null;
        }
    }
}