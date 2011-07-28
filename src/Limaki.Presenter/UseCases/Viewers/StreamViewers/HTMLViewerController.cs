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
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using Limada.Model;
using Limaki.Common.Text.HTML;
using Limaki.Model.Streams;
using Limaki.UseCases.Viewers.StreamViewers.WebProxy;


namespace Limaki.UseCases.Viewers.StreamViewers {
    public interface IHTMLViewer {
        object CreateControl(object parent);
        bool AcceptsProxy(object webBrowser);
        void SetProxy(IPAddress adress, int port, object webBrowser);

        void AfterNavigate(object webBrowser, Func<bool> done);
        
    }

    public class HTMLViewerController : StreamViewerController {
        public IHTMLViewer Viewer { get; set; }
        
        object _control = null;
        public override object Control {
            get {
                if (_control == null) {
                    _control = Viewer.CreateControl(this.Parent);
                    OnAttach(_control);
                    UseWebServer = !OS.Mono;
                    UseProxy = Viewer.AcceptsProxy(_control);
                }

                return _control;
            }
        }

        protected IWebBrowser WebBrowser {
            get { return Control as IWebBrowser; }
        }

        public override bool Supports(long streamType) {
            return streamType == StreamTypes.HTML;
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
        WebServer webServer {
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

        public override void SetContent(StreamInfo<Stream> info) {
            bool closeStream = this.IsStreamOwner;
            try {

                if (UseWebServer || UseProxy) {

                    lock (lockObject) {
                        var response = new ThingWebResponse {
                            IsStreamOwner = this.IsStreamOwner,
                            Thing = this.ContentThing,
                            ThingGraph = this.ThingGraph,
                            UseProxy = this.UseProxy,
                            BaseUri = this.webServer.Uri,
                        };
                        closeStream = ! response.IsStreamOwner;

                        //  webServer.AddContent(webContent.Uri.AbsoluteUri,
                        webServer.ContentGetter = response.Getter(info);

                        WebBrowser.MakeReady();
                        if (UseProxy) {
                            Viewer.SetProxy(webServer.Addr, webServer.Port, this.Control);
                        }

                        WebBrowser.Navigate(response.WebContentOfThing.Uri.AbsoluteUri);

                        Viewer.AfterNavigate(WebBrowser, ()=>response.Done);
                        Trace.WriteLine("Navigated to" + response.WebContentOfThing.Uri.AbsoluteUri);
                    }
                } else {
                    WebBrowser.MakeReady();
                    if (OS.Mono) {
                        WebBrowser.DocumentStream = info.Data;
                    } else
                        using (StreamReader reader = new StreamReader(info.Data)) {
                            string text = reader.ReadToEnd();
                            WebBrowser.DocumentText = text;

                            // this is testing coulde; should be removed!
                            if (text != null)
                                foreach (var s in HTMLHelper.Links(text)) {
                                    //System.Console.WriteLine (s);
                                }
                        }
                }

            } catch (Exception e) {
                Trace.WriteLine ("content load failed:\t" + e.Message);  
            } finally {
                if (IsStreamOwner) {
                    info.Data = null;
                    var source = info.Source as Stream;
                    if (closeStream && source != null) {
                        source.Close ();
                    }
                    info.Source = null;
                } 
            }
        }

        public override void Save(StreamInfo<Stream> info) { }
        public override bool CanSave() {return false;}

        public override void Dispose() {
            if (_control is IDisposable) {
                ((IDisposable)_control).Dispose ();
            }
            _control = null;
            if (_webServer != null) {
                _webServer.Dispose ();
                _webServer = null;
            }
        }
        public override void Clear() {
            base.Clear();
            if (_webServer != null) {
                
            }
            if (_control != null) {
                WebBrowser.Navigate ("about:blank");
            }
        }

        
    }
}