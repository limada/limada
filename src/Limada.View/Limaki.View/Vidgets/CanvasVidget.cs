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
using Xwt;
using Xwt.Backends;
using Xwt.Drawing;

namespace Limaki.View.Vidgets {
    
    [BackendType(typeof(ICanvasVidgetBackend))]
    public class CanvasVidget : Vidget, ICanvasVidget {

        public virtual new ICanvasVidgetBackend Backend { get { return BackendHost.Backend as ICanvasVidgetBackend; } }

        public override void Dispose () { }

        #region Draw

        public event EventHandler<ContextEventArgs> Draw;

        void ICanvasVidget.DrawContext (Context context, Rectangle dirtyRect) {
            OnDraw(new ContextEventArgs(context, dirtyRect));
        }

        /// <summary>
        /// calls Draw-event, if set
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDraw (ContextEventArgs e) {
            if (Draw != null)
                Draw(this, e);
        }

        #endregion
    }

    public class ContextEventArgs : EventArgs {

        public ContextEventArgs (Context context, Rectangle clip) {
            this.Context = context;
            this.DirtyRectangle = clip;
        }

        public Context Context { get; protected set; }
        public Rectangle DirtyRectangle { get; protected set; }
    }

    public interface ICanvasVidgetBackend : IVidgetBackend {}

    public interface ICanvasVidget : IVidget {
        void DrawContext (Context context, Rectangle dirtyRect);
    }

}