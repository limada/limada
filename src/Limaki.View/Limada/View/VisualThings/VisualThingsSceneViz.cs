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
using Limaki.Drawing;
using Limaki.Graphs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Limaki.Common;
using Limaki.View;
using Limaki.View.GraphScene;
using Limaki.View.Visuals;
using Limaki.View.Viz.Mesh;
using Limaki.View.Viz.Modelling;

namespace Limada.View.VisualThings {
    /// <summary>
    /// helper class to manage VisualGraphScenes backed by ThingGraphs
    /// </summary>
    public class VisualThingsSceneViz {

        public VisualThingsSceneViz () {
            UseSchema = true;
        }

        public bool UseSchema { get; set; }

        protected virtual IGraph<IVisual, IVisualEdge> CreateVisualGraph (IThingGraph thingGraph) {

            SchemaFacade.MakeMarkersUnique(thingGraph);

            var schemaGraph = thingGraph;
            if (UseSchema && !(thingGraph is SchemaThingGraph)) {
                schemaGraph = new SchemaThingGraph(thingGraph);
            }

            return new SubGraph<IVisual, IVisualEdge>(new VisualThingGraph(new VisualGraph(), schemaGraph), new VisualGraph());
        }

        // TODO: harmonise this with GraphSceneMesh.CreateSinkScene / CreateSinkGraph
        public virtual IGraphScene<IVisual, IVisualEdge> CreateScene (IThingGraph thingGraph) {
            var result = new Scene { Graph = CreateVisualGraph(thingGraph) };
            result.CreateMarkers();
            return result;
        }

        public virtual void Flush (IGraphScene<IVisual, IVisualEdge> scene) {
            if (scene == null)
                return;
            var visualThingGraph = scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>();
            if (visualThingGraph != null) {
                visualThingGraph.Mapper.ConvertSinkSource();
            }

        }

        /// <summary>
        /// creates a ThingGraphView with only the things that are in the scene
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public virtual SubGraph<IThing, ILink> CreateThingsView (IGraphScene<IVisual, IVisualEdge> scene) {
            var graph = scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>();
            if (graph == null)
                return null;

            var thingView = new SubGraph<IThing, ILink>(graph.Source as IThingGraph, new ThingGraph());
            thingView.AddRange(scene.Elements.Select(v => graph.Get(v)));

            return thingView;

        }

        /// <summary>
        /// enumerates all things of scene's selected elements
        /// if a Digidoc is the only selected, then the Pages of the Digidoc are enumerated
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
                var digidoc = new DigidocSchema(scene.Graph.ThingGraph(), thing);
                if (digidoc.HasPages())
                    things = digidoc.OrderedPages();

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

                var thingGraph = scene.Graph.ThingGraph();
                thingGraph.SetSource(thing, fileName);

                var desc = thingGraph.Description(thing);

                if (desc == null || desc.ToString() == string.Empty) {
                    desc = Path.GetFileNameWithoutExtension(fileName);
                    var vis = scene.Graph.VisualOf(thing);
                    if (vis != null) {
                        scene.Graph.DoChangeData (vis, desc);
                        scene.Requests.Add(new LayoutCommand<IVisual>(vis, LayoutActionType.Justify));
                    }
                }

            }
        }

       public void MergeVisual (IGraphScene<IVisual, IVisualEdge> scene) {

           if (scene.Selected.Count != 2)
               throw new NotSupportedException ("Currently only merges of 2 things are supported");

           var sink = scene.Selected.Elements.OrderBy (v => v.Location, new PointComparer { Order = PointOrder.XY }).First ();
           var sweep = scene.Selected.Elements.Where (v => v != sink).FirstOrDefault ();

           var thingGraph = scene.Graph.ThingGraph ();
           
           if (thingGraph == null)
               throw new NotSupportedException ("Currently only merges of Thing-Backed graphs are supported");

           var meshed = Registry.Pooled<IGraphSceneMesh<IVisual, IVisualEdge>> ()
               .Scenes.Any (s => s == scene);

           var sweepThing = scene.Graph.ThingOf (sweep);
           var sinkThing = scene.Graph.ThingOf (sink);
           var graphPair = scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>();

           var allowed = typeof (IThing<string>).IsAssignableFrom (sweepThing.GetType ()) && sweepThing.GetType () == sinkThing.GetType ();
           if (!allowed) {
               throw new NotSupportedException ("Currently only merges of texts are supported");
           }

           foreach (var thing in thingGraph.MergeThing (sweepThing, sinkThing)) {
               thingGraph.OnGraphChange (thing, GraphEventType.Update);

               if (!meshed) {
                   if (scene.Graph.ContainsVisualOf (thing)) {
                       var vis = scene.Graph.VisualOf (thing);
                       graphPair.UpdateSink (vis);
                       if (scene.Contains (vis))
                           scene.Requests.Add (new LayoutCommand<IVisual> (vis, LayoutActionType.Justify));
                   }
               }
           }

           scene.Requests.Add (new DeleteCommand<IVisual, IVisualEdge> (sweep, scene));


       }

    }
}