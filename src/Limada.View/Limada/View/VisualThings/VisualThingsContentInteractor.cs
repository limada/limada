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
using Limada.Schemata;
using Limaki.Contents;
using Limaki.Graphs;
using Limaki.View.Visuals;

namespace Limada.View.VisualThings {
    
    /// <summary>
    /// helper class to manage VisualGraphs backed by StreamThings
    /// </summary>
    public class VisualThingsContentInteractor : IVisualContentInteractor<IThing> {


		/// <summary>
		/// creates a visual, backed by a
		/// StreamThing, created and assigned with content
		/// </summary>
		/// <param name="graph"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		public virtual IVisual VisualOfContent (IGraph<IVisual, IVisualEdge> graph, Content<Stream> content) {
            
            if (content == null)
                return null;

            IVisual result = null;
            var sourceGraph = graph.Source<IVisual, IVisualEdge, IThing, ILink>();
            if (sourceGraph != null) {
                var thingGraph = graph.ThingGraph();
                var factory = graph.ThingFactory();
                IThing thing = null;
                if (content.Data != null)
                    thing = new ThingContentFacade(factory).AssignContent(thingGraph, null, content);
                else {
                    thing = factory.CreateItem(content.Description);
                    thingGraph.Add(thing);
                }

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
                return thingGraph.ContentOf(sourceGraph.Get(visual));
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