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
using Limaki.Model.Content.IO;
using Limaki.Net.WebProxyServer;
using System.Linq;

namespace Limaki.Viewers.StreamViewers {

    public class WebResponseBase {

        ContentInfos _contentInfos = null;
        protected ContentInfos ContentInfos { get { return _contentInfos ?? (_contentInfos = Registry.Pool.TryGetCreate<ContentInfos>()); } }


        public bool IsStreamOwner { get; set; }
        public bool Done { get; set; }

        protected virtual WebContent GetContentFromContent (Content<Stream> content, Uri uri, bool useContentSource) {
            var webContent = new WebContent();
            webContent.ClearContentAfterServing = true;
            webContent.ContentIsStream = true;
            webContent.IsStreamOwner = this.IsStreamOwner;
            webContent.ContentStream = content.Data;
            webContent.Uri = uri;

            webContent.MimeType = ContentInfos.Where(ci=>ci.ContentType==content.ContentType).Select(ci=>ci.MimeType).FirstOrDefault();
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