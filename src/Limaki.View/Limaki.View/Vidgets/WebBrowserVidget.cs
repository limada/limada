/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 - 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using Limaki.Contents;
using Limaki.Contents.IO;
using Xwt.Backends;

namespace Limaki.View.Vidgets {

    [BackendType(typeof(IWebBrowserBackend))]
    public class WebBrowserVidget : Vidget, IWebBrowser, IHistoryAware {
        
        protected override VidgetBackendHost CreateBackendHost () {
            return new WebBrowserBackendHost();
        }

        private IEnumerable<ContentInfo> _supportedContents = null;

        public IEnumerable<ContentInfo> ContentSpecs {
            get {
                return _supportedContents ?? (_supportedContents =
                    new HtmlContentSpot ().ContentSpecs
                    #if DEBUG
                    .Union (new PdfContentSpot ().ContentSpecs)
                    #endif
                    .ToArray ()
                    );
            }
        }

        public new IWebBrowserBackend Backend { get { return BackendHost.Backend as IWebBrowserBackend; } }

        public Stream DocumentStream { get { return Backend.DocumentStream; } set { Backend.DocumentStream = value; } }

        public string DocumentText { get { return Backend.DocumentText; } set { Backend.DocumentText = value; } }

        public void MakeReady () { Backend.MakeReady (); }

        public string Url { get { return Backend.Url; } set { Backend.Url = value; } }

        public void Navigate (string urlString) { Backend.Navigate (urlString); }

        public void WaitFor (Func<bool> done) { Backend.WaitFor (done); }

        public bool CanGoBack { get { return Backend.CanGoBack; } }

        public bool CanGoForward { get { return Backend.CanGoForward; } }

        public bool GoBack () { return Backend.GoBack (); }

        public bool GoForward () { return Backend.GoForward (); }

        public void GoHome () { Backend.GoHome (); }

        public override void Dispose () {}

        public void Clear () { Backend.Clear (); }
    }
}