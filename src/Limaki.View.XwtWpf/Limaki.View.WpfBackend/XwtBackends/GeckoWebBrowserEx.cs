using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using Gecko;
using Limaki.Common.Text.HTML;
using Limaki.Contents;
using Limaki.View.Vidgets;
using System.Diagnostics;

namespace Limaki.View.WpfBackend {

    public class GeckoWebBrowserEx : Gecko.GeckoWebBrowser {

        public bool IsBusy {
            get;
            set;
        }

        public GeckoWebBrowserEx () {
            new XulRunner ().Initialize ();
            Gecko.GeckoPreferences.Default["extensions.blocklist.enabled"] = false;
            //Gecko.GeckoPreferences.Default["pdfjs.disabled"] = false;
            //TODO: this.DomKeyUp += new EventHandler<DomKeyEventArgs> (GeckoWebBrowser_DomKeyUp);
            //base.DocumentCompleted += (s, e) => 
            //    IsBusy = false;
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

        #region IWebBrowser Member

        public void WaitFor (Func<bool> done) {
            //int i = 0;
            //while (!done () && i < 10) {
            //    Thread.Sleep (5);
            //    i++;
            //}
            WpfExtensions.DoEvents ();
        }

        public void WaitLoaded () {
            // blocks everything:
            //BlockUntilNavigationFinishedDone = true;
            //BlockUntilNavigationFinished ();
        }

        public void MakeReady () {
            BlockUntilNavigationFinishedDone = true;
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
                var html = value;
                if (value.StartsWith ("<html>"))
                    html = HtmlHelper.HtmUtf8Begin + value.Substring (6);
                InternalLoadContent (html, "", "text/html");
                BlockUntilNavigationFinishedDone = true;
                BlockUntilNavigationFinished ();
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
                WpfExtensions.DoEvents ();
                //Application.RaiseIdle (new EventArgs ());
            }
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
            Navigate (string.Concat ("data:", type, ";base64,", Convert.ToBase64String (bytes)), GeckoLoadFlags.FromExternal);
        }

        protected void InternalLoadContent (string content, string url, string contentType) {
            using (var sContentType = new nsACString (contentType))
            using (var sUtf8 = new nsACString ("UTF8")) {
                ByteArrayInputStream inputStream = null;
                try {
                    inputStream = ByteArrayInputStream.Create (System.Text.Encoding.UTF8.GetBytes (content != null ? content : string.Empty));

                    var docShell = Xpcom.QueryInterface<nsIDocShell> (this.WebBrowser);
                    nsIURI uri = null;
                    if (!string.IsNullOrEmpty (url))
                        uri = IOService.CreateNsIUri (url);
                    nsIDocShellLoadInfo l = null;
                    if (true) {
                        l = Xpcom.QueryInterface<nsIDocShellLoadInfo> (this.WebBrowser);

                        docShell.CreateLoadInfo (ref l);
                        
                        l.SetLoadTypeAttribute (new IntPtr(16));
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
     
        public bool CanGoBack {
            get { return WebNav.GetCanGoBackAttribute (); }
        }

        public bool CanGoForward {
            get { return WebNav.GetCanGoForwardAttribute ();  }
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
            BlockUntilNavigationFinishedDone = true;
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

        #region Workarounds

        protected override IntPtr WndProc (IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
            
            const int WM_GETDLGCODE = 0x87;
            const int DLGC_WANTALLKEYS = 0x4;
            const int WM_MOUSEACTIVATE = 0x21;
            const int MA_ACTIVATE = 0x1;
            const int WM_IME_SETCONTEXT = 0x0281;
            const int WM_PAINT = 0x000F;
            const int WM_SETFOCUS = 0x0007;
            const int WM_KILLFOCUS = 0x0008;

            System.Windows.RoutedEventArgs ev = null;
            switch (msg) {
                case WM_SETFOCUS:
                case WM_MOUSEACTIVATE:
                    ev = new System.Windows.RoutedEventArgs (System.Windows.FrameworkElement.GotFocusEvent, this);
                    base.OnGotFocus (ev);
                    break;
                case WM_KILLFOCUS:
                    ev = new System.Windows.RoutedEventArgs (System.Windows.FrameworkElement.LostFocusEvent, this);
                    base.OnLostFocus (ev);
                    break;
                case WM_IME_SETCONTEXT:
                    if (WebBrowserFocus != null) {
                        if (wParam == IntPtr.Zero) {
                            ev = new System.Windows.RoutedEventArgs (System.Windows.FrameworkElement.LostFocusEvent, this);
                            base.OnLostFocus (ev);
                        } else {
                            ev = new System.Windows.RoutedEventArgs (System.Windows.FrameworkElement.GotFocusEvent, this);
                            base.OnGotFocus (ev);
                        }
                    }
                    break;
            }

            if (ev != null)
                handled = ev.Handled;

            //Trace.WriteLine (msg.ToString ("X"));
            return base.WndProc (hwnd, msg, wParam, lParam, ref handled);
        }

        #endregion
    }
}