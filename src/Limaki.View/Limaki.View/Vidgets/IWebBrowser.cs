/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 */

using System;
using System.IO;
using System.Net;

namespace Limaki.View.Vidgets {

    public interface IWebBrowser {

        Stream DocumentStream { get; set; }
        string DocumentText { get; set; }
        Uri Url { get; set; }
        void Navigate (string urlString);

        void AfterNavigate (Func<bool> done);
        void MakeReady ();
        
    }

    public interface IWebBrowserBackend : IWebBrowser, IVidgetBackend, IHistoryAware {
        
    }

    public interface IWebBrowserWithProxy {
        void SetProxy(IPAddress adress, int port, object webBrowser);
    }

    public interface IGeckoWebBrowserBackend : IWebBrowserBackend, IWebBrowserWithProxy { }
}