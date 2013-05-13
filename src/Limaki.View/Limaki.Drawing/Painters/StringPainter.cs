/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2012 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing.Shapes;
using System;
using Xwt.Drawing;
using Xwt;

namespace Limaki.Drawing.Painters {

    public class StringPainter : StringPainterBase {

        public override void Render(ISurface surface) {
            if (string.IsNullOrEmpty(this.Text))
                return;

            var matrix = ((ContextSurface)surface).Matrix;
            if (matrix.M11 < 0.2d || matrix.M22 < 0.2d)
                return;

            var ctx = ((ContextSurface)surface).Context;
            var style = this.Style;
            var shape = this.OuterShape;
            var font = Style.Font;

            if (AlignText && shape is IVectorShape) {
                var vector = ((IVectorShape)shape).Data;

                var f = ctx.DpiFactor();
                var width = Vector.Length(vector);
                
                var height = Math.Ceiling(font.Size);

                var text = new TextLayout(ctx);
                text.Trimming = TextTrimming.WordElipsis;
                text.Text = this.Text;
                text.Font = font;
                var size = text.GetSize();
                width = Math.Min(size.Width, width);
                height = Math.Max(size.Height,height);
                text.Width = width;
                text.Height = height;

                var c = new Point(
                    (vector.Start.X + (vector.End.X - vector.Start.X) / 2d),
                    (vector.Start.Y + (vector.End.Y - vector.Start.Y) / 2d));

                ctx.Save();
                ctx.Translate(c.X, c.Y);
                ctx.Rotate(Vector.Angle(vector));

                ctx.SetColor(style.TextColor);
                ctx.SetLineWidth(1);
                ctx.DrawTextLayout(text, -width / 2d, -height / 2d);
                ctx.Restore();
            } else {
                var rect = shape.BoundsRect.Inflate(-5, -5);
                ContextPainterExtensions.DrawText(ctx, rect, this.Text, font, style.TextColor);
            }

        }
    }
}