using System;
using System.IO;
using Limaki.Common;
using Limaki.Model.Content;
using Limaki.Net.WebProxyServer;
using System.Diagnostics;

namespace Limaki.Viewers.StreamViewers {

    public class WebResponse:IWebResponse {

        public bool IsStreamOwner { get; set; }

        public bool Done { get; set; }

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

        private ContentProviders _providers = null;
        ContentProviders Providers {
            get { return _providers ?? (_providers = Registry.Pool.TryGetCreate<ContentProviders> ()); }
        }

        // TODO: this is a copy of ThingWebResponse; unify usage

        public virtual WebContent GetContentFromContent (Content<Stream> content, Uri uri) {
            var webContent = new WebContent ();
            webContent.ClearContentAfterServing = true;
            webContent.ContentIsStream = true;
            webContent.IsStreamOwner = this.IsStreamOwner;
            webContent.ContentStream = content.Data;
            webContent.Uri = uri;

            webContent.MimeType = Providers.MimeType (content.StreamType);

            var source = content.Source as string;
            if (source != null && source != "about:blank") {
                if (Uri.IsWellFormedUriString (source, UriKind.RelativeOrAbsolute)) {
                    uri = null;
                    Uri.TryCreate (source, UriKind.RelativeOrAbsolute, out uri);
                    if (uri != null && !uri.IsUnc && !uri.IsFile) {
                        webContent.Uri = uri;
                    }
                }
            }

            return webContent;
        }
    }
}