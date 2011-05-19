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
using Limada.View;
using Limaki.Drawing;
using Limaki.Graphs.Extensions;
using Limaki.Presenter.Widgets.Layout;
using Limaki.Presenter.Widgets.UI;
using Limaki.Widgets;
using System.Linq;
using Limaki.Presenter.UI;

namespace Limada.Presenter {
    public class SearchHandler {


        public bool IsSearchable(Scene scene) {
            return scene != null && GraphPairExtension<IWidget, IEdgeWidget>.Source<IThing, ILink>(scene.Graph) != null;
        }

        public void LoadSearch(Scene scene, IGraphLayout<IWidget,IEdgeWidget> layout, object name) {
            var graph = GraphPairExtension<IWidget, IEdgeWidget>.Source<IThing, ILink> (scene.Graph);
            
            if (graph==null) {
                throw new ArgumentException ("Search works only on ThingGraphs");
            }

            SceneTools.CleanScene(scene);
            var thingGraph = scene.Graph.ThingGraph();

            var widgets = from thing in thingGraph.Search(name, false)
                        let widget = graph.Get(thing)
                        orderby widget.Data.ToString()
                        select widget;

            new GraphSceneFacade<IWidget, IEdgeWidget>(delegate() { return scene; }, layout).Add(widgets, false, true);

            scene.ClearSpatialIndex();
        }
    }
}