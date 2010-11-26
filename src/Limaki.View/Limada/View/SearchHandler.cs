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


        public bool IsSearchable(Scene scene) {
            return scene != null && new GraphPairFacade<IWidget, IEdgeWidget>().Source<IThing, ILink>(scene.Graph) != null;
        }

        public void LoadSearch(Scene scene, ILayout<Scene, IWidget> layout, object name) {
            var graph = new GraphPairFacade<IWidget, IEdgeWidget>().Source<IThing, ILink> (scene.Graph);
            
            if (graph==null) {
                throw new ArgumentException ("Search works only on ThingGraphs");
            }

            SceneTools.CleanScene(scene);
            IThingGraph thingGraph = WidgetThingGraphExtension.GetThingGraph(scene.Graph);

            ICollection<IWidget> widgets = new List<IWidget> ();
            foreach (IThing thing in ThingGraphUtils.Search(thingGraph, name, false)) {
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