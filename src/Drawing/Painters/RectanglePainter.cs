/*
 * Limaki 
 * Version 0.07
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Limaki.Drawing.Painters {
    public class RectanglePainter:Painter<Rectangle> {
        protected SolidBrush _brush = null;
        protected virtual SolidBrush GetSolidBrush(Color color) {
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
        public override void Render(Graphics g) {
            Rectangle rect = Shape.Data;
            IStyle style = this.Style;
            RenderType renderType = this.RenderType;
            if ((RenderType.Fill & renderType) != 0) {
                g.FillRectangle(GetSolidBrush(style.FillColor), rect);
                
            }
            if ((RenderType.Draw & renderType) != 0) {
                //if (Style.Pen.Alignment == System.Drawing.Drawing2D.PenAlignment.Center) {
                //    int penSize = -(int)Style.Pen.Width/2;
                //    Rectangle rect = Rectangle.Inflate(Shape.Data, penSize, penSize);
                //}
                g.DrawRectangle(style.Pen, rect);
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