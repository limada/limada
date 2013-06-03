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
using Limaki.View.Layout;
using Limaki.Visuals;
using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// enumerates all things of scene's selected elements
        /// if a Document is the only selected, then the Pages of the Document are enumerated
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public IEnumerable<IThing> SelectedThings (IGraphScene<IVisual, IVisualEdge> scene) {
            var visuals = scene.Selected.Elements;
            if (visuals.Count() == 0)
                visuals = scene.Graph.Where(v => !(v is IVisualEdge));
            if (visuals.Count() == 0)
                return null;
            IEnumerable<IThing> things = null;
            if (visuals.Count() == 1) {
                var thing = scene.Graph.ThingOf(visuals.First());
                var schema = new DocumentSchema(scene.Graph.ThingGraph(), thing);
                if (schema.HasPages())
                    things = schema.OrderedPages();

            }
            if (things == null) {
                things = visuals
                    .OrderBy(v => v.Location, new PointComparer { Delta = 20 })
                    .Select(v => scene.Graph.ThingOf(v));
            }
            return things;

        }

       public void SetDescription (IGraphScene<IVisual, IVisualEdge> scene, IThing thing, string fileName) {
            if (thing != null) {
                var thingGraph = scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>().Two as IThingGraph;
                thingGraph.SetSource(thing, fileName);
                var desc = thingGraph.Description(thing);
                if (desc == null || desc.ToString() == string.Empty) {
                    desc = Path.GetFileNameWithoutExtension(fileName);
                    var vis = scene.Graph.VisualOf(thing);
                    if (vis != null) {
                        vis.Data = desc;
                        scene.Requests.Add(new LayoutCommand<IVisual>(vis, LayoutActionType.Justify));
                    }
                }

            }
        }
    }
}