/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2013 Lytico
 *
 * http://www.limada.org
 */


using Limada.Model;
using Limada.Schemata;
using Limada.VisualThings;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Visuals;
using System.Linq;

namespace Limada.Usecases {

    public class SceneIo {

        public SceneIo () {
            UseSchema = true;
        }

        public bool UseSchema { get; set; }

        protected virtual IGraph<IVisual, IVisualEdge> CreateVisualGraph (IThingGraph thingGraph) {

            SchemaFacade.MakeMarkersUnique(thingGraph);

            var schemaGraph = thingGraph;
            if (UseSchema && !(thingGraph is SchemaThingGraph)) {
                schemaGraph = new SchemaThingGraph(thingGraph);
            }

            return new GraphView<IVisual, IVisualEdge>(new VisualThingGraph(new VisualGraph(), schemaGraph), new VisualGraph());
        }

        public virtual IGraphScene<IVisual, IVisualEdge> CreateScene (IThingGraph thingGraph) {
            return new Scene { Graph = CreateVisualGraph(thingGraph) };
        }

        public virtual void Flush (IGraphScene<IVisual, IVisualEdge> scene) {
            if (scene == null)
                return;
            var visualThingGraph = scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>();
            if (visualThingGraph != null) {
                visualThingGraph.Mapper.ConvertOneTwo();
            }

        }

        /// <summary>
        /// creates a ThingGraphView with only the things that are in the scene
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public virtual GraphView<IThing, ILink> CreateThingsView (IGraphScene<IVisual, IVisualEdge> scene) {
            var graph = scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>();
            if (graph == null)
                return null;

            var thingView = new GraphView<IThing, ILink>(graph.Two as IThingGraph, new ThingGraph());
            thingView.AddRange(scene.Elements.Select(v => graph.Get(v)));

            return thingView;

        }

    }
}