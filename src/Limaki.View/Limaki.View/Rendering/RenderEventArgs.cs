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


using System;
using Limaki.Drawing;

namespace Limaki.View.Rendering {
    public class RenderEventArgs : EventArgs, IRenderEventArgs, IDisposable {

        public IClipper Clipper {
            get { return this._clipper; }
        }

        protected IClipper _clipper;
        internal void SetClipper(IClipper clipper) {
            _clipper = clipper;
        }        

        protected ISurface _surface = null;
        public ISurface Surface {
            get { return this._surface; }
        } 


        #region IDispose Members
        ~RenderEventArgs() {
            Dispose(false);
        }

        private bool disposed;
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposed) {
                disposed = true;
            }
        }
        #endregion
    }
}

