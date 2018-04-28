/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012 - 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Limaki.Common;
using Limaki.Contents;
using Limaki.Contents.IO;
using Limaki.Drawing;
using Limaki.View.Common;
using Limaki.View.Vidgets;
using Xwt;
using Xwt.WPFBackend;


namespace Limaki.View.WpfBackend {

    public class GeckoWebBrowserBackend : Xwt.WPFBackend.WidgetBackend, IWebBrowserWidgetBackend, IWebBrowser, IHistoryAware, IWebBrowserWithProxy, IZoomTarget {

        protected GeckoWebBrowserEx Browser;

        public GeckoWebBrowserBackend () : this (new GeckoWebBrowserEx ()) { }

        internal GeckoWebBrowserBackend (GeckoWebBrowserEx browser) {
            this.Widget = browser;
            this.Browser = browser;
        }

        #region IZoomTarget Member

        public ZoomState ZoomState {
            get { return ZoomState.Original; }
            set {
                if (value == ZoomState.Original) {
                    ZoomFactor = 1.0d;
                }
            }
        }

        public double ZoomFactor { get { return Browser.Window.TextZoom; } set { Browser.Window.TextZoom = (float) value; } }

        public void UpdateZoom () { Browser.Window.TextZoom = (float) ZoomFactor; }

        #endregion

        public Stream DocumentStream { get { return Browser.DocumentStream; } set { Browser.DocumentStream = value; } }

        public string DocumentText { get { return Browser.DocumentText; } set { Browser.DocumentText = value; } }

        public string Url { get { return Browser.Url; } set { Browser.Url = value; } }
        
        public void Navigate (string urlString) { Browser.Navigate (urlString); }

        public void WaitFor (System.Func<bool> done) { Browser.WaitFor (done); }

        public void WaitLoaded () { Browser.WaitLoaded (); }

        public void MakeReady () { Browser.MakeReady (); }
        
        public bool CanGoBack { get { return Browser.CanGoBack; } }

        public bool CanGoForward { get { return Browser.CanGoForward; } }

        public bool GoBack () { return Browser.GoBack (); }

        public bool GoForward () { return Browser.GoForward (); }

        public void GoHome () { Browser.GoHome (); }

        public void Clear () { Browser.Clear (); }

        public void SetProxy (System.Net.IPAddress adress, int port, object webBrowser) { Browser.SetProxy (adress, port, webBrowser); }
    }
}