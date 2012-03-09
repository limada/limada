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
 */

using System;
using Limada.Model;
using Limaki.Drawing;
using Limaki.Graphs.Extensions;
using System.Linq;
using Limada.View;
using Limada.VisualThings;
using Limaki.View.UI.GraphScene;
using Limaki.View.Visuals.UI;
using Limaki.Visuals;

namespace Limada.View {
    public class SearchHandler {


        public bool IsSearchable(IGraphScene<IVisual, IVisualEdge> scene) {
            return scene != null && scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>() != null;
        }

        public void LoadSearch(IGraphScene<IVisual, IVisualEdge> scene, IGraphLayout<IVisual, IVisualEdge> layout, object name) {
            var graph = scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>();
            
            if (graph==null) {
                throw new ArgumentException ("Search works only on ThingGraphs");
            }

            SceneExtensions.CleanScene(scene);
            var thingGraph = scene.Graph.ThingGraph();

            var visuals = from thing in thingGraph.Search(name, false)
                        let visual = graph.Get(thing)
                        orderby visual.Data.ToString()
                        select visual;

            new GraphSceneFacade<IVisual, IVisualEdge>(delegate() { return scene; }, layout).Add(visuals, false, true);

            scene.ClearSpatialIndex();
        }
    }
}