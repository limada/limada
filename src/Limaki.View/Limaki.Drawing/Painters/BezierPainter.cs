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

namespace Limaki.Drawing.Painters {

    public class BezierPainter : RectanglePainter, IPainter<IBezierShape, Xwt.Rectangle> {

        public override void Render (ISurface surface) {
            var ctx = ((ContextSurface) surface).Context;
            var bezierPoints = (Shape as IBezierShape).BezierPoints;
            Render (ctx, (c, d) => {
                c.MoveTo (bezierPoints[0]);
                for (int i = 1; i < bezierPoints.Length; i += 3)
                    c.CurveTo (bezierPoints[i], bezierPoints[i + 1], bezierPoints[i + 2]);
                c.ClosePath ();
            });
        }
    }
}