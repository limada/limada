using System;
using System.Net;

namespace Limaki.WebServers {

    public interface IWebServer {

        void Listen();

        void StartListen();

        void Sleep();

        void Close();

        void AcceptRequestAsync(IAsyncResult ar);

        void ReadRequestAsync(IAsyncResult ar);

        WebContent NotFoundContent (RequestInfo requestInfo, out ResponseInfo responseInfo);

        string ServerName { get; set; }

        IPAddress Addr { get; set; }

        int Port { get; set; }

        string Authority { get; }

        Uri Uri { get; }

        Func<string, WebContent> ContentGetter { get; set; }

        bool Asycn { get; set; }

        void AddContent (string uri, Func<string, WebContent> content);

        void RemoveContent(string uri);

        WebContent GetContent(string uri);

        bool HasContent (string uri);

        void Start();

        Byte[] MakeHeader(string httpVersion, string mimeHeader, int totalBytes, string statusCode);

        event EventHandler Closed;

        void Dispose();

    }

}