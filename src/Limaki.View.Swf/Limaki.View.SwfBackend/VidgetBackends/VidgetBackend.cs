/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Windows.Forms;
using Limaki.View;
using Limaki.View.Vidgets;
using Xwt.GdiBackend;
using Xwt;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public interface ISwfBackend {
        Control Control { get; }
    }

    public abstract class VidgetBackend<T> : IVidgetBackend, ISwfBackend where T : Control, new () {

        public IVidget Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = frontend;
        }

        public VidgetBackend () {
            Compose ();
        }

        protected virtual void Compose () {
            this.Control = new T ();
        }

        public T Control { get; protected set; }

        public Xwt.Size Size {
            get { return Control.Size.ToXwt (); }
            set { Control.Size = value.ToGdi (); }
        }

        public virtual string ToolTipText { get; set; }

        public virtual void Invalidate (Xwt.Rectangle rect) {
            Control.Invalidate (rect.ToGdi ());
        }

        public virtual void SetFocus () { Control.Focus (); }

        public virtual void Update () {
            Control.Update ();
        }

        public virtual void Invalidate () {
            Control.Invalidate ();
        }

        public virtual void Dispose () {
            Control.Dispose ();
        }

        Control ISwfBackend.Control {
            get { return this.Control; }
        }
    }
}