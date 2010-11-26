/*
 * Limada 
 * Version 0.08
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 */

using System;
using System.Collections.Generic;
using Limada.Model;
using Limada.Schemata;
using Limaki.Drawing;
using Limaki.Graphs.Extensions;
using Limaki.Widgets;
using Limaki.Widgets.Layout;

namespace Limada.View {
    public class SearchHandler {
        public IEnumerable<IThing> Search(Scene scene, object name) {
            IThingGraph thingGraph = WidgetThingGraphExtension.GetThingGraph(scene.Graph);
            if (thingGraph is SchemaThingGraph) {
                thingGraph = ((SchemaThingGraph)thingGraph).Source as IThingGraph;
            }

            CommonSchema schema = new CommonSchema();
            
            foreach (IThing thing in thingGraph.GetByData(name,false)) {
                IThing described = schema.GetTheRoot(thingGraph, thing, CommonSchema.DescriptionMarker);
                if (described != null) {
                    yield return described;
                } else {
                    yield return thing;
                }
            }


        }

        public bool IsSearchable(Scene scene) {
            return new GraphPairFacade<IWidget, IEdgeWidget>().Source<IThing, ILink>(scene.Graph) != null;
        }

        public void LoadSearch(Scene scene, ILayout<Scene, IWidget> layout, object name) {
            var graph = new GraphPairFacade<IWidget, IEdgeWidget>().Source<IThing, ILink> (scene.Graph);
            
            if (graph==null) {
                throw new ArgumentException ("Search works only on ThingGraphs");
            }

            SceneTools.CleanScene(scene);

            ICollection<IWidget> widgets = new List<IWidget> ();
            foreach (IThing thing in Search(scene,name)) {
                IWidget widget = graph.Get (thing);
                if (widget != null) { // could be a marker or marked thing, then returns null
                    widgets.Add (widget);
                }
            }

            new SceneFacade(delegate() { return scene; }, layout).Add(widgets, false, true);

            scene.ClearSpatialIndex();
           

            
        }
    }
}