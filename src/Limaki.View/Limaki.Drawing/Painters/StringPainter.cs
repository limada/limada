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
 * http://limada.sourceforge.net
 * 
 */

using Limaki.Drawing.Shapes;
using System;
using Xwt.Drawing;
using Xwt;

namespace Limaki.Drawing.Painters {

    public class StringPainter : StringPainterBase, IPainter<string> {

        public override void Render(ISurface surface) {
            if (string.IsNullOrEmpty(this.Text))
                return;

            var matrix = ((ContextSurface)surface).Matrix;
            var elements = matrix.Elements;
            if (elements[0] < 0.2d || elements[3] < 0.2d)
                return;

            var ctx = ((ContextSurface)surface).Context;
            var style = this.Style;
            var shape = this.Shape;
            var font = Style.Font;

            if (AlignText && shape is IVectorShape) {
                var vector = ((IVectorShape)shape).Data;
                var width = Vector.Length(vector);
                var height = font.Size + (font.Size / 2d);

                var text = new TextLayout(ctx);
                text.Trimming = TextTrimming.WordElipsis;
                text.Text = this.Text;
                text.Font = font;
                text.Height = height;
                var size = text.GetSize();
                width = Math.Min(size.Width, width);
                text.Width = width;

                var c = new Point(
                    (vector.Start.X + (vector.End.X - vector.Start.X) / 2d),
                    (vector.Start.Y + (vector.End.Y - vector.Start.Y) / 2d));

                ctx.Save();
                ctx.Translate(c.X, c.Y);
                ctx.Rotate(Vector.Angle(vector));

                ctx.SetColor(style.TextColor);
                ctx.SetLineWidth(1);
                ctx.DrawTextLayout(text, -width / 2f, -height / 2f);
                ctx.Restore();
            } else {
                var rect = shape.BoundsRect.Inflate(-5, -5);
                //rect.Inflate(-PenWidth, -PenWidth);

                var w = rect.Width;
                var h = rect.Height;
                if (w > 1 && h > 1) {
                    var text = new TextLayout(ctx);
                    text.Trimming = TextTrimming.WordElipsis;
                    text.Text = this.Text;
                    text.Font = font;
                    text.Width = w;
                    text.Height = h;
                    var size = text.GetSize();
                    w = size.Width < w ? (w - size.Width - 5) / 2d : 0;
                    var lh = (font.Size + (font.Size / 2d)) / 2d;
                    h = size.Height < h ? (h - size.Height - 5 + lh) / 2d : 0;
                    ctx.SetColor(style.TextColor);
                    ctx.SetLineWidth(1);
                    ctx.DrawTextLayout(text, rect.X + w, rect.Y + h);

                }
            }

        }
    }
}