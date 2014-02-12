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
using Limaki.Contents;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model.Content;
using Limaki.Visuals;
using Limaki.View.DragDrop;

namespace Limada.VisualThings {
    /// <summary>
    /// helper class to manage VisualGraphs backed by StreamThings
    /// </summary>
    public class VisualThingsContentViz : IVisualContentViz<IThing> {

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
        /// creates a visual, backed by a
        /// StreamThing, created and assigned with content
        /// tries to get out the most information in content
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual IVisual VisualOfRichContent (IGraph<IVisual, IVisualEdge> graph, Content<Stream> content) {
            
            IVisual result = null;
            var sourceGraph = graph.Source<IVisual, IVisualEdge, IThing, ILink>();
            if (sourceGraph != null) {
                var thingGraph = graph.ThingGraph();
                var factory = graph.ThingFactory();

                var thing = new ThingContentFacade(factory).AssignContent(thingGraph, null, content);

                // TODO: use ContentThingDiggers here (over its provider), similar to a ContentDigger, but with graph and thing as parameters

                result = sourceGraph.Get(thing);
            }
            return result;
        }

        /// <summary>
        /// gives back the content of the
        /// visual if this is backed by a StreamThing
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="visual"></param>
        /// <returns></returns>
        public Content<Stream> ContentOf (IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            var sourceGraph = graph.Source<IVisual, IVisualEdge, IThing, ILink>();
            if (sourceGraph != null) {
                var thingGraph = graph.ThingGraph();
                return ThingContentFacade.ConentOf(
                    thingGraph,
                    sourceGraph.Get(visual));
            }
            return null;
        }

        /// <summary>
        /// assigns a content to the 
        /// thing (if its a StreamThing)
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="store"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual IThing AssignContent(IGraph<IVisual,IVisualEdge> graph, IThing store, Content<Stream> content) {
            var thingGraph = graph.ThingGraph();
            var factory = graph.ThingFactory();
            var streamThing = store as IStreamThing;
            if ((streamThing==null && store!=null) || (thingGraph==null)||(factory ==null)){
                throw new ArgumentException ("stream can not be set");
            }

            return new ThingContentFacade(factory).AssignContent(thingGraph, streamThing, content);
        }

        
    }
}