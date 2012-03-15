using System;
using System.Net;

namespace Limaki.Viewers.StreamViewers {

    public interface IWebBrowserBackendHandler {

        object CreateControl(object parent);
        bool AcceptsProxy(object webBrowser);
        void SetProxy(IPAddress adress, int port, object webBrowser);

        void AfterNavigate(object webBrowser, Func<bool> done);
        
    }
}