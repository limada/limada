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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using Limada.Model;
using Limada.Schemata;
using Limaki.Common.Text.HTML;
using Limaki.Model.Streams;
using Limaki.UseCases.Viewers.StreamViewers.WebProxy;
using Limaki.UseCases.Viewers.StreamViewers.WebProxy;
using Id = System.Int64;

namespace Limaki.UseCases.Viewers.StreamViewers {
    public interface IHTMLViewer {
        object CreateControl(object parent);
        bool AcceptsProxy(object webBrowser);
        void SetProxy(IPAddress adress, int port, object webBrowser);

        void AfterNavigate(object webBrowser, bool done);
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
                        var webContent = GetContentFromInfo (info);
                        closeStream = ! webContent.IsStreamOwner;
                        bool done = false;
                        webServer.GetContent = (s) => {
                            WebContent result = null;
                            try {
                                Uri request = new Uri(s);
                                if (webContent.Uri.AbsoluteUri == request.AbsoluteUri) {
                                    if (!webContent.ContentIsEmpty) {
                                        result = webContent;
                                    }
                                } else {
                                    result = this.GetContentFromGraph(request);
                                    if (UseProxy && result == null) {
                                        result = new WebProxyContent();
                                    }
                                }
                            } catch {
                                Trace.WriteLine("request denied:" + s);
                                result = null;
                            } finally {
                                done = true;
                            }
                            return result;
                        };

                        WebBrowser.MakeReady();
                        if (UseProxy) {
                            Viewer.SetProxy (webServer.Addr,webServer.Port, this.Control);
                        }

                        WebBrowser.Navigate(webContent.Uri.AbsoluteUri);
                        
                        Viewer.AfterNavigate (WebBrowser, done);
                        Trace.WriteLine("Navigated to" + webContent.Uri.AbsoluteUri);
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

        IDictionary<Id, string> _mimeTypes = null;
        public virtual IDictionary<Id,string> MimeTypes {
            get {
                if (_mimeTypes==null) {
                    _mimeTypes = new Dictionary<Id, string> ();
                    _mimeTypes.Add (StreamTypes.HTML, "text/html");
                    _mimeTypes.Add(StreamTypes.ASCII, "text/plain");
                    _mimeTypes.Add(StreamTypes.Doc, "application/msword");
                    _mimeTypes.Add(StreamTypes.GIF, "image/gif");
                    _mimeTypes.Add(StreamTypes.JPG, "image/jpeg");
                    _mimeTypes.Add(StreamTypes.PNG, "image/png");
                    _mimeTypes.Add(StreamTypes.RTF, "text/rtf");
                    _mimeTypes.Add(StreamTypes.TIF, "image/tiff");
                }
                return _mimeTypes;
            }
        }
        public virtual string MimeType(Id streamType) {
            string result = null;
            MimeTypes.TryGetValue (streamType, out result);
            return result;
        }

        public virtual WebContent GetContentFromInfo(StreamInfo<Stream> info) {
            var webContent = new WebContent();
            webContent.ClearContentAfterServing = true;
            webContent.ContentIsStream = true;
            webContent.IsStreamOwner = this.IsStreamOwner;
            webContent.ContentStream = info.Data;
            webContent.Uri = new Uri(webServer.Uri,"Id=" + CurrentThingId.ToString("X"));
            webContent.MimeType = MimeType (info.StreamType);
            if (UseProxy) {
                var source = info.Source as string;
                if (source != null && source != "about:blank") {
                    if (Uri.IsWellFormedUriString(source, UriKind.RelativeOrAbsolute)) {
                        Uri uri = null;
                        Uri.TryCreate(source,UriKind.RelativeOrAbsolute,out uri);
                        if (uri != null && !uri.IsUnc && ! uri.IsFile) {
                            webContent.Uri = uri;
                        }
                    }
                }
            }
            return webContent;
        }

        public virtual WebContent GetContentFromThing(IThingGraph graph, IThing thing) {
            var info = ThingStreamFacade.GetStreamInfo(graph, thing);
            return GetContentFromInfo (info);
        }

        CommonSchema schema = new CommonSchema();
        public virtual WebContent GetContentFromGraph(Uri uri) {
            WebContent result = null;
            try {
                var graph = this.ThingGraph as SchemaThingGraph;
                
                var thing = this.ContentThing;
                if (thing != null && graph != null) {
                    var searchGraph = graph.Source;
                    string content = uri.Segments[uri.Segments.Length - 1];
                    foreach (ILink link in searchGraph.Edges(thing)) {
                        var adj = link.Leaf;
                        if (adj != thing && ( adj is IStreamThing )) {
                            var desc = graph.Description(adj);
                            if (desc != null && desc.ToString () == content) {
                                return GetContentFromThing (graph, adj);
                            }
                        }
                    }
                }
                if (graph != null) {
                    string content = uri.AbsoluteUri;
                    foreach (IThing found in graph.GetByData (content, true)) {
                        var target = schema.GetTheRoot (graph, found, CommonSchema.SourceMarker);
                        if (target is IStreamThing) {
                            return GetContentFromThing (graph, target);
                        }
                    }
                }
            } catch (Exception e) {
                Trace.WriteLine (e.Message);
                return null;
            }
            return result;
        }
    }
}