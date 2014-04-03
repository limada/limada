using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Limaki.View.Vidgets;

namespace Limaki.View.WpfBackend {

    public class WebBrowserBackend : WrapPanel, IWebBrowserBackend {
        // WebBrowser is sealed, so we have to wrap it
        protected WebBrowser WebBrowser { get; set; }

        # region IWebBrowserBackend-Implementation
        
        public bool AllowNavigation {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public bool AllowWebBrowserDrop {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public bool CanGoBack {
            get { throw new NotImplementedException(); }
        }

        public bool CanGoForward {
            get { throw new NotImplementedException(); }
        }

        public System.IO.Stream DocumentStream {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public string DocumentText {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public string DocumentTitle {
            get { throw new NotImplementedException(); }
        }

        public string DocumentType {
            get { throw new NotImplementedException(); }
        }

        public bool IsBusy {
            get { throw new NotImplementedException(); }
        }

        public bool IsOffline {
            get { throw new NotImplementedException(); }
        }

        public string StatusText {
            get { throw new NotImplementedException(); }
        }

        public Uri Url {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public bool GoBack () {
            throw new NotImplementedException();
        }

        public bool GoForward () {
            throw new NotImplementedException();
        }

        public void GoHome () {
            throw new NotImplementedException();
        }

        public void MakeReady () {
            throw new NotImplementedException();
        }

        public void Navigate (string urlString) {
            throw new NotImplementedException();
        }

        public void Navigate (Uri url) {
            throw new NotImplementedException();
        }

        public void Navigate (string urlString, bool newWindow) {
            throw new NotImplementedException();
        }

        public void Navigate (string urlString, string targetFrameName) {
            throw new NotImplementedException();
        }

        public void Navigate (Uri url, bool newWindow) {
            throw new NotImplementedException();
        }

        public void Navigate (Uri url, string targetFrameName) {
            throw new NotImplementedException();
        }

        public void Navigate (string urlString, string targetFrameName, byte[] postData, string additionalHeaders) {
            throw new NotImplementedException();
        }

        public void Navigate (Uri url, string targetFrameName, byte[] postData, string additionalHeaders) {
            throw new NotImplementedException();
        }

        public void Refresh () {
            throw new NotImplementedException();
        }

        public void AfterNavigate (Func<bool> done) {
            throw new NotImplementedException();
        }

        public void Stop () {
            throw new NotImplementedException();
        }

        public void GoSearch () {
            throw new NotImplementedException();
        }

        public void ShowPageSetupDialog () {
            throw new NotImplementedException();
        }

        public void ShowPrintPreviewDialog () {
            throw new NotImplementedException();
        }

        public void ShowPropertiesDialog () {
            throw new NotImplementedException();
        }

        public void ShowSaveAsDialog () {
            throw new NotImplementedException();
        }

        public event EventHandler CanGoBackChanged;

        public event EventHandler CanGoForwardChanged;

        public event EventHandler DocumentTitleChanged;

        public event EventHandler FileDownload;

        public event System.ComponentModel.CancelEventHandler NewWindow;

        public event EventHandler StatusTextChanged;

        public event EventHandler PaddingChanged;

        #endregion

        #region IVidgetBackend-Implementation

        public WebBrowserVidget Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (WebBrowserVidget) frontend;
        }

        public Xwt.Size Size {
            get { throw new NotImplementedException();}
        }

        public void Update () {
            throw new NotImplementedException();
        }

        public void Invalidate () {
            throw new NotImplementedException();
        }

        public void Invalidate (Xwt.Rectangle rect) {
            throw new NotImplementedException();
        }

        public void Dispose () {
            throw new NotImplementedException();
        }

        public void SetFocus () {
            throw new NotImplementedException ();
        }

        #endregion
    }
}
