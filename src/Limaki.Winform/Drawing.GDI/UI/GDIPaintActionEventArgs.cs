/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


using System;
using System.Drawing;
using Limaki.Drawing.UI;

namespace Limaki.Drawing.GDI.UI {
    public class GDIPaintActionEventArgs : PaintActionEventArgsBase {
        private Graphics graphics = null;
        #region Public Constructors
        public GDIPaintActionEventArgs(Graphics graphics, Rectangle clipRect) {
            if (graphics == null)
                throw new ArgumentNullException("graphics");
            this.graphics = graphics;
            this.clip_rectangle = GDIConverter.Convert(clipRect);
            this.surface = new GDISurface() { Graphics = graphics };
        }



        #endregion	// Public Constructors

        /// <summary>
        /// sets the graphics
        ///  </summary>
        /// <param name="g"></param>
        /// <returns>the previouse graphics</returns>
        internal Graphics SetGraphics(Graphics g) {
            Graphics res = graphics;
            graphics = g;

            return res;
        }
    }
}