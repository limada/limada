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

using System;
using Limaki.View.Vidgets;
using Limaki.View.XwtBackend;

namespace Limaki.View.GtkBackend {
    
    public interface IGtkBackend {
        Gtk.Widget Widget { get; }
    }

    public abstract class VidgetBackend<T> : IVidgetBackend, IGtkBackend where T : Gtk.Widget, new () {

        public IVidget Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = frontend;
        }

        IVidgetEventSink EventSink { get; set; }
        public void InitializeEvents (IVidgetEventSink eventSink) {
            EventSink = eventSink;
            Widget.FocusInEvent += (s, e) => EventSink?.OnEvent (nameof (IVidget.GotFocus), new EventArgs ());
            Widget.ButtonReleaseEvent += (s, e) => EventSink?.OnEvent (nameof (IVidget.ButtonReleased), e.ToXwt ((Gtk.Widget)s).ToLmk ());
        }

        public VidgetBackend () {
            Compose ();
        }

        protected virtual void Compose () {
            Widget = new T ();
        }

        public T Widget { get; protected set; }

        public Xwt.Size Size {
            get { return Widget.VidgetBackendSize (); }
            set { Widget.VidgetBackendSize (value); }
        }

        public string ToolTipText { get { return Widget.TooltipText; } set { Widget.TooltipText = value; } }

        public void Update () { Widget.VidgetBackendUpdate (); }

        public void QueueDraw () { Widget.VidgetBackendInvalidate (); }

        public void SetFocus () { Widget.VidgetBackendSetFocus (); }

        public void QueueDraw (Xwt.Rectangle rect) { Widget.VidgetBackendInvalidate (rect); }

        public virtual void Dispose () {
            Widget.Dispose ();
        }

        Gtk.Widget IGtkBackend.Widget {
            get { return this.Widget; }
        }

    }
}