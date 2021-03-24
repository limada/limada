/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing.Shapes;
using Xwt.Drawing;
using Xwt;
using System.Collections.Generic;

namespace Limaki.Drawing.XwtBackend {

    public class ContextPainterExtensions {

        public static void DrawBezier (Context ctx, IList<Point> bezierPoints) {
            ctx.MoveTo(bezierPoints[0]);
            for (int i = 1; i < bezierPoints.Count; i += 3)
                ctx.CurveTo(bezierPoints[i], bezierPoints[i + 1], bezierPoints[i + 2]);
            ctx.ClosePath();

        }

        public static void DrawBezier (Context ctx, BezierSegment seg) {
            ctx.MoveTo(seg.Start);
            ctx.CurveTo(seg.Cp1, seg.Cp2, seg.End);
        }

        public static void DrawPoligon (Context ctx, IList<Point> poligon) {
            ctx.MoveTo(poligon[0]);
            for (int i = 1; i < poligon.Count; i ++)
                ctx.LineTo(poligon[i]);
            ctx.ClosePath();

        }

        public static void DrawRoundedRect (Context ctx, Rectangle rectangle, double radius) {
            // create the arc for the Rectangle sides 
            var l = rectangle.Left;
            var t = rectangle.Top;
            var w = rectangle.Width;
            var h = rectangle.Height;

            // top left  
            ctx.Arc(l + radius, t + radius, radius, 180, 270);
            // top right 
            ctx.Arc(l + w - radius, t + radius, radius, 270, 0);
            // bottom right  
            ctx.Arc(l + w - radius, t + h - radius, radius, 0, 90);
            // bottom left 
            ctx.Arc(l + radius, t + h - radius, radius, 90, 180);
        }

        /// <summary>
        /// draws text centered in rect
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="rect"></param>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="textColor"></param>
        public static void DrawText (Context ctx, Rectangle rect, string text, Font font, Color textColor) {
            var w = rect.Width;
            var h = rect.Height;
            if (w > 1 && h > 1) {
                // this gives not necessarily the correct height:
                var textLayout = new TextLayout(ctx) {
                    Trimming = TextTrimming.WordElipsis, 
                    Text = text, Font = font, Width = w + 0.1, Height = h+0.1,
                };
                var size = textLayout.GetSize();
                w = size.Width < w ? (w - size.Width) / 2d : 0;
                h = size.Height < h ? (h - size.Height) / 2d : 0;
                ctx.SetColor(textColor);
                ctx.SetLineWidth(1);
                ctx.DrawTextLayout(textLayout, rect.X + w, rect.Y);

            }
        }
    }
}