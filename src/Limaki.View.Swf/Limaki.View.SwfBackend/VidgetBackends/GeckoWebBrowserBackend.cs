/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008 - 2015 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Gecko;
using Limaki.Common;
using Limaki.Contents;
using Limaki.Contents.IO;
using Limaki.Drawing;
using Limaki.View;
using Limaki.View.Vidgets;
using Xwt.GdiBackend;
using Application = System.Windows.Forms.Application;
using Limaki.Common.Text.HTML;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public class GeckoWebBrowserBackend : VidgetBackend<GeckoWebBrowserBackend.WebBrowserEx>, IGeckoWebBrowserBackend, IZoomTarget, IHistoryAware, IContentSpec {

        public class WebBrowserEx : Gecko.GeckoWebBrowser {
            public WebBrowserEx () {
                new XulRunner ().Initialize ();
                Gecko.GeckoPreferences.Default["extensions.blocklist.enabled"] = false;
                //Gecko.GeckoPreferences.Default["pdfjs.disabled"] = false;
                this.DomKeyUp += new EventHandler<DomKeyEventArgs> (GeckoWebBrowser_DomKeyUp);
            }

            void GeckoWebBrowser_DomKeyUp (object sender, DomKeyEventArgs e) {
                char keyCode = Convert.ToChar (e.KeyCode);
                if (!e.ShiftKey && !e.AltKey && e.CtrlKey && keyCode == 'k') {
                    ZoomFactor = ZoomFactor * 1.1f;
                }
                if (!e.ShiftKey && !e.AltKey && e.CtrlKey && keyCode == 'm') {
                    ZoomFactor = ZoomFactor / 1.1f;
                }
            }

            public void WaitFor (Func<bool> done) {
                //if (!OS.Mono) {
                //    // try to resolve timing problems 
                //    // does not work so well, but better than nothing
                //    int i = 0;
                //    while (!done() && i < 10) {
                //        Thread.Sleep(5);
                //        i++;
                //    }
                //}
                Application.DoEvents ();
            }

            #region IWebBrowser Member

            public void MakeReady () {
                if (base.Document == null) {
                    base.Navigate ("about:blank");
                    BlockUntilNavigationFinished ();
                } else {
                    base.Stop ();
                }
            }

            public void WaitLoaded () {
                BlockUntilNavigationFinished ();
            }

            public string DocumentText
            {
                get { return base.Document.TextContent; }
                set
                {
                    var html = value;
                    if (value.StartsWith ("<html>"))
                        html = HtmlHelper.HtmUtf8Begin + value.Substring (6);

                    //InternalLoadContent (html, null, "text/html");
                    LoadHtml (html);
                    BlockUntilNavigationFinished ();

                }
            }

            protected void InternalLoadContent (string content, string url, string contentType) {
                using (var sContentType = new nsACString (contentType))
                    using (var sUtf8 = new nsACString ("UTF8")) {
                        ByteArrayInputStream inputStream = null;
                        try {
                            inputStream = ByteArrayInputStream.Create (System.Text.Encoding.UTF8.GetBytes (content != null ? content : string.Empty));

                            nsIDocShell docShell = Xpcom.QueryInterface<nsIDocShell> (this.WebBrowser);
                            nsIURI uri = null;
                            if (!string.IsNullOrEmpty (url))
                                uri = IOService.CreateNsIUri (url);
                            nsIDocShellLoadInfo l = null;
                            if (true) {
                                l = Xpcom.QueryInterface<nsIDocShellLoadInfo> (this.WebBrowser);

                                docShell.CreateLoadInfo (ref l);

                                l.SetLoadTypeAttribute (new IntPtr (16));
                            }

                            docShell.LoadStream (inputStream, uri, sContentType, sUtf8, l);
                            Marshal.ReleaseComObject (docShell);
                            if (l != null)
                                Marshal.ReleaseComObject (l);

                        } finally {
                            if (inputStream != null)
                                inputStream.Close ();
                        }
                    }
            }

            protected bool BlockUntilNavigationFinishedDone = false;

            protected void BlockUntilNavigationFinishedEvent (object sender, EventArgs e) {
                BlockUntilNavigationFinishedDone = true;
            }

            protected void BlockUntilNavigationFinished () {
                BlockUntilNavigationFinishedDone = false;
                this.DocumentCompleted -= BlockUntilNavigationFinishedEvent;
                this.DocumentCompleted += BlockUntilNavigationFinishedEvent;
                this.NavigationError -= BlockUntilNavigationFinishedEvent;
                this.NavigationError += BlockUntilNavigationFinishedEvent;
                while (!BlockUntilNavigationFinishedDone) {
                    Application.DoEvents ();
                    Application.RaiseIdle (new EventArgs ());
                }
            }

            public void Navigate (Uri url) {
                base.Navigate (url.AbsoluteUri);
            }

            public void Refresh (System.Windows.Forms.WebBrowserRefreshOption opt) {
                base.Refresh ();
            }

            public new string Url { get { return base.Url.AbsoluteUri; } set { Navigate (value); } }

            public Stream DocumentStream
            {
                get { throw new NotImplementedException (); }
                set
                {
                    value.Position = 0;
                    var reader = new StreamReader (value);
                    string text = reader.ReadToEnd ();
                    this.DocumentText = text;
                    value.Position = 0;
                }
            }

            public void GoHome () {
                base.Navigate ("about:blank");
            }

            public void Clear () {
                if (IsHandleCreated)
                    base.Navigate ("about:blank");
            }

            #endregion

            public double ZoomFactor { get { return this.Window.TextZoom; } set { this.Window.TextZoom = (float) value; } }

            protected override void Dispose (bool disposing) {
                base.Dispose (disposing);
            }

            #region Workarounds

            /// <summary>
            /// GeckoWebBrowser doesnt call OnEnter, OnLeave
            /// </summary>
            /// <param name="m"></param>
            protected override void WndProc (ref Message m) {

                base.WndProc (ref m);

                const int WM_GETDLGCODE = 0x87;
                const int DLGC_WANTALLKEYS = 0x4;
                const int WM_MOUSEACTIVATE = 0x21;
                const int MA_ACTIVATE = 0x1;
                const int WM_IME_SETCONTEXT = 0x0281;
                const int WM_PAINT = 0x000F;
                const int WM_SETFOCUS = 0x0007;
                const int WM_KILLFOCUS = 0x0008;


                const int ISC_SHOWUICOMPOSITIONWINDOW = unchecked((int) 0x80000000);
                if (!DesignMode) {
                    IntPtr focus;
                    switch (m.Msg) {
                        case WM_KILLFOCUS:
                            base.OnLeave (new EventArgs ());
                            break;
                        case WM_SETFOCUS:
                        case WM_MOUSEACTIVATE:
                            base.OnGotFocus (new EventArgs ());
                            break;
                        case WM_IME_SETCONTEXT:
                            if (WebBrowserFocus != null) {
                                if (m.WParam == IntPtr.Zero)
                                    base.OnLeave (new EventArgs ());
                                else
                                    base.OnGotFocus (new EventArgs ());
                            }
                            break;
                    }
                }
            }

            #endregion
        }

        #region IVidgetBackend-Implementation

        public new WebBrowserVidget Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (WebBrowserVidget)frontend;
        }
        
        #endregion

        IEnumerable<ContentInfo> _supportedContents = null;
        public IEnumerable<ContentInfo> ContentSpecs {
            get {
                return _supportedContents ?? (_supportedContents =
                    new HtmlContentSpot().ContentSpecs
                    //.Union(new PdfContentSpot().ContentSpecs)
                    .ToArray()
                );
            }
        }

        public Stream DocumentStream { get { return Control.DocumentStream; } set { Control.DocumentStream = value; } }

        public string DocumentText { get { return Control.DocumentText; } set { Control.DocumentText = value; } }

        public string Url { get { return Control.Url; } set { Control.Url = value; } }

        public bool CanGoBack { get { return Control.CanGoBack; } }

        public bool CanGoForward { get { return Control.CanGoForward; } }

        public void Navigate (string urlString) {
            Control.Navigate (urlString);
        }

        public void WaitFor (Func<bool> done) {
            Control.WaitFor (done);
        }

        public void WaitLoaded () {
            Control.WaitLoaded ();
        }

        public void MakeReady () {
            Control.MakeReady ();
        }

        public void Clear () {
            Control.Clear ();
        }

        public bool GoBack () {
            return Control.GoBack ();
        }

        public bool GoForward () {
            return Control.GoForward ();
        }

        public void GoHome () {
            Control.GoHome ();
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
        
        public double ZoomFactor { get { return Control.ZoomFactor; } set { Control.ZoomFactor = value; } }

        public void UpdateZoom () {
            Control.Window.TextZoom = (float) ZoomFactor;
        }

        #endregion


        #region IWebBrowserWithProxy Members

        public void SetProxy (IPAddress adress, int port, object webBrowser) {
            var control = webBrowser as GeckoWebBrowserBackend;
            if (control != null) {
                var prefs = GeckoPreferences.User;

                prefs["network.proxy.http"] = adress.ToString ();
                prefs["network.proxy.http_port"] = port;

                if (false) {
                    prefs["network.proxy.ssl"] = adress.ToString ();
                    prefs["network.proxy.ssl_port"] = port;
                }

                prefs["network.proxy.no_proxies_on"] = "";
                prefs["network.proxy.type"] = 1;


            }
        }

        #endregion
    }
}
