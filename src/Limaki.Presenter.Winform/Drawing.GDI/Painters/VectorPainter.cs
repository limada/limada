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

using System.Drawing;
using Limaki.Drawing.Painters;
using Limaki.Drawing.Shapes;
using GDIPen=Limaki.Drawing.GDI.GDIPen;

namespace Limaki.Drawing.GDI.Painters {
    public class VectorPainter:Painter<Vector>,IPainter<IVectorShape,Vector> {
        SolidBrush _brush = null;
        SolidBrush GetSolidBrush(System.Drawing.Color color) {
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

        public override void Render( ISurface surface ) {
            if ((RenderType.Draw & RenderType) != 0) {
                Graphics g = ((GDISurface)surface).Graphics;
                Vector v = Shape.Data;
                System.Drawing.Pen pen = ( (GDIPen) Style.Pen ).Native;
                g.DrawLine(
                    pen, 
                    GDIConverter.Convert(v.Start),
                    GDIConverter.Convert(v.End));
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