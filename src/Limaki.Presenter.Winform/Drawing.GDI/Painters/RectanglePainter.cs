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
using System.Drawing;
using System.Drawing.Drawing2D;
using Limaki.Drawing.GDI;
using Limaki.Drawing.Painters;

namespace Limaki.Drawing.GDI.Painters {
    public class RectanglePainter:Painter<RectangleI>,IPainter<IRectangleShape,RectangleI> {
        protected SolidBrush _brush = null;
        protected virtual SolidBrush GetSolidBrush(System.Drawing.Color color) {
            if (_brush != null) {
                // maybe more expensive to ask as to set:
                //if (_brush.Color != color) {
                //    _brush.Color = color;
                //}
                _brush.Color = color;
                return _brush;

            }
            if (_brush != null) {
                _brush.Dispose ();
                _brush = null;
            }
            _brush = new SolidBrush(color);
            return _brush;
        }

        public override void Render( ISurface surface ) {
            Rectangle rect = GDIConverter.Convert(Shape.Data);
            IStyle style = this.Style;
            RenderType renderType = this.RenderType;
            Graphics g = ((GDISurface)surface).Graphics;
            if ((RenderType.Fill & renderType) != 0) {
                g.FillRectangle(
                    GetSolidBrush(GDIConverter.Convert(style.FillColor)), 
                    rect);
                
            }
            if ((RenderType.Draw & renderType) != 0) {
                //if (Style.Pen.Alignment == System.Drawing.Drawing2D.PenAlignment.Center) {
                //    int penSize = -(int)Style.Pen.Width/2;
                //    Rectangle rect = Rectangle.Inflate(Shape.Data, penSize, penSize);
                //}
                System.Drawing.Pen pen = ((GDIPen)style.Pen).Native;
                g.DrawRectangle(pen, rect);
            }


        }
        public override void Dispose(bool disposing) {
            base.Dispose(disposing);
            if (disposing) {
                if (_brush != null) {
                    _brush.Dispose();
                    _brush = null;
                }   
            }
        }
    }
}