/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Drawing.Drawing2D;
using Limaki.Drawing.GDI;
using Limaki.Drawing.Shapes;
using Xwt;
using Xwt.Gdi;
using Xwt.Gdi.Backend;

namespace Limaki.Drawing.GDI.Painters {

    public class RectanglePainter : GdiPainter<Rectangle>, IPainter<IRectangleShape, Rectangle> {

        public override void RenderXwt (ISurface surface) {
            var ctx = ((ContextSurface) surface).Context;
            Render (ctx, (c, d) => c.Rectangle (d));
        }

        public override void RenderGdi (ISurface surface) {
            var rect = GDIConverter.Convert(Shape.Data);
            var style = this.Style;
            var renderType = this.RenderType;
            var g = ((GDISurface)surface).Graphics;
            if ((RenderType.Fill & renderType) != 0) {
                g.FillRectangle(
                    GetSolidBrush(GdiConverter.ToGdi(style.FillColor)), 
                    rect);
                
            }
            if ((RenderType.Draw & renderType) != 0) {
                //if (Style.Pen.Alignment == System.Drawing.Drawing2D.PenAlignment.Center) {
                //    int penSize = -(int)Style.Pen.Width/2;
                //    Rectangle rect = Rectangle.Inflate(Shape.Data, penSize, penSize);
                //}
                System.Drawing.Pen pen = ((GDIPen)style.Pen).Backend;
                g.DrawRectangle(pen, rect);
            }


        }
       
    }
}