/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Graphs;
using Limaki.View.Rendering;
using Limaki.XwtAdapter;
using Limaki.Drawing;

namespace Limaki.View.XwtContext {
    public class GraphSceneContextLayer<TItem, TEdge> : GraphSceneLayer<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        public bool AntiAlias = true;

        public override void OnPaint (IRenderEventArgs e) {
            var surface = ((ContextSurface) e.Surface);
            var ctx = surface.Context;

            var transform = this.Camera.Matrix;
            
            ctx.SetTransform (transform);
            surface.Matrix = transform;

            this.Renderer.Render (this.Data, e);
            ctx.ResetTransform();

        }

        public override void DataChanged () { }
        }
}