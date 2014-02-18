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

namespace Limaki.View.Headless.VidgetBackends {

    public abstract class GraphSceneDisplayBackend<TItem, TEdge> : DisplayBackend<IGraphScene<TItem, TEdge>>
        where TEdge : TItem, IEdge<TItem> {
    }

    public class GraphSceneDisplayBackendComposer<TItem, TEdge> : DisplayBackendComposer<IGraphScene<TItem, TEdge>>
    where TEdge : TItem, IEdge<TItem> {
        public override void Factor(Display<IGraphScene<TItem, TEdge>> display) {
            base.Factor(display);
            this.DataLayer = new HeadlessGraphSceneLayer<TItem, TEdge>();
        }
    }
}
