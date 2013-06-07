using Limaki.View;
using System;
using System.IO;
using Xwt.Backends;

namespace Limaki.Viewers.Vidgets {

    [BackendType(typeof(IWebBrowserBackend))]
    public class WebBrowserVidget : Vidget, IWebBrowser {

        IWebBrowserBackend Backend { get { return BackendHost.Backend as IWebBrowserBackend; } }

        public bool AllowNavigation { get { return Backend.AllowNavigation; } set { Backend.AllowNavigation = value; } }

        public bool AllowWebBrowserDrop { get { return Backend.AllowWebBrowserDrop; } set { Backend.AllowWebBrowserDrop = value; } }

        public bool CanGoBack { get { return Backend.CanGoBack; } }

        public bool CanGoForward { get { return Backend.CanGoForward; } }

        public Stream DocumentStream { get { return Backend.DocumentStream; } set { Backend.DocumentStream = value; } }

        public string DocumentText { get { return Backend.DocumentText; } set { Backend.DocumentText = value; } }

        public string DocumentTitle { get { return Backend.DocumentTitle; } }

        public string DocumentType { get { return Backend.DocumentType; } }

        public bool IsBusy { get { return Backend.IsBusy; } }

        public bool IsOffline { get { return Backend.IsOffline; } }

        public string StatusText { get { return Backend.StatusText; } }

        public Uri Url { get { return Backend.Url; } set { Backend.Url = value; } }

        public bool GoBack () { return Backend.GoBack(); }

        public bool GoForward () { return Backend.GoForward(); }

        public void GoHome () { Backend.GoHome(); }

        public void MakeReady () { Backend.MakeReady(); }

        public void Navigate (string urlString) { Backend.Navigate(urlString); }

        public void Navigate (System.Uri url) { Backend.Navigate(url); }

        public void Navigate (string urlString, bool newWindow) { Backend.Navigate(urlString,newWindow); }

        public void Navigate (string urlString, string targetFrameName) { Backend.Navigate(urlString,targetFrameName); }

        public void Navigate (System.Uri url, bool newWindow) { Backend.Navigate(url,newWindow); }

        public void Navigate (System.Uri url, string targetFrameName) { Backend.Navigate(url,targetFrameName); }

        public void Navigate (string urlString, string targetFrameName, byte[] postData, string additionalHeaders) {
            Backend.Navigate(urlString,targetFrameName,postData,additionalHeaders);
        }

        public void Navigate (System.Uri url, string targetFrameName, byte[] postData, string additionalHeaders) {
            Backend.Navigate(url, targetFrameName, postData, additionalHeaders);
        }

        public void Refresh () { Backend.Refresh(); }

        public void Stop () { Backend.Stop(); }

        public void GoSearch () { Backend.GoSearch(); }

        public void ShowPageSetupDialog () { Backend.ShowPageSetupDialog(); }

        public void ShowPrintPreviewDialog () { Backend.ShowPrintPreviewDialog(); }

        public void ShowPropertiesDialog () { Backend.ShowPropertiesDialog(); }

        public void ShowSaveAsDialog () { Backend.ShowSaveAsDialog(); }

        public event System.EventHandler CanGoBackChanged { add { Backend.CanGoBackChanged += value; } remove { Backend.CanGoBackChanged -= value; } }

        public event System.EventHandler CanGoForwardChanged { add { Backend.CanGoForwardChanged += value; } remove { Backend.CanGoForwardChanged -= value; } }

        public event System.EventHandler DocumentTitleChanged { add { Backend.DocumentTitleChanged += value; } remove { Backend.DocumentTitleChanged -= value; } }

        public event System.EventHandler FileDownload { add { Backend.FileDownload += value; } remove { Backend.FileDownload -= value; } }

        public event System.ComponentModel.CancelEventHandler NewWindow { add { Backend.NewWindow += value; } remove { Backend.NewWindow -= value; } }

        public event System.EventHandler StatusTextChanged { add { Backend.StatusTextChanged += value; } remove { Backend.StatusTextChanged -= value; } }

        public event System.EventHandler PaddingChanged { add { Backend.PaddingChanged += value; } remove { Backend.PaddingChanged -= value; } }

        public override void Dispose () {}
    }
}