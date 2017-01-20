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

using Xwt;
using Xwt.Backends;

namespace Limaki.View.Vidgets {
    [BackendType (typeof (IVidgetBackend))]
    public abstract class Vidget : IVidget {

        private VidgetBackendHost _backendHost;
        public Vidget () {
            _backendHost = CreateBackendHost ();
            _backendHost.Frontend = this;
        }

        protected virtual VidgetBackendHost CreateBackendHost () {
            return new VidgetBackendHost ();
        }

        protected VidgetBackendHost BackendHost {
            get { return _backendHost; }
        }

        public abstract void Dispose ();

        public virtual IVidgetBackend Backend {
            get { return BackendHost.Backend; }
        }

        public virtual Size Size { get { return Backend.Size; } }

        public virtual string ToolTipText { get { return Backend.ToolTipText; } set { Backend.ToolTipText = value; } }

        public virtual void SetFocus () { Backend.SetFocus (); }

        public virtual void Update () { Backend.Update (); }
        public virtual void QueueDraw () { Backend.QueueDraw (); }
        public virtual void QueueDraw (Rectangle rect) { Backend.QueueDraw (rect); }

    }
}