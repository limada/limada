using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Limada.Model;
using Limada.Schemata;
using Limaki.Model.Streams;
using Limaki.Net.WebProxyServer;

namespace Limaki.Viewers.StreamViewers {
    public class ThingWebResponse {

        public virtual IThingGraph ThingGraph { get; set; }
        public virtual IThing Thing { get; set; }
        public bool IsStreamOwner { get; set; }
        public bool UseProxy { get; set; }
        public bool Done { get; set; }
        public Uri BaseUri { get; set; }

        public WebContent WebContentOfThing { get; protected set; }

        public virtual Func<string, WebContent> Getter(Content<Stream> info) {
            var graph = this.ThingGraph;
            var thing = this.Thing;

            WebContentOfThing = GetContentFromInfo(info, GetUri(thing));
            IsStreamOwner = WebContentOfThing.IsStreamOwner;

            Done = false;

            Func<string, WebContent> getter =
                (s) => {
                    WebContent result = null;
                    try {
                        var request = new Uri(s);
                        if (WebContentOfThing.Uri.AbsoluteUri == request.AbsoluteUri) {
                            if (!WebContentOfThing.ContentIsEmpty) {
                                result = WebContentOfThing;
                            }
                        } else {
                            result = this.GetContentFromGraph(graph, thing, request);
                            if (UseProxy && result == null) {
                                result = new WebProxyContent();
                            }
                        }
                    } catch (Exception ex) {
                        Trace.WriteLine("request denied:" + s);
                        Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
                        result = null;
                    } finally {
                        Done = true;
                    }
                    return result;
                };
            //);


            return getter;
        }

        protected virtual Uri GetUri(IThing thing) {
            return new Uri(BaseUri, "Id=" + Thing.Id.ToString("X"));
        }

        public virtual WebContent GetContentFromInfo(Content<Stream> info, Uri uri) {
            var webContent = new WebContent();
            webContent.ClearContentAfterServing = true;
            webContent.ContentIsStream = true;
            webContent.IsStreamOwner = this.IsStreamOwner;
            webContent.ContentStream = info.Data;
            webContent.Uri = uri;

            webContent.MimeType = MimeType(info.StreamType);
            if (UseProxy) {
                var source = info.Source as string;
                if (source != null && source != "about:blank") {
                    if (Uri.IsWellFormedUriString(source, UriKind.RelativeOrAbsolute)) {
                        uri = null;
                        Uri.TryCreate(source, UriKind.RelativeOrAbsolute, out uri);
                        if (uri != null && !uri.IsUnc && !uri.IsFile) {
                            webContent.Uri = uri;
                        }
                    }
                }
            }
            return webContent;
        }

        public virtual WebContent GetContentFromThing(IThingGraph graph, IThing thing) {
            var info = ThingStreamFacade.GetContent(graph, thing);
            var uri = GetUri(thing);
            return GetContentFromInfo(info, uri);
        }

        public virtual WebContent GetContentFromGraph(IThingGraph graph, IThing thing, Uri uri) {
            CommonSchema schema = new CommonSchema();
            WebContent result = null;
            try {
                var schemaGraph = graph as SchemaThingGraph;
                if (thing != null && schemaGraph != null) {
                    var searchGraph = schemaGraph.Source;
                    string content = uri.Segments[uri.Segments.Length - 1];
                    foreach (ILink link in searchGraph.Edges(thing)) {
                        var adj = link.Leaf;
                        if (adj != thing && (adj is IStreamThing)) {
                            var desc = schemaGraph.Description(adj);
                            if (desc != null && desc.ToString() == content) {
                                return GetContentFromThing(schemaGraph, adj);
                            }
                        }
                    }
                }
                if (schemaGraph != null) {
                    foreach (var found in schemaGraph.GetByData(uri.AbsoluteUri, true)) {
                        var target = schema.GetTheRoot(schemaGraph, found, CommonSchema.SourceMarker);
                        if (target is IStreamThing) {
                            return GetContentFromThing(schemaGraph, target);
                        }
                    }
                }
            } catch (Exception e) {
                Trace.WriteLine(e.Message);
                return null;
            }
            return result;
        }

       

        #region MimeTypes - Refactor this
        IDictionary<long, string> _mimeTypes = null;
        public virtual IDictionary<long, string> MimeTypes {
            get {
                if (_mimeTypes == null) {
                    _mimeTypes = new Dictionary<long, string>();
                    _mimeTypes.Add(StreamTypes.HTML, "text/html");
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

        public virtual string MimeType(Int64 streamType) {
            string result = null;
            MimeTypes.TryGetValue(streamType, out result);
            return result;
        }
        #endregion
    }
}