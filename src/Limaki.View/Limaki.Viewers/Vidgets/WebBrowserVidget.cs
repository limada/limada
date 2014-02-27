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

using Limaki.Common;
using Limaki.View;
using System;
using System.IO;
using Xwt.Backends;

namespace Limaki.Viewers.Vidgets {

    [BackendType(typeof(IWebBrowserBackend))]
    public class WebBrowserVidget : Vidget, IWebBrowser, IHistoryAware {

        public class WebBrowserBackendHost : VidgetBackendHost {
            protected override IVidgetBackend OnCreateBackend () {
                this.ToolkitEngine.Backend.CheckInitialized();
                return Registry.Factory.Create<IWebBrowserBackend>();
            }
        }

        protected override VidgetBackendHost CreateBackendHost () {
            return new WebBrowserBackendHost();
        }

        public IWebBrowserBackend Backend { get { return BackendHost.Backend as IWebBrowserBackend; } }

        public Stream DocumentStream { get { return Backend.DocumentStream; } set { Backend.DocumentStream = value; } }

        public string DocumentText { get { return Backend.DocumentText; } set { Backend.DocumentText = value; } }

        public void MakeReady () { Backend.MakeReady (); }
        public Uri Url { get { return Backend.Url; } set { Backend.Url = value; } }
        public void Navigate (string urlString) { Backend.Navigate (urlString); }

        public void AfterNavigate (Func<bool> done) {
            Backend.AfterNavigate (done);
        }

        public bool CanGoBack { get { return Backend.CanGoBack; } }
        public bool CanGoForward { get { return Backend.CanGoForward; } }
        public bool GoBack () { return Backend.GoBack (); }
        public bool GoForward () { return Backend.GoForward (); }
        public void GoHome () { Backend.GoHome (); }

        public override void Dispose () {}
    }
}