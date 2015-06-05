/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Limaki.Common;
using Limaki.Contents;
using Limaki.View;
using Limaki.View.ContentViewers;
using Limaki.View.Vidgets;
using Limaki.WebServers;

namespace Limaki.View.ContentViewers {

    public abstract class HtmlContentViewerBase : ContentStreamViewer {

        WebBrowserVidget _webBrowser = null;
        protected WebBrowserVidget WebBrowser {
            get {
                if (_webBrowser == null) {
                    _webBrowser = new WebBrowserVidget();
                    UseWebServer = !OS.Mono;
                    UseProxy = WebBrowser.Backend is IWebBrowserWithProxy;
                }
                return _webBrowser;
            }
        }

        public override IVidget Frontend { get { return WebBrowser; } }
        public override IVidgetBackend Backend { get { return WebBrowser.Backend; } }

        public override bool Supports (long streamType) {
            var supporter = WebBrowser.Backend as IContentSpec;
            if (supporter != null)
                return supporter.ContentSpecs.Any(i => i.ContentType == streamType);

            return streamType == ContentTypes.HTML;
        }


        #region WebServer
        protected int _port = -1;
        public int Port {
            get {
                if (_port == -1) {
                    _port = 41100;
                }
                return _port;

            }
            set { _port = value; }
        }

        protected WebServer _webServer = null;
        public virtual WebServer WebServer {
            get {
                if (_webServer == null) {
                    _webServer = new WebServer ();
                    _webServer.Port = this.Port;

                    _webServer.Start();
                }
                return _webServer;
            }
        }

        protected object lockObject = new object ();

        public virtual bool UseWebServer { get; set; }
        public virtual bool UseProxy { get; set; }
        public virtual bool StealthMode { get; set; }

        #endregion

        public abstract IWebResponse CreateResponse ();

        public virtual void SetContent (IWebResponse response, Content<Stream> content) {
            bool closeStream = this.IsStreamOwner;
            lock (lockObject) {
              
                closeStream = !response.IsStreamOwner;

                WebServer.ContentGetter = response.Getter (content);

                WebBrowser.MakeReady ();
                if (UseProxy) {
                    var browserWithProxy = this.Backend as IWebBrowserWithProxy;
                    if (browserWithProxy != null) {
                        browserWithProxy.SetProxy(WebServer.Addr, WebServer.Port, this.Backend);
                    }
                    
                }

                WebBrowser.Navigate (response.AbsoluteUri);
                WebBrowser.WaitFor(() => 
                    response.Done
                );

                Trace.WriteLine ("Navigated to " + response.AbsoluteUri);
            }
        }

        public override void SetContent(Content<Stream> content) {
            bool closeStream = this.IsStreamOwner;
            try {

                if (UseWebServer || UseProxy) {
                    var response = CreateResponse ();
                    SetContent (response, content);
                } else {
                    WebBrowser.MakeReady();
                    WebBrowser.DocumentStream = content.Data;
                }

            } catch (Exception e) {
                Trace.WriteLine ("content load failed:\t" + e.Message);  
            } finally {
                if (IsStreamOwner) {
                    content.Data = null;
                    var source = content.Source as Stream;
                    if (closeStream && source != null) {
                        source.Close ();
                    }
                    content.Source = null;
                } 
            }
        }

        public override void Save(Content<Stream> content) { }
        public override bool CanSave() {return false;}

        ~HtmlContentViewerBase () {
            Dispose(false);
        }

        protected virtual void Dispose (bool disposing) {
            if (disposing) {
                if (_webBrowser is IDisposable) {
                    ((IDisposable) _webBrowser).Dispose();
                }
            }
            _webBrowser = null;
            if (_webServer != null) {
                _webServer.Dispose ();
                _webServer = null;
            }
        }

        public override void Dispose () {
            Dispose (true);
        }

        public override void Clear() {
            base.Clear();
            if (_webServer != null) {
                
            }
            if (_webBrowser != null) {
                WebBrowser.Clear();
            }
        }

        
    }
}