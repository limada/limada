/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008 - 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace Limaki.View.SwfBackend.VidgetBackends {

    public class GeckoWebBrowserBackend : Gecko.GeckoWebBrowser, IGeckoWebBrowserBackend, IZoomTarget, IHistoryAware, IContentSpec {

        public GeckoWebBrowserBackend() {
            new XulRunner().Initialize();
            Gecko.GeckoPreferences.Default["extensions.blocklist.enabled"] = false;
            //Gecko.GeckoPreferences.Default["pdfjs.disabled"] = false;
            this.DomKeyUp += new EventHandler<DomKeyEventArgs>(GeckoWebBrowser_DomKeyUp);
        }

        void GeckoWebBrowser_DomKeyUp (object sender, DomKeyEventArgs e) {
            char keyCode = Convert.ToChar(e.KeyCode);
            if (!e.ShiftKey && !e.AltKey && e.CtrlKey && keyCode == 'k') {
                ZoomFactor = ZoomFactor * 1.1f;
            }
            if (!e.ShiftKey && !e.AltKey && e.CtrlKey && keyCode == 'm') {
                ZoomFactor = ZoomFactor / 1.1f;
            }
        }

        public void WaitFor (Func<bool> done) {
            if (!OS.Mono) {
                // try to resolve timing problems 
                // does not work so well, but better than nothing
                int i = 0;
                while (!done() && i < 10) {
                    Thread.Sleep(5);
                    i++;
                }
            }
            // fails with IExplorer, not necessry with gecko:
            // control.Refresh();
            Application.DoEvents();
        }

        #region IWebBrowser Member

        public void MakeReady() {
            if (base.Document == null) {
                base.Navigate("about:blank");
                
                for (int i = 0; i < 200 && base.IsBusy; i++) {
                    Application.DoEvents();
                    Thread.Sleep(5);
                }
            } else {
                base.Stop();
            }
        }

        public string DocumentText {
            get { return base.Document.TextContent; }
            set {
                SetDocumentTextOverAboutBlank (value);
                //SetDocumentTextOverPostData (value);
            }
        }

        void SetDocumentTextOverAboutBlank(string content) {
            if (base.Document == null) {
                base.Navigate("about:blank");
            }
            for (int i = 0; i < 200 && base.IsBusy; i++) {
                Application.DoEvents();
                Thread.Sleep(5);
            }

            //base.Document.DocumentElement.InnerHtml = content;

            //does nothing: base.Document.TextContent = content;
        }

        public void Navigate (Uri url) {
            base.Navigate(url.AbsoluteUri);
        }

        public void Refresh (System.Windows.Forms.WebBrowserRefreshOption opt) {
            base.Refresh();
        }

        public new string Url {
            get { return base.Url.AbsoluteUri; }
            set { Navigate (value); }
        }

        public Stream DocumentStream {
            get {
                throw new NotImplementedException ();
            }
            set {
                using (var reader = new StreamReader (value)) {
                    string text = reader.ReadToEnd ();
                    this.DocumentText = text;
                }
            }
        }

        public void GoHome () { base.Navigate ("about:blank"); }

        public void Clear () {
            if (IsHandleCreated)
                base.Navigate ("about:blank");
        }

        #endregion

        #region IZoomTarget Member

        public ZoomState ZoomState {
            get {return ZoomState.Original;}
            set {
                if (value == ZoomState.Original) {
                    ZoomFactor = 1.0d;
                }
            }
        }

        
        public double ZoomFactor {
            get { return base.Window.TextZoom; }
            set { base.Window.TextZoom = (float)value; }
        }

        public void UpdateZoom() {
            base.Window.TextZoom = (float)ZoomFactor;
        }

        #endregion

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
        }

        #region IWebBrowserWithProxy Members

        public void SetProxy(IPAddress adress, int port, object webBrowser) {
            var control = webBrowser as GeckoWebBrowserBackend;
            if (control != null) {
                var prefs = GeckoPreferences.User;

                prefs["network.proxy.http"] = adress.ToString();
                prefs["network.proxy.http_port"] = port;

                if (false) {
                    prefs["network.proxy.ssl"] = adress.ToString();
                    prefs["network.proxy.ssl_port"] = port;
                }

                prefs["network.proxy.no_proxies_on"] = "";
                prefs["network.proxy.type"] = 1;


            }
        }

        #endregion

        #region Workarounds

        /// <summary>
        /// GeckoWebBrowser doesnt call OnEnter, OnLeave
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc (ref Message m) {

            base.WndProc(ref m);
            
            const int WM_GETDLGCODE = 0x87;
            const int DLGC_WANTALLKEYS = 0x4;
            const int WM_MOUSEACTIVATE = 0x21;
            const int MA_ACTIVATE = 0x1;
            const int WM_IME_SETCONTEXT = 0x0281;
            const int WM_PAINT = 0x000F;
            const int WM_SETFOCUS = 0x0007;
            const int WM_KILLFOCUS = 0x0008;


            const int ISC_SHOWUICOMPOSITIONWINDOW = unchecked((int)0x80000000);
            if (!DesignMode) {
                IntPtr focus;
                switch (m.Msg) {
                    case WM_KILLFOCUS:
                        base.OnLeave(new EventArgs());
                        break;
                    case WM_SETFOCUS:
                    case WM_MOUSEACTIVATE:
                        base.OnGotFocus(new EventArgs());
                        break;
                    case WM_IME_SETCONTEXT:
                        if (WebBrowserFocus != null) {
                            if (m.WParam == IntPtr.Zero)
                                base.OnLeave(new EventArgs());
                            else
                                base.OnGotFocus(new EventArgs());
                        }
                        break;
                }
            }
        }

        #endregion
       
        #region IVidgetBackend-Implementation

        public WebBrowserVidget Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (WebBrowserVidget)frontend;
        }

        Xwt.Size IVidgetBackend.Size {
            get { return this.Size.ToXwt(); }
        }


        void IVidgetBackend.Invalidate (Xwt.Rectangle rect) {
            this.Invalidate(rect.ToGdi());
        }

        void IVidgetBackend.SetFocus () { this.Focus (); }

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

     
    }
}
