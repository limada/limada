using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using Gecko;
using Limaki.Contents;
using Limaki.View.Vidgets;
using System.Diagnostics;

namespace Limaki.View.WpfBackend {

    public class GeckoWebBrowserEx : Gecko.GeckoWebBrowser {

        public GeckoWebBrowserEx () {
            new XulRunner ().Initialize ();
            Gecko.GeckoPreferences.Default["extensions.blocklist.enabled"] = false;
            //Gecko.GeckoPreferences.Default["pdfjs.disabled"] = false;
            //TODO: this.DomKeyUp += new EventHandler<DomKeyEventArgs> (GeckoWebBrowser_DomKeyUp);
            base.DocumentCompleted += (s, e) => IsBusy = false;
            this.Focusable = true;
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
            LoadHtml (content);
        }

        /// <summary>
        /// Loads supplied html string.
        /// Note: LoadHtml isn't intended to load complex Html Documents.		
        /// In order to find out when LoadHtml has finished attach a handler to DocumentCompleted Event.
        /// </summary>
        /// <param name="htmlDocument"></param>
        public void LoadHtml (string htmlDocument) {
            LoadBase64EncodedData ("text/html", htmlDocument);
        }

        /// <summary>
        /// Load supplied string.encoded as base64.
        /// </summary>
        /// <param name="type">the type of the data eg. text/html </param>
        /// <param name="data">string that will be encoded as base64 </param>
        public void LoadBase64EncodedData (string type, string data) {
            var bytes = System.Text.Encoding.UTF8.GetBytes (data);
            Navigate (string.Concat ("data:", type, ";base64,", Convert.ToBase64String (bytes)));
        }

        private void InternalLoadContent (string content, string url, string contentType) {
            using (var sContentType = new nsACString (contentType))
            using (var sUtf8 = new nsACString ("UTF8")) {
                ByteArrayInputStream inputStream = null;
                try {
                    inputStream = ByteArrayInputStream.Create (System.Text.Encoding.UTF8.GetBytes (content != null ? content : string.Empty));

                    nsIDocShell docShell = Xpcom.QueryInterface<nsIDocShell> (this.WebBrowser);
                    docShell.LoadStream (inputStream, IOService.CreateNsIUri (url), sContentType, sUtf8, null);
                    Marshal.ReleaseComObject (docShell);
                } finally {
                    if (inputStream != null)
                        inputStream.Close ();
                }
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
                value.Position = 0;
                var reader = new StreamReader (value);
                string text = reader.ReadToEnd ();
                this.DocumentText = text;
                value.Position = 0;
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

        #region experimental

        protected override IntPtr WndProc (IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
            switch (msg) {
               case NativeMethods.WM_MOUSEDOWN:
                    break;
            }
            // Trace.WriteLine ("Gecko.WndProc\t"+msg.ToString("X"));
            return base.WndProc (hwnd, msg, wParam, lParam, ref handled);
        }

        #endregion
    }
}