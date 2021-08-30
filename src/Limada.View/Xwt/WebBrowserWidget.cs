using System.IO;
using Limaki.View.Vidgets;
using Xwt.Backends;

namespace Xwt {

    public interface IWebBrowserWidgetBackend : IWidgetBackend, IWebBrowser, IHistoryAware {}

    [BackendType (typeof (IWebBrowserWidgetBackend))]
    public class WebBrowserWidget : Xwt.Widget, IWebBrowser, IHistoryAware {

        protected new IWebBrowserWidgetBackend Backend {get { return base.Backend as IWebBrowserWidgetBackend; }}

        public Stream DocumentStream { get { return Backend.DocumentStream; } set { Backend.DocumentStream = value; } }

        public string DocumentText { get { return Backend.DocumentText; } set { Backend.DocumentText = value; } }

        public string Url { get { return Backend.Url; } set { Backend.Url = value; } }

        public void Navigate (string urlString) { Backend.Navigate (urlString); }

        public void WaitFor (System.Func<bool> done) { Backend.WaitFor (done); }

        public void WaitLoaded () { Backend.WaitLoaded (); }

        public void MakeReady () { Backend.MakeReady (); }

        public bool CanGoBack { get { return Backend.CanGoBack; } }

        public bool CanGoForward { get { return Backend.CanGoForward; } }

        public bool GoBack () { return Backend.GoBack (); }

        public bool GoForward () { return Backend.GoForward (); }

        public void GoHome () { Backend.GoHome (); }

        public void Clear () { Backend.Clear (); }
    }
}