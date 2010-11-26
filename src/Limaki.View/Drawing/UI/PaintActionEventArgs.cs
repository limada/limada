/*
 * Limaki 
 * Version 0.08
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
using Limaki.Drawing;

namespace Limaki.Drawing.UI {
    public class PaintActionEventArgsBase : EventArgs, IPaintActionEventArgs, IDisposable {
        
        protected RectangleI clip_rectangle;
        private bool disposed;

        protected ISurface surface = null;
        public ISurface Surface {
            get { return this.surface; }
        } 


        #region Public Instance Properties
        public RectangleI ClipRectangle {
            get { return this.clip_rectangle; }
        }



        #endregion	

        #region Public Instance Methods
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion	



        internal void SetClip(RectangleI clip) {
            clip_rectangle = clip;
        }

        #region Protected Instance Methods
        ~PaintActionEventArgsBase() {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposed) {
                disposed = true;
            }
        }
        #endregion	// Protected Instance Methods
    }
}