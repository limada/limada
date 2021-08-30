/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Drawing.XwtBackend;
using Limaki.View.Viz.Rendering;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.XwtBackend {

    public class RenderContextEventArgs : RenderEventArgs {

        private Context _context = null;

        public RenderContextEventArgs(Context context, Rectangle clipRect) {
            if (context == null)
                throw new ArgumentNullException(nameof (context));
            this._context = context;
            this._clipper = new BoundsClipper(clipRect);
            this._surface = new ContextSurface { Context = context };
        }

    }
}