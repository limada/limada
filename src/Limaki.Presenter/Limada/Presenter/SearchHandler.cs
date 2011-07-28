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
using System.Collections.Generic;
using Limada.Model;
using Limada.View;
using Limaki.Drawing;
using Limaki.Graphs.Extensions;
using Limaki.Presenter.Visuals.Layout;
using Limaki.Presenter.Visuals.UI;
using Limaki.Visuals;
using System.Linq;
using Limaki.Presenter.UI;

namespace Limada.Presenter {
    public class SearchHandler {


        public bool IsSearchable(IGraphScene<IVisual, IVisualEdge> scene) {
            return scene != null && scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>() != null;
        }

        public void LoadSearch(IGraphScene<IVisual, IVisualEdge> scene, IGraphLayout<IVisual, IVisualEdge> layout, object name) {
            var graph = scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>();
            
            if (graph==null) {
                throw new ArgumentException ("Search works only on ThingGraphs");
            }

            SceneTools.CleanScene(scene);
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