using System;
using Limaki.Viewers;

namespace Limaki.View.XwtBackend {
    public class WebBrowserBackend : DummyBackend, IWebBrowserBackend {
        public bool AllowNavigation { get;set; }

        public bool AllowWebBrowserDrop { get; set; }

        public bool CanGoBack { get { return false; } }

        public bool CanGoForward { get { return false; } }

        public System.IO.Stream DocumentStream { get; set; }
        public string DocumentText { get; set; }

        public string DocumentTitle { get { return ""; } }

        public string DocumentType { get { return ""; } }

        public bool IsBusy { get { return false; } }

        public bool IsOffline { get { return true; } }

        public string StatusText { get { return ""; } }

        public Uri Url { get; set; }

        public bool GoBack () {
            return false;
        }

        public bool GoForward () {
            return false;
        }

        public void GoHome () {
           
        }

        public void MakeReady () {
           
        }

        public void Navigate (string urlString) {
            
        }

        public void Navigate (Uri url) {
            throw new NotImplementedException();
        }

        public void Navigate (string urlString, bool newWindow) {
            
        }

        public void Navigate (string urlString, string targetFrameName) {
          
        }

        public void Navigate (Uri url, bool newWindow) {
          
        }

        public void Navigate (Uri url, string targetFrameName) {
        
        }

        public void Navigate (string urlString, string targetFrameName, byte[] postData, string additionalHeaders) {
            throw new NotImplementedException();
        }

        public void Navigate (Uri url, string targetFrameName, byte[] postData, string additionalHeaders) {
           
        }

        public void Refresh () {
           
        }

        public void AfterNavigate (Func<bool> done) {
           
        }

        public void Stop () {
            
        }

        public void GoSearch () {
            
        }

        public void ShowPageSetupDialog () {
           
        }

        public void ShowPrintPreviewDialog () {
           
        }

        public void ShowPropertiesDialog () {
            
        }

        public void ShowSaveAsDialog () {
       
        }

        public event EventHandler CanGoBackChanged;

        public event EventHandler CanGoForwardChanged;

        public event EventHandler DocumentTitleChanged;

        public event EventHandler FileDownload;

        public event System.ComponentModel.CancelEventHandler NewWindow;

        public event EventHandler StatusTextChanged;

        public event EventHandler PaddingChanged;
    }
}