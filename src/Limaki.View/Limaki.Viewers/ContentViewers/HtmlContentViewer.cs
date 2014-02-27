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
using Limada.Model;
using Limaki.Common.Text.HTML;
using Limaki.Contents;
using Limaki.Model.Content;
using Limaki.Net.WebProxyServer;
using Limaki.View;
using Limaki.Viewers.Vidgets;

namespace Limaki.Viewers.StreamViewers {

    public class HtmlContentViewer : ContentStreamViewer {

        WebBrowserVidget _webBrowser = null;
        protected WebBrowserVidget WebBrowser {
            get {
                if (_webBrowser == null) {
                    _webBrowser = new WebBrowserVidget();
                    OnAttachBackend(WebBrowser.Backend);
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
        private int _port = -1;
        public int Port {
            get {
                if (_port == -1) {
                    _port = 41100;
                }
                return _port;

            }
            set { _port = value; }
        }

        WebServer _webServer = null;
        public WebServer WebServer {
            get {
                if (_webServer == null) {
                    _webServer = new WebServer();
                    _webServer.Port = this.Port;

                    _webServer.Start();
                }
                return _webServer;
            }
        }

        private object lockObject = new object();

        public bool UseWebServer { get; set; }
        public bool UseProxy { get; set; }
        #endregion

        public virtual IThingGraph ThingGraph { get; set; }
        public virtual IThing ContentThing { get; set; }

        public virtual  void SetContent (IWebResponse response, Content<Stream> content) {
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
                WebBrowser.AfterNavigate(() => response.Done);

                Trace.WriteLine ("Navigated to " + response.AbsoluteUri);
            }
        }

        public override void SetContent(Content<Stream> content) {
            bool closeStream = this.IsStreamOwner;
            try {

                if (UseWebServer || UseProxy) {
                     var response = new ThingWebResponse {
                        IsStreamOwner = this.IsStreamOwner,
                        Thing = this.ContentThing,
                        ThingGraph = this.ThingGraph,
                        UseProxy = this.UseProxy,
                        BaseUri = this.WebServer.Uri,
                    };
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

        public override void Dispose () {
            if (_webBrowser is IDisposable) {
                ((IDisposable)_webBrowser).Dispose();
            }
            _webBrowser = null;
            if (_webServer != null) {
                _webServer.Dispose ();
                _webServer = null;
            }
        }
        public override void Clear() {
            base.Clear();
            if (_webServer != null) {
                
            }
            if (_webBrowser != null) {
                WebBrowser.Navigate ("about:blank");
            }
        }

        
    }
}