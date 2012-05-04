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

using Limaki.Drawing.Shapes;
using Xwt;
using Xwt.Drawing;
using System;

namespace Limaki.Drawing.Painters {

    public class RoundedRectanglePainter : RectanglePainter, IPainter<IRoundedRectangleShape, Rectangle> {

        public override void Render (ISurface surface) {
            var ctx = ((ContextSurface) surface).Context;
            Render (ctx, (c, d) => SetRoundedRect (c, d, 10d));
        }

        protected virtual void SetRoundedRect (Context ctx, Rectangle rectangle, double radius) {
            // if corner radius is less than or equal to zero, 
            // return the original Rectangle 
            if (radius <= 0.0d) {
                ctx.Rectangle (rectangle);
                return;
            }

            // if the corner radius is greater than or equal to 
            // half the width, or height (whichever is shorter) 
            // then return a capsule instead of a lozenge 
            if (radius >= (Math.Min (rectangle.Width, rectangle.Height)) / 2.0) {
                GetCapsule (ctx, rectangle);
                return;
            }

            // create the arc for the Rectangle sides 
            var l = rectangle.Left;
            var t = rectangle.Top;
            var w = rectangle.Width;
            var h = rectangle.Height;

            // top left  
            ctx.Arc (l + radius, t + radius, radius, 180, 270);
            // top right 
            ctx.Arc (l + w - radius, t + radius, radius, 270, 0);
            // bottom right  
            ctx.Arc (l + w - radius, t + h - radius, radius, 0, 90);
            // bottom left 
            ctx.Arc (l + radius, t + h - radius, radius, 90, 180);

            ctx.ClosePath ();

        }

        protected virtual void GetCapsule (Context ctx, Rectangle rectangle) {
            var radius = 0d;
            var l = rectangle.Left;
            var t = rectangle.Top;
            var w = rectangle.Width;
            var h = rectangle.Height;
            try {
                if (rectangle.Width > rectangle.Height) {
                    // return horizontal capsule 
                    radius = h / 2d;
                    ctx.Arc (l + radius, t + radius, radius, 90, 270);
                    ctx.Arc (l + w - radius, t + radius, radius, -90, 90);
                } else if (rectangle.Width < rectangle.Height) {
                    // return vertical capsule
                    radius = w / 2d;
                    ctx.Arc (l + radius, t + radius, radius, 180, 360);
                    ctx.Arc (l + radius, t + h - radius, radius, 0, 180);

                } else {
                    // return circle
                    radius = h / 2d;
                    ctx.Arc (l + radius, t + radius, radius, 0, 360);
                }
            } catch {
                radius = h / 2d;
                ctx.Arc (l + radius, t + radius, radius, 0, 360);
            } finally {
                ctx.ClosePath ();
            }
        }
    }
}