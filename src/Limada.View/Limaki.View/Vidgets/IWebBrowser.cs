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
        string Url { get; set; }
        void Navigate (string urlString);

        void WaitFor (Func<bool> done);
        void WaitLoaded ();
        void MakeReady ();

        void Clear ();
    }

    public interface IWebBrowserBackend : IWebBrowser, IVidgetBackend, IHistoryAware {
       
    }

    public interface IWebBrowserWithProxy {
        void SetProxy(IPAddress adress, int port, object webBrowser);
    }

    public interface IGeckoWebBrowserBackend : IWebBrowserBackend, IWebBrowserWithProxy { }
}