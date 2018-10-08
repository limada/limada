/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2015 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Graphs;
using Limaki.View.Visuals;
using Limaki.View.Viz.Mapping;
using Limaki.View.Viz.Visualizers;
using System.Collections.Generic;

namespace Limaki.View.Viz.Visuals {

    [Obsolete]
    public class VisualGraphSceneDisplayMeshEvents : IGraphSceneDisplayEvents<IVisual, IVisualEdge> {

        public void GraphChanged (
            object sender,
            GraphChangeArgs<IVisual, IVisualEdge> args,
            IVisual sinkItem, IGraphScene<IVisual, IVisualEdge> sinkScene,
            IGraphSceneDisplay<IVisual, IVisualEdge> sinkDisplay) {

            // everything is done in MeshBackHandler
            return;
        }
    }
}