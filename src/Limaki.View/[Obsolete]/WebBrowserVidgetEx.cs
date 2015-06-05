using System;
using System.ComponentModel;
using Limaki.Common;

namespace Limaki.View.Vidgets {

    [Obsolete]
    public class WebBrowserVidgetEx : WebBrowserVidget, IWebBrowserEx {

        public class WebBrowserExBackendHost : VidgetBackendHost {
            protected override IVidgetBackend OnCreateBackend () {
                this.ToolkitEngine.Backend.CheckInitialized ();
                return Registry.Factory.Create<IWebBrowserExBackend> ();
            }
        }

        protected override VidgetBackendHost CreateBackendHost () {
            return new WebBrowserExBackendHost ();
        }

        public new IWebBrowserExBackend Backend { get { return BackendHost.Backend as IWebBrowserExBackend; } }

        public bool AllowNavigation { get { return Backend.AllowNavigation; } set { Backend.AllowNavigation = value; } }

        public bool AllowWebBrowserDrop { get { return Backend.AllowWebBrowserDrop; } set { Backend.AllowWebBrowserDrop = value; } }
        
        public string DocumentTitle { get { return Backend.DocumentTitle; } }

        public string DocumentType { get { return Backend.DocumentType; } }

        public bool IsBusy { get { return Backend.IsBusy; } }

        public bool IsOffline { get { return Backend.IsOffline; } }

        public string StatusText { get { return Backend.StatusText; } }
        
        public void Navigate (System.Uri url) { Backend.Navigate (url); }

        public void Navigate (string urlString, bool newWindow) { Backend.Navigate (urlString, newWindow); }

        public void Navigate (string urlString, string targetFrameName) { Backend.Navigate (urlString, targetFrameName); }

        public void Navigate (System.Uri url, bool newWindow) { Backend.Navigate (url, newWindow); }

        public void Navigate (System.Uri url, string targetFrameName) { Backend.Navigate (url, targetFrameName); }

        public void Navigate (string urlString, string targetFrameName, byte[] postData, string additionalHeaders) {
            Backend.Navigate (urlString, targetFrameName, postData, additionalHeaders);
        }

        public void Navigate (System.Uri url, string targetFrameName, byte[] postData, string additionalHeaders) {
            Backend.Navigate (url, targetFrameName, postData, additionalHeaders);
        }

 

        public void Refresh () { Backend.Refresh (); }

        public void Stop () { Backend.Stop (); }

        public void GoSearch () { Backend.GoSearch (); }

        public void ShowPageSetupDialog () { Backend.ShowPageSetupDialog (); }

        public void ShowPrintPreviewDialog () { Backend.ShowPrintPreviewDialog (); }

        public void ShowPropertiesDialog () { Backend.ShowPropertiesDialog (); }

        public void ShowSaveAsDialog () { Backend.ShowSaveAsDialog (); }

        public event EventHandler CanGoBackChanged { add { Backend.CanGoBackChanged += value; } remove { Backend.CanGoBackChanged -= value; } }
        public event EventHandler CanGoForwardChanged { add { Backend.CanGoForwardChanged += value; } remove { Backend.CanGoForwardChanged -= value; } }
        public event EventHandler DocumentTitleChanged { add { Backend.DocumentTitleChanged += value; } remove { Backend.DocumentTitleChanged -= value; } }

        public event EventHandler FileDownload { add { Backend.FileDownload += value; } remove { Backend.FileDownload -= value; } }

        public event CancelEventHandler NewWindow { add { Backend.NewWindow += value; } remove { Backend.NewWindow -= value; } }

        public event EventHandler StatusTextChanged { add { Backend.StatusTextChanged += value; } remove { Backend.StatusTextChanged -= value; } }

        public event EventHandler PaddingChanged { add { Backend.PaddingChanged += value; } remove { Backend.PaddingChanged -= value; } }

        public override void Dispose () { }
    }
}