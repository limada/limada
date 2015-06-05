using System;
using System.IO;
using Limaki.View.Vidgets;
using Xwt;
using Xwt.Backends;

namespace Limaki.View.XwtBackend {

    public class WebBrowserVidgetBackend : VidgetBackend<WebBrowserWidget>, IWebBrowserBackend {

        public new Vidgets.WebBrowserVidget Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            Frontend = (Vidgets.WebBrowserVidget) frontend;
        }

        public Stream DocumentStream { get { return Widget.DocumentStream; } set { Widget.DocumentStream = value; } }

        public string DocumentText { get { return Widget.DocumentText; } set { Widget.DocumentText = value; } }

        public string Url { get { return Widget.Url; } set { Widget.Url = value; } }

        public void Navigate (string urlString) { Widget.Navigate (urlString); }

        public void WaitFor (System.Func<bool> done) { Widget.WaitFor (done); }

        public void MakeReady () { Widget.MakeReady (); }

        public void WaitLoaded () { Widget.WaitLoaded(); }

        public bool CanGoBack { get { return Widget.CanGoBack; } }

        public bool CanGoForward { get { return Widget.CanGoForward; } }

        public bool GoBack () { return Widget.GoBack (); }

        public bool GoForward () { return Widget.GoForward (); }

        public void GoHome () { Widget.GoHome (); }

        public void Clear () { Widget.Clear (); }
    }
}