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
using Xwt;

namespace Limaki.View.XwtBackend {

    public abstract class VidgetBackend<T> : IVidgetBackend, IXwtBackend where T : Widget, new () {

        public virtual IVidget Frontend { get; protected set; }

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

    
}