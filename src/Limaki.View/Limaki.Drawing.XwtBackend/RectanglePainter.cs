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

using Limaki.Drawing;
using Xwt;
using Limaki.Drawing.Shapes;

namespace Limaki.Drawing.XwtBackend {
    public class RectanglePainter : ContextPainter<Rectangle>, IPainter<IRectangleShape, Rectangle> {

        public override void Render (ISurface surface) {
            var ctx = ((ContextSurface) surface).Context;
            Render (ctx, (c, d) => c.Rectangle (d));
        }
    }
}