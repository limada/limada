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
using Xwt;

namespace Limaki.View.XwtBackend {
    
    public abstract class VidgetBackend<T> : IVidgetBackend, IXwtBackend where T : Widget, new () {

        public virtual IVidget Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = frontend;
        }
        protected IVidgetEventSink EventSink { get; set; }
        public virtual void InitializeEvents (IVidgetEventSink eventsink) {
            EventSink = eventsink;
            Widget.ComposeEvents (EventSink);
        }

        public VidgetBackend () {
            Compose ();
        }

        protected virtual void Compose () {
            Widget = new T ();
        }

        public T Widget { get; protected set; }

        public Size Size {
            get { return Widget.Size; }
            set {
                Widget.WidthRequest = value.Width;
                Widget.HeightRequest = value.Height;
            }
        }

        public string ToolTipText {
            get { return Widget.TooltipText; }
            set { Widget.TooltipText = value; }
        }

        public virtual void Update () { Widget.VidgetBackendUpdate (); }

        public virtual void QueueDraw () { Widget.VidgetBackendInvalidate (); }

        public virtual void SetFocus () { Widget.SetFocus (); }

        public virtual void QueueDraw (Xwt.Rectangle rect) { Widget.VidgetBackendInvalidate (rect); }

        public virtual void Dispose () {
            Widget.Dispose ();
        }

        Widget IXwtBackend.Widget {
            get { return this.Widget; }
        }
        
    }

    public static class XwtVidgetBackendExtensions {

        public static void ComposeEvents (this Widget Widget, IVidgetEventSink EventSink) {

            Widget.GotFocus += (s, e) => EventSink?.OnEvent (nameof (IVidget.GotFocus), new EventArgs ());
            Widget.ButtonReleased += (s, e) => EventSink?.OnEvent (nameof (IVidget.ButtonReleased), e.ToLmk ());

        }
    }

}