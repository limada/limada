/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Visualizers;

namespace Limaki.View.Visuals.UI {

    public interface IGraphSceneEvents<IVisual, IVisualEdge> where IVisualEdge : IEdge<IVisual>, IVisual {
        void GraphChanged (
            IGraph<IVisual, IVisualEdge> sourceGraph, IVisual sourceItem,
            IVisual sinkItem, IGraphScene<IVisual, IVisualEdge> sinkScene,
            IGraphSceneDisplay<IVisual, IVisualEdge> sinkDisplay, GraphEventType eventType);

        void GraphDataChanged (IVisual sourceItem, IVisual sinkItem, IGraphScene<IVisual, IVisualEdge> sinkScene, IGraphSceneDisplay<IVisual, IVisualEdge> sinkDisplay);
    }
}