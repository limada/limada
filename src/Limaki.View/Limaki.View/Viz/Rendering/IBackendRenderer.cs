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
 * http://www.limada.org
 * 
 */

using Xwt.Drawing;
using System;

namespace Limaki.View.Viz.Rendering {
    /// <summary>
    /// BackendRenderers are responible
    /// to manage the rendering methods of a backend
    /// They are device-specific (eg. swf, wpf...)
    /// </summary>
    public interface IBackendRenderer {
        /// <summary>
        /// invokes the render-method of the Backend
        /// </summary>
        void Render();

        /// <summary>
        /// invokes the render-method of the Backend
        /// </summary>
        /// <param name="clipper"></param>
        void Render(IClipper clipper);

        Func<Color> BackColor { get; set; }

        /// <summary>
        /// prepares the PaintEventArgs
        /// prepares the Surface
        /// paints the background
        /// calls EventController.OnPaint()
        /// </summary>
        /// <param name="e"></param>
        void OnPaint ( IRenderEventArgs e );
    }
}