/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Limada.Model;
using Limada.Schemata;
using Limaki.Contents;
using Limaki.Model.Content;
using Limaki.Net.WebProxyServer;

namespace Limaki.Viewers.StreamViewers {

    public class ThingWebResponse : WebResponseBase,IWebResponse {

        public virtual IThingGraph ThingGraph { get; set; }
        public virtual IThing Thing { get; set; }

        public bool UseProxy { get; set; }

        public Uri BaseUri { get; set; }

        public WebContent WebContent { get; protected set; }

        public virtual Func<string, WebContent> Getter(Content<Stream> content) {
            var graph = this.ThingGraph;
            var thing = this.Thing;

            WebContent = GetContentFromContent(content, GetUri(thing));
            IsStreamOwner = WebContent.IsStreamOwner;

            Done = false;

            Func<string, WebContent> getter =
                (s) => {
                    WebContent result = null;
                    try {
                        var request = new Uri(s);
                        if (WebContent.Uri.AbsoluteUri == request.AbsoluteUri) {
                            if (!WebContent.ContentIsEmpty) {
                                result = WebContent;
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


        public virtual WebContent GetContentFromContent (Content<Stream> content, Uri uri) {
            return base.GetContentFromContent(content, uri, this.UseProxy);
        }


        public virtual WebContent GetContentFromThing(IThingGraph graph, IThing thing) {
            var info = ThingContentFacade.ContentOf(graph, thing);
            var uri = GetUri(thing);
            return GetContentFromContent(info, uri);
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

        public string AbsoluteUri {
            get { return WebContent.Uri.AbsoluteUri; }
        }
 

      

      
      
      
    }
}