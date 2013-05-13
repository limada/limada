using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Limaki.Viewers;
using Limaki.Viewers.StreamViewers;
using Limaki.Common;

namespace Limaki.Swf.Backends.Viewers.Content {

    public class WebBrowserBackendHandler:IWebBrowserBackendHandler {

        public static bool GeckoFailed = false ;
        public object CreateBackend(object parent) {
            Control _control = null;
            if (GeckoFailed || OS.Mono || OS.IsWin64Process) { //(true){ //
                _control = new Backends.WebBrowser();
                _control.Parent = parent as Control;
                GeckoFailed = true;
                Trace.WriteLine("No Gecko");
            } else {
                try {
                    var gecko = Registry.Factory.Create<IGeckoWebBrowser>();
                    if (gecko !=null)
                        _control = (Control)gecko;
                    else
                        throw new Exception();
                } catch {
                    GeckoFailed = true;
                    return CreateBackend(parent);
                }
                _control.Parent = parent as Control;
                Thread.Sleep(0);
            }
            return _control;
        }

        public bool AcceptsProxy(object control) {
            return control is IWebBrowserWithProxy;
        }

        public void SetProxy(IPAddress adress, int port, object webBrowser) {
            var browser = webBrowser as IWebBrowserWithProxy;
            if(browser != null) {
                browser.SetProxy(adress, port, webBrowser);
            }
        }

        public void AfterNavigate(object webBrowser, Func<bool> done) {
            var control = webBrowser as Control;
            if (!OS.Mono) {
                // try to resolve timing problems 
                // does not work so well, but better than nothing
                int i = 0;
                while (!done() && i < 10) {
                    Thread.Sleep (5);
                    i++;
                }
            }
            // fails with IExplorer, not necessry with gecko:
            // control.Refresh();
            Application.DoEvents(); 
        }

    }
}