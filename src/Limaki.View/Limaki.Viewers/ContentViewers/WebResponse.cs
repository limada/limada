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
using System.IO;
using Limaki.Common;
using Limaki.Model.Content;
using Limaki.Net.WebProxyServer;
using System.Diagnostics;
using Limaki.Model.Content.IO;

namespace Limaki.Viewers.StreamViewers {

    public class WebResponse : WebResponseBase, IWebResponse {

        public virtual WebContent GetContentFromContent (Content<Stream> content, Uri uri) {
            return base.GetContentFromContent(content, uri, true);
        }

        public virtual Func<string, WebContent> Getter (Content<Stream> content) {
            var uri = new Uri (AbsoluteUri, UriKind.Absolute);
            var webContent = GetContentFromContent (content, uri);
            IsStreamOwner = webContent.IsStreamOwner;

            Done = false;

            Func<string, WebContent> getter =
                (s) => {
                    WebContent result = null;
                    try {
                        var request = new Uri(s);
                        if (webContent.Uri.AbsoluteUri == request.AbsoluteUri) {
                            if (!webContent.ContentIsEmpty) {
                                result = webContent;
                            }
                        } else {
                            result = new WebProxyContent();
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
            return getter;
        }

        public virtual string AbsoluteUri { get; set; }

        private IoProvider<Stream,Content<Stream>> _provider = null;
        IoProvider<Stream, Content<Stream>> Provider {
            get { return _provider ?? (_provider = Registry.Pool.TryGetCreate<IoProvider<Stream, Content<Stream>>>()); }
        }
        

      
    }
}