using System.Net;
using System.Threading;
using System.Windows.Forms;
using Limaki.UseCases.Viewers.StreamViewers;
using System;

namespace Limaki.UseCases.Winform.Viewers.StreamViewers {
    public class HTMLViewer:IHTMLViewer {
        public static bool GeckoFailed = true;
        public object CreateControl(object parent) {
            Control _control = null;
            if (GeckoFailed || Commons.Mono || Commons.IsWin64BitOS()) { //(true){ //
                _control = new Limaki.Winform.Controls.WebBrowser();
                _control.Parent = parent as Control;
                System.Console.WriteLine("No Gecko");
            } else {
                try {
                    _control = new Limaki.ThirdPartyWrappers.GeckoWebBrowser();
                } catch {
                    GeckoFailed = true;
                    return CreateControl(parent);
                }
                _control.Parent = parent as Control;
                Thread.Sleep(0);
            }
            return _control;
        }

        public bool AcceptsProxy(object control) {
            if (!GeckoFailed) {
                try {
                    return control is Limaki.ThirdPartyWrappers.GeckoWebBrowser;
                }
                catch {
                    GeckoFailed = true;
                }
            }
            return false;
        }

        public void SetProxy(IPAddress adress, int port, object webBrowser) {
            if (!GeckoFailed) {
                try {
                    var control = webBrowser as Limaki.ThirdPartyWrappers.GeckoWebBrowser;
                    if (control != null) {
                        var prefs = Skybound.Gecko.GeckoPreferences.User;

                        prefs["network.proxy.http"] = adress.ToString();
                        prefs["network.proxy.http_port"] = port;

                        prefs["network.proxy.no_proxies_on"] = "";
                        prefs["network.proxy.type"] = 1;


                    }
                } catch {
                    GeckoFailed = true;
                }
            }
        }

        public void AfterNavigate(object webBrowser, bool done) {
            var control = webBrowser as Control;
            if (!Commons.Mono) {
                // try to resolve timing problems 
                // does not work so well, but better than nothing
                int i = 0;
                while (!done && i < 10) {
                    Thread.Sleep (5);
                    i++;
                }
            }
            control.Refresh();
            Application.DoEvents(); 
        }
    }
}