using System.Net;
using System.Threading;
using System.Windows.Forms;
using Limaki.UseCases.Viewers.StreamViewers;

namespace Limaki.UseCases.Winform.Viewers.StreamViewers {
    public class HTMLViewer:IHTMLViewer {
        
        public object CreateControl(object parent) {
            Control _control = null;
            if (Commons.Mono) { //(true){ //
                _control = new Limaki.Winform.Controls.WebBrowser();
                _control.Parent = parent as Control;
            } else {
                _control = new Limaki.ThirdPartyWrappers.GeckoWebBrowser();
                _control.Parent = parent as Control;
                Thread.Sleep(0);
            }
            return _control;
        }

        public bool AcceptsProxy(object control) {
            return control is Limaki.ThirdPartyWrappers.GeckoWebBrowser;
        }

        public void SetProxy(IPAddress adress, int port, object webBrowser) {
            var control = webBrowser as Limaki.ThirdPartyWrappers.GeckoWebBrowser;
            if (control != null) {
                var prefs = Skybound.Gecko.GeckoPreferences.User;

                prefs["network.proxy.http"] = adress.ToString();
                prefs["network.proxy.http_port"] = port;

                prefs["network.proxy.no_proxies_on"] = "";
                prefs["network.proxy.type"] = 1;


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