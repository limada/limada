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
using System.Linq;
using Limaki.Contents;

namespace Limaki.WebServer {

    public class WebResponseBase {

        ContentInfos _contentInfoPool = null;
        protected ContentInfos ContentInfoPool { get { return _contentInfoPool ?? (_contentInfoPool = Registry.Pooled<ContentInfos>()); } }


        public bool IsStreamOwner { get; set; }
        public bool Done { get; set; }

        protected virtual WebContent GetContentFromContent (Content<Stream> content, Uri uri, bool useContentSource) {
            var webContent = new WebContent();
            webContent.ClearContentAfterServing = true;
            webContent.ContentIsStream = true;
            webContent.IsStreamOwner = this.IsStreamOwner;
            webContent.ContentStream = content.Data;
            webContent.Uri = uri;

            webContent.MimeType = ContentInfoPool.Where(ci=>ci.ContentType==content.ContentType).Select(ci=>ci.MimeType).FirstOrDefault();
            if (useContentSource) {
                var source = content.Source as string;
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
    }
}