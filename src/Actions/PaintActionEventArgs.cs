/*
 * Limaki 
 * Version 0.064
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

/* This code is derived from
 * 
 * Mono 1.6
 * 
 */

// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Copyright (c) 2004 Novell, Inc.
//
// Authors:
//	Peter Bartok	pbartok@novell.com
//


// COMPLETE

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Limaki.Actions {
    public class PaintActionEventArgs : EventArgs, IDisposable {
        private Graphics graphics;
        private Rectangle clip_rectangle;
        private bool disposed;

        #region Public Constructors
        public PaintActionEventArgs(Graphics graphics, Rectangle clipRect) {
            if (graphics == null)
                throw new ArgumentNullException("graphics");
            this.graphics = graphics;
            this.clip_rectangle = clipRect;
        }

        public PaintActionEventArgs(Graphics graphics, Rectangle clipRect, Region clipRegion) {
            if (graphics == null)
                throw new ArgumentNullException("graphics");
            this.graphics = graphics;
            this.clip_rectangle = clipRect;
            this._clipRegion = clipRegion;
        }
        public PaintActionEventArgs(Graphics graphics, Rectangle clipRect, GraphicsPath clipPath):this(graphics,clipRect) {
            this.clip_path = clipPath;
        }
        #endregion	// Public Constructors

        #region Public Instance Properties
        public Rectangle ClipRectangle {
            get { return this.clip_rectangle; }
        }

        GraphicsPath clip_path = null;
        public GraphicsPath ClipPath {
            get { return this.clip_path; }
            set { this.clip_path=value; }
        }

        Region _clipRegion = null;
        public Region ClipRegion {
            get { return this._clipRegion; }
            set { this._clipRegion = value; }
        }

        public Graphics Graphics {
            get { return this.graphics; }
        }
        #endregion	

        #region Public Instance Methods
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion	

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

        internal void SetClip(Rectangle clip) {
            clip_rectangle = clip;
        }

        #region Protected Instance Methods
        ~PaintActionEventArgs() {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposed) {
                //#if !NET_2_0

                //                if (graphics != null)
                //                    graphics.Dispose ();
                //#endif
                disposed = true;
            }
        }
        #endregion	// Protected Instance Methods
    }
}