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

using System;
using Limaki.Drawing;
using Limaki.Drawing.XwtBackend;
using Limaki.View.Viz;
using Limaki.View.Viz.Rendering;
using Xwt.Drawing;

namespace Limaki.View.XwtBackend.Viz {

    public class GraphSceneContextPainterRenderer<T> : IBackendRenderer {

        public ILayer<T> Layer { get; set; }

        public Func<Color> BackColor { get; set; }

        public void Render() {
            throw new NotImplementedException();
        }

        public void Render(IClipper clipper) {
            throw new NotImplementedException();
        }

        public void OnPaint (IRenderEventArgs e) {
            var surface = (ContextSurface) e.Surface;
            var ctx = surface.Context;

            // paint background
            ctx.SetColor(BackColor());
            ctx.Rectangle (e.Clipper.Bounds); 
            ctx.Fill();

            Layer.OnPaint (e);
        }
    }
}