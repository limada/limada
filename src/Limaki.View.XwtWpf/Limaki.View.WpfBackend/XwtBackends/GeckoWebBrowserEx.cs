using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using Gecko;
using Limaki.Contents;
using Limaki.View.Vidgets;

namespace Limaki.View.WpfBackend {

    public class GeckoWebBrowserEx : Gecko.GeckoWebBrowser {

        public GeckoWebBrowserEx () {
            new XulRunner ().Initialize ();
            Gecko.GeckoPreferences.Default["extensions.blocklist.enabled"] = false;
            //Gecko.GeckoPreferences.Default["pdfjs.disabled"] = false;
            //TODO: this.DomKeyUp += new EventHandler<DomKeyEventArgs> (GeckoWebBrowser_DomKeyUp);
            base.DocumentCompleted += (s, e) => IsBusy = false;
        }

        public double ZoomFactor { get { return Window.TextZoom; } set { Window.TextZoom = (float) value; } }

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
            int i = 0;
            while (!done () && i < 10) {
                Thread.Sleep (5);
                i++;
            }
            WpfExtensions.DoEvents ();
        }

        #region IWebBrowser Member

        public void MakeReady () {
            if (!IsHandleCreated)
                WpfExtensions.DoEvents ();
            if (base.Document == null) {
                
                base.Navigate ("about:blank");

                for (int i = 0; i < 200 && IsBusy; i++) {
                    WpfExtensions.DoEvents ();
                    Thread.Sleep (5);
                }
            } else {
                Stop ();
            }
        }

       
        public string DocumentText {
            get { return base.Document.TextContent; }
            set {
                SetDocumentTextOverAboutBlank (value);
                //SetDocumentTextOverPostData (value);
            }
        }

        void SetDocumentTextOverAboutBlank (string content) {
            if (base.Document == null) {
                base.Navigate ("about:blank");
            }
            for (int i = 0; i < 200 && IsBusy; i++) {
                WpfExtensions.DoEvents ();
                Thread.Sleep (5);
            }
        }

        public void Navigate (Uri url) {
            base.Navigate (url.AbsoluteUri);
        }


        public string Url {
            get { return WebNav.GetCurrentURIAttribute ().ToUri ().AbsoluteUri; }
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

   

        protected override void Dispose (bool disposing) {
            base.Dispose (disposing);
        }

        #region IWebBrowserWithProxy Members

        public void SetProxy (IPAddress adress, int port, object webBrowser) {
            var control = webBrowser as GeckoWebBrowser;
            if (control != null) {
                var prefs = GeckoPreferences.User;

                prefs["network.proxy.http"] = adress.ToString ();
                prefs["network.proxy.http_port"] = port;

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
        //protected override void WndProc (ref Message m) {

        //    base.WndProc (ref m);

        //    const int WM_GETDLGCODE = 0x87;
        //    const int DLGC_WANTALLKEYS = 0x4;
        //    const int WM_MOUSEACTIVATE = 0x21;
        //    const int MA_ACTIVATE = 0x1;
        //    const int WM_IME_SETCONTEXT = 0x0281;
        //    const int WM_PAINT = 0x000F;
        //    const int WM_SETFOCUS = 0x0007;
        //    const int WM_KILLFOCUS = 0x0008;


        //    const int ISC_SHOWUICOMPOSITIONWINDOW = unchecked ((int) 0x80000000);
        //    if (!DesignMode) {
        //        IntPtr focus;
        //        switch (m.Msg) {
        //            case WM_KILLFOCUS:
        //                base.OnLeave (new EventArgs ());
        //                break;
        //            case WM_SETFOCUS:
        //            case WM_MOUSEACTIVATE:
        //                base.OnGotFocus (new EventArgs ());
        //                break;
        //            case WM_IME_SETCONTEXT:
        //                if (WebBrowserFocus != null) {
        //                    if (m.WParam == IntPtr.Zero)
        //                        base.OnLeave (new EventArgs ());
        //                    else
        //                        base.OnGotFocus (new EventArgs ());
        //                }
        //                break;
        //        }
        //    }
        //}

        #endregion

     
        public bool CanGoBack {
            get { return WebNav.GetCanGoBackAttribute (); }
        }

        public bool CanGoForward {
            get { return WebNav.GetCanGoForwardAttribute ();  }
        }

        public bool IsBusy {
            get;
            set; 
        }

        public EventHandler<DomKeyEventArgs> DomKeyUp {
            get { throw new NotImplementedException (); }
            set { throw new NotImplementedException(); }
        }

        #region copy from winform.geckowebbrowser
        /// <summary>
        /// Cancels any pending navigation and also stops any sound or animation.
        /// </summary>
        public void Stop () {
            if (WebNav != null)
                try {
                    WebNav.Stop ((int) nsIWebNavigationConsts.STOP_ALL);
                } catch (COMException ex) {
                    if (ex.ErrorCode == GeckoError.NS_ERROR_UNEXPECTED)
                        return;
                    throw;
                }
        }

        #endregion

       
    }
}