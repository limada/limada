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
using Limaki.Drawing;
using Xwt;
using Xwt.Backends;

namespace Limaki.View {
    /// <summary>
    /// intermediate marker interface for Widgets
    /// will be replaced by Xwt.Widget in the future
    /// is like a controller for a concrete backend
    /// </summary>
    public interface IVidget : IDisposable {
        IVidgetBackend Backend { get; }
    }

    [BackendType(typeof(IVidgetBackend))]
    public abstract class Vidget : IVidget {

        private VidgetBackendHost _backendHost;
        public Vidget ()
		{
			_backendHost = CreateBackendHost ();
			_backendHost.Frontend = this;
		}
		
		protected virtual VidgetBackendHost CreateBackendHost ()
		{
            return new VidgetBackendHost();
		}

        protected VidgetBackendHost BackendHost {
			get { return _backendHost; }
		}

        public abstract void Dispose();

        public virtual IVidgetBackend Backend {
            get { return BackendHost.Backend; }
        }
    }
}
