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


using Limaki.Common;
using Xwt.Drawing;

namespace Limaki.View.Rendering {
    /// <summary>
    /// DeviceRenderers are responible
    /// to manage the rendering methods of a device
    /// They are device-specific (eg. winform, wpf...)
    /// </summary>
    public interface IDeviceRenderer {
        /// <summary>
        /// invokes the render-method of the control
        /// </summary>
        void Render();

        /// <summary>
        /// invokes the render-method of the control
        /// </summary>
        /// <param name="clipper"></param>
        void Render(IClipper clipper);


        Get<Color> BackColor { get; set; }

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