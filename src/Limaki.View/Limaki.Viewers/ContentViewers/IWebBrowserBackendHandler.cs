using System;
using System.Net;
using Limaki.View;

namespace Limaki.Viewers.StreamViewers {

    public interface IWebBrowserBackendHandler {

        IWebBrowserBackend CreateBackend (object parent);
        bool AcceptsProxy(object webBrowser);
        void SetProxy(IPAddress adress, int port, object webBrowser);
        void AfterNavigate(object webBrowser, Func<bool> done);
        
    }
}