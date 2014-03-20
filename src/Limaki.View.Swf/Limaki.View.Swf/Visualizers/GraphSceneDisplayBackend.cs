/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Gdi.UI;
using Limaki.View.Viz.Visualizers;

namespace Limaki.View.Swf.Visualizers {

    public abstract class GraphSceneDisplayBackend<TItem, TEdge> : DisplayBackend<IGraphScene<TItem, TEdge>>
        where TEdge : TItem, IEdge<TItem> {
    }

    public class GraphSceneDisplayBackendComposer<TItem, TEdge> : DisplayBackendComposer<IGraphScene<TItem, TEdge>>
    where TEdge : TItem, IEdge<TItem> {
        public override void Factor(Display<IGraphScene<TItem, TEdge>> display) {
            base.Factor(display);
            this.DataLayer = new GdiGraphSceneLayer<TItem, TEdge>();
        }
    }
}