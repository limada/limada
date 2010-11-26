/*
 * Limaki 
 * Version 0.063
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

using System.Drawing;
using Limaki.Drawing.Shapes;

namespace Limaki.Drawing.Painters {
    public class VectorPainter:Painter<Vector> {
        SolidBrush _brush = null;
        SolidBrush GetSolidBrush(Color color) {
            if ((_brush != null) && (_brush.Color == color)) {
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
            if ((RenderType.Draw & RenderType) != 0) {
                Vector v = Shape.Data;
                g.DrawLine(Style.Pen, v.Start,v.End);
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