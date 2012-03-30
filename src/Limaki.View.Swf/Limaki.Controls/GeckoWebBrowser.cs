using System;
using System.IO;
using Limaki.Drawing;
using Limaki.Viewers;
using Gecko;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Diagnostics;
using Limaki.View;
using System.Collections.Generic;

namespace Limaki.ThirdPartyWrappers {
    
    public class GeckoWebBrowser:Gecko.GeckoWebBrowser, IWebBrowserWithProxy,
        IWebBrowser, IZoomTarget, INavigateTarget {

        public string XulDir(string basedir) {
            foreach (var dir in new string[]{ @"Plugins\",@"..\3rdParty\bin\"}) {
                var s = dir;
                for (int i = 0; i <= 6; i++) {
                    var xuldir = basedir + s + @"xulrunner11.0";
                    if (Directory.Exists(xuldir))
                        return xuldir;
                    s = @"..\" + s;
                }
            }
            return null;
        }

        public GeckoWebBrowser() {
            string xulDir = XulDir(AppDomain.CurrentDomain.BaseDirectory);
            if (xulDir == null)
                throw new ArgumentException("xulrunner is missing");
            Xpcom.Initialize(xulDir);

            this.DomKeyUp += new EventHandler<GeckoDomKeyEventArgs>(GeckoWebBrowser_DomKeyUp);
        }

        void GeckoWebBrowser_DomKeyUp(object sender, GeckoDomKeyEventArgs e) {
            char keyCode = Convert.ToChar (e.KeyCode);
            if (!e.ShiftKey && ! e.AltKey && e.CtrlKey && keyCode == 'k') {
                ZoomFactor = ZoomFactor*1.1f;
            }
            if (!e.ShiftKey && !e.AltKey && e.CtrlKey && keyCode == 'm') {
                ZoomFactor = ZoomFactor / 1.1f;
            }
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

        public bool AllowNavigation {
            get {
                throw new Exception("The method or operation is not implemented.");
            }
            set {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public bool AllowWebBrowserDrop {
            get {
                throw new Exception("The method or operation is not implemented.");
            }
            set {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public new System.Windows.Forms.HtmlDocument Document {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public System.IO.Stream DocumentStream {
            get {
                throw new Exception("The method or operation is not implemented.");
            }
            set {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public string DocumentText {
            get {return base.Document.TextContent;}
            set {
                SetDocumentTextOverAboutBlank(value);
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

        void SetDocumentTextOverPostData(string content) {
            // does not work:
            byte[] buf = 
                Encoding.Convert(Encoding.Unicode, Encoding.ASCII, Encoding.Unicode.GetBytes(content));
            string header = "Content-Type: text/html\r\nContent-Length:"+buf.Length.ToString();
            Navigate("http://localhost/", 0, null, buf, header);

        }

        // mono-webbrowser-render
        //public void Render(string html, string uri, string contentType) {

        //    nsIWebBrowserStream stream;
        //    if (Navigation != null) {
        //        stream = (nsIWebBrowserStream)navigation.navigation;
        //    } else
        //        throw new Mono.WebBrowser.Exception(Mono.WebBrowser.Exception.ErrorCodes.Navigation);
        //    AsciiString asciiUri = new AsciiString(uri);
        //    nsIURI ret;
        //    IOService.newURI(asciiUri.Handle, null, null, out ret);

        //    AsciiString ctype = new AsciiString(contentType);

        //    HandleRef han = ctype.Handle;

        //    stream.openStream(ret, han);

        //    IntPtr native_html = Marshal.StringToHGlobalAnsi(html);
        //    stream.appendToStream(native_html, (uint)html.Length);
        //    Marshal.FreeHGlobal(native_html);

        //    stream.closeStream();

        //}

        public string DocumentType {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool IsOffline {
            get { throw new Exception("The method or operation is not implemented."); }
        }


        public System.Windows.Forms.WebBrowserReadyState ReadyState {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public new Uri Url {
            get {return base.Url;}
            set { throw new Exception("The method or operation is not implemented."); }
        }

        public void GoHome() {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Navigate(Uri url) {
            base.Navigate (url.AbsoluteUri);
        }

        public void Navigate(string urlString, bool newWindow) {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Navigate(string urlString, string targetFrameName) {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Navigate(Uri url, bool newWindow) {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Navigate(Uri url, string targetFrameName) {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Navigate(string urlString, string targetFrameName, byte[] postData, string additionalHeaders) {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Navigate(Uri url, string targetFrameName, byte[] postData, string additionalHeaders) {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Refresh(System.Windows.Forms.WebBrowserRefreshOption opt) {
            base.Refresh ();
        }

        public void GoSearch() {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ShowPageSetupDialog() {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ShowPrintPreviewDialog() {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ShowPropertiesDialog() {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ShowSaveAsDialog() {
            throw new Exception("The method or operation is not implemented.");
        }

        public new event System.Windows.Forms.WebBrowserDocumentCompletedEventHandler DocumentCompleted;

        public event EventHandler FileDownload;

        public new event System.Windows.Forms.WebBrowserNavigatedEventHandler Navigated;

        public new event System.Windows.Forms.WebBrowserNavigatingEventHandler Navigating;

        public event System.ComponentModel.CancelEventHandler NewWindow;

        public new event System.Windows.Forms.WebBrowserProgressChangedEventHandler ProgressChanged;

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
            var control = webBrowser as GeckoWebBrowser;
            if (control != null) {
                var prefs = GeckoPreferences.User;

                prefs["network.proxy.http"] = adress.ToString();
                prefs["network.proxy.http_port"] = port;

                prefs["network.proxy.no_proxies_on"] = "";
                prefs["network.proxy.type"] = 1;


            }
        }

        #endregion
    }
}
