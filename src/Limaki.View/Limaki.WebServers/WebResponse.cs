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
using Limaki.Contents;
using System.Diagnostics;
using Limaki.Contents.IO;

namespace Limaki.WebServers {

    public class WebResponse : WebResponseBase, IWebResponse {

        public virtual WebContent GetContentFromContent (Content<Stream> content, Uri uri) {
            var result = base.GetContentFromContent(content, uri, true);
            result.IsStreamOwner = this.IsStreamOwner;
            return result;
        }

        public virtual WebContent GetContentFromContent (Content<Stream> content) {
            return GetContentFromContent (content, new Uri (AbsoluteUri, UriKind.Absolute));
        }

        public virtual Func<string, WebContent> Getter (Content<Stream> content) {
            var webContent = GetContentFromContent (content);

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
                            // try to get the content form the web
                            result = new WebRequestContent();
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

        private StreamContentIoPool _pool = null;
        StreamContentIoPool ContentIoPool {
            get { return _pool ?? (_pool = Registry.Pooled<StreamContentIoPool>()); }
        }
        

      
    }
}