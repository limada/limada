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
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.XwtBackend;
using Limaki.View.Viz;
using Limaki.View.Viz.Rendering;
using Limaki.View.XwtBackend;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.XwtBackend {

    /// <summary>
    /// BackendRenderer drawing on a Xwt.Drawing.Context
    /// and DisplayBackend as Backend
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XwtBackendRenderer<T> : IBackendRenderer {

        public virtual DisplayBackend<T> Backend { get; set; }
        public virtual IDisplay<T> Display { get; set; }

        public void Render () {
            Backend.QueueDraw();
        }

        public void Render (IClipper clipper) {
            Backend.QueueDraw(clipper.Bounds);
        }

        public Func<Color> BackColor { get; set; }

        public void OnPaint (IRenderEventArgs e) {
            var ev = e as RenderContextEventArgs;
            var sf = e.Surface as ContextSurface;
            OnDraw(sf.Context, e.Clipper.Bounds);
        }

        public void OnDraw (Context ctx, Rectangle dirtyRect) {
            var display = this.Display;
            var data = display.Data;

            // draw background
            ctx.SetColor(BackColor());
            ctx.Rectangle(dirtyRect); 
            ctx.Fill();

            if (data != null) {
                try {
                    ctx.Save();
                    display.ActionDispatcher.OnPaint(new RenderContextEventArgs(ctx, dirtyRect));
                } catch (Exception ex) {
                    Registry.Pooled<IExceptionHandler>().Catch(ex, MessageType.OK);
                } finally {
                    ctx.Restore();
                }
            }
        }
     }

}
