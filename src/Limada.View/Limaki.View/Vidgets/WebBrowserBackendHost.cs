/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2015 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Common;

namespace Limaki.View.Vidgets {

    public class WebBrowserBackendHost : VidgetBackendHost {
        protected override IVidgetBackend OnCreateBackend () {
            this.ToolkitEngine.Backend.CheckInitialized ();
            // WebBrowserBackend needs special support of factory to decide which backend is available 
            return Registry.Factory.Create<IWebBrowserBackend> ();
        }
    }
}