using System;
using System.IO;
using Limaki.Model.Content;
using Limaki.Net.WebProxyServer;

namespace Limaki.Viewers.StreamViewers {
    public interface IWebResponse {
        bool IsStreamOwner { get; set; }
        bool Done { get; set; }
        Func<string, WebContent> Getter (Content<Stream> content);
        string AbsoluteUri { get; }
    }
}