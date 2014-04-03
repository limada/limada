/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Drawing;
using Limaki.GdiBackend;
using Limaki.View.Viz.Rendering;
using Xwt.GdiBackend;

namespace Limaki.SwfBackend.Viz {

    public class GdiRenderEventArgs : RenderEventArgs {

        private Graphics graphics = null;

        public GdiRenderEventArgs (Graphics graphics, Rectangle clipRect) {
            if (graphics == null)
                throw new ArgumentNullException ("graphics");
            this.graphics = graphics;
            this._clipper = new BoundsClipper (clipRect.ToXwt ());
            this._surface = new GdiSurface { Graphics = graphics };
        }

    }
}