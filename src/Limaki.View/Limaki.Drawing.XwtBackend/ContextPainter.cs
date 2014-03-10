/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Drawing.Painters;
using Xwt.Drawing;

namespace Limaki.Drawing.XwtBackend {

    public abstract class ContextPainter<T>:Painter<T> {

        public virtual void Render (Context ctx, Action<Context, T> draw,
                                    RenderType renderType, Color fillColor, Color penColor, double thickness) {
            ctx.NewPath ();
            draw (ctx, Shape.Data);
            if (renderType.HasFlag (RenderType.Fill)) {
                ctx.SetColor (fillColor);
                ctx.FillPreserve ();
            }
            if (renderType.HasFlag (RenderType.Draw)) {
                ctx.SetColor (penColor);
                ctx.SetLineWidth (thickness);
                ctx.Stroke ();
            }
            ctx.ClosePath ();
        }

        public virtual void Render (Context ctx, Action<Context, T> draw) {
            var style = this.Style;
            var renderType = this.RenderType;
            ctx.NewPath ();
            draw (ctx, Shape.Data);
            if (renderType.HasFlag (RenderType.Fill)) {
                ctx.SetColor (style.FillColor);
                ctx.FillPreserve ();
            }
            if (renderType.HasFlag (RenderType.Draw)) {
                ctx.SetColor (style.Pen.Color);
                ctx.SetLineWidth (style.Pen.Thickness);
                ctx.Stroke ();
            }
            ctx.ClosePath ();
        }
    }
}