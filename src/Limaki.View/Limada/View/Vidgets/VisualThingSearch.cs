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
 */

using System;
using Limada.Model;
using System.Linq;
using Limaki.Graphs;
using Limada.View.VisualThings;
using Limaki.View;
using Limaki.View.GraphScene;
using Limaki.View.Visuals;
using Limaki.View.Viz.Modelling;
using Xwt;
using Limaki.View.Vidgets;

namespace Limada.View.Vidgets {
    
    /// <summary>
    /// searchs in a VisualScene backed by a ThingGraph
    /// </summary>
    public class VisualThingSearch:IVisualGraphSceneSearch {

        public bool IsSearchable(IGraphScene<IVisual, IVisualEdge> scene) {
            return scene != null && scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>() != null;
        }

        public void LoadSearch(IGraphScene<IVisual, IVisualEdge> scene, IGraphSceneLayout<IVisual, IVisualEdge> layout, object name) {

            var graph = scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>();
            
            if (graph==null) {
                throw new ArgumentException ("Search works only on ThingGraphs");
            }

            scene.CleanScene();

            var visuals = scene.Graph.ThingGraph()
                .Search(name, false)
                .Select(t => graph.Get(t));

            new GraphSceneFacade<IVisual, IVisualEdge>(() => scene, layout)
                .Add(visuals, false, false);

            var aligner = new Aligner<IVisual, IVisualEdge>(scene, layout);
            aligner.FullLayout(null, new Point(layout.Border.Width, layout.Border.Height), layout.Options(), new VisualComparer());
            aligner.Commit();

            scene.ClearSpatialIndex();
        }
    }
}