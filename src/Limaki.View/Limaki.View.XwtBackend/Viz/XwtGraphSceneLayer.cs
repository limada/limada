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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Drawing.XwtBackend;
using Limaki.View.Viz.Rendering;

namespace Limaki.View.XwtBackend.Viz {

    public class XwtGraphSceneLayer<TItem, TEdge> : GraphSceneLayer<TItem, TEdge>
     where TEdge : TItem, IEdge<TItem> {

        public override void OnPaint (IRenderEventArgs e) {
            var surface = (ContextSurface) e.Surface;
            var ctx = surface.Context;
            var matrix = this.Camera.Matrix;

            ctx.Save();
            
            try {
                if(!matrix.IsIdentity)
                    ctx.ModifyCTM(matrix);
                this.Renderer.Render(this.Data, e);

            } catch(Exception ex) {
                throw;
            } finally {
                ctx.Restore();
            }
               
        }
    }
}
