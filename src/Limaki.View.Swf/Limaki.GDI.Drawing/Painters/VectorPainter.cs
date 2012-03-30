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
    public class VectorPainter:GdiPainter<Vector>,IPainter<IVectorShape,Vector> {

        public override void Render (ISurface surface) {
            RenderGdi (surface);
        }
        public override void RenderGdi( ISurface surface ) {
            if ((RenderType.Draw & RenderType) != 0) {
                Graphics g = ((GDISurface)surface).Graphics;
                Vector v = Shape.Data;
                System.Drawing.Pen pen = ( (GDIPen) Style.Pen ).Backend;
                g.DrawLine(
                    pen, 
                    GDIConverter.Convert(v.Start),
                    GDIConverter.Convert(v.End));
            }
        }

        public override void RenderXwt (ISurface surface) {
            throw new System.NotImplementedException ();
        }

        
    }
}