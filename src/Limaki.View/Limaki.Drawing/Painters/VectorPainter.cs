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

using System;
using Limaki.Drawing.Shapes;
using Xwt.Drawing;
using Xwt;

namespace Limaki.Drawing.Painters {

    public class VectorPainter : Painter<Vector>, IPainter<IVectorShape, Vector> {

        public override void Render (ISurface surface) {
            var ctx = ((ContextSurface) surface).Context;
            var vector = Shape.Data;

            var width = this.Style.Pen.Thickness;
            var arrowHeigth = width * 5.5d;
            var arrowWidth = width * 1.5d;
            var end = DrawArrow (ctx, vector, arrowWidth, arrowHeigth);
            ctx.SetColor (Style.PenColor);
            ctx.Fill ();
        
            this.RenderType = RenderType.Draw;

            Render (ctx, (c, d) => {
                c.MoveTo (vector.Start);
                c.LineTo (end);
            });
        }

        protected Matrix Matrix = new Matrix ();

        public Point DrawArrow (Context ctx, Vector v, double w, double h) {
            if (h == 0 || w == 0)
                throw new ArgumentException ("ArrowWidth must not be 0");

            Point[] arrow =  {
                            new Point (0, 0),
                            new Point (-w, -h),
                            new Point (w, -h),
                            new Point (0, -h)
                        };
            Matrix.SetIdentity ();
            var angle = Vector.Angle (v) + (v.Start.X - v.End.X > 0 ? 90d : -90d);
            Matrix.RotateAppend (angle);
            Matrix.TranslateAppend (v.End.X, v.End.Y);
            Matrix.Transform (arrow);
            ctx.MoveTo (arrow[0]);
            ctx.LineTo (arrow[1]);
            ctx.LineTo (arrow[2]);
            ctx.ClosePath ();
            return arrow[3];
        }
    }
}