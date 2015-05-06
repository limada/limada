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

using Limaki.View.Vidgets;

namespace Limaki.View.GtkBackend {
    public interface IGtkBackend {
        Gtk.Widget Widget { get; }
    }

    public abstract class VidgetBackend<T> : IVidgetBackend, IGtkBackend where T : Gtk.Widget, new () {

        public IVidget Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = frontend;
        }

        public VidgetBackend () {
            Compose ();
        }

        protected virtual void Compose () {
            this.Widget = new T ();
        }

        public T Widget { get; protected set; }

        public Xwt.Size Size {
            get { return Widget.VidgetBackendSize (); }
            set { Widget.VidgetBackendSize (value); }
        }

        public void Update () { Widget.VidgetBackendUpdate (); }

        public void Invalidate () { Widget.VidgetBackendInvalidate (); }

        public void SetFocus () { Widget.VidgetBackendSetFocus (); }

        public void Invalidate (Xwt.Rectangle rect) { Widget.VidgetBackendInvalidate (rect); }

        public virtual void Dispose () {
            Widget.Dispose ();
        }

        Gtk.Widget IGtkBackend.Widget {
            get { return this.Widget; }
        }

    }
}