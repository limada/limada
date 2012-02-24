using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace Limaki.UseCases.Viewers.StreamViewers.WebProxyServer {
    public abstract class WebServerBase : IDisposable {
        public WebServerBase() {
            Port = 40110;
            ServerName = "Limaki.ContentServer";
            Asycn = !OS.Mono;
        }

        public string ServerName { get; set; }

        private IPAddress _addr = IPAddress.Loopback;
        public IPAddress Addr {
            get { return _addr; }
            set {
                if (_addr != value) {
                    _addr = value;
                    _uri = null;
                }
            }
        }

        private int _port = 40110;
        public int Port {
            get { return _port; }
            set {
                if (_port != value) {
                    _port = value;
                    _uri = null;
                }
            }
        }

        public string Authority {
            get { return string.Format(Addr.ToString() + ":{0}", Port); }
        }

        Uri _uri = null;
        public Uri Uri {
            get {
                if (_uri == null) {
                    _uri = new Uri("http://" + Authority, UriKind.Absolute);
                }
                return _uri;
            }
        }

        #region try to use stack or something
        /// <summary>
        /// replace this with ConcurrentDictionary<TKey,TValue>
        /// </summary>
        protected IDictionary<string,Func<string, WebContent>> Contents = new Dictionary<string,Func<string, WebContent>>();

        protected object contentsLock = new object();
        public void AddContent(string uri, Func<string, WebContent> content) {
            lock(contentsLock) {
                Contents[uri]= content;
            }
        }
        public void RemoveContent(string uri) {
            lock (contentsLock) {
                Contents.Remove(uri);
            }
        }
        public WebContent GetContent(string uri) {
            lock (contentsLock) {
                Func<string, WebContent> result = null;
                if(Contents.TryGetValue(uri, out result))
                    return result(uri);
                return null;
            }
        }
        #endregion

        public Func<string, WebContent> ContentGetter = null;

        public bool Asycn { get; set; }

        protected Thread listenerThread = null;

        public virtual void Start() {
            try {
                //start listing on the given port

                int count = 0;
                bool hasPort = false;
                while (count < 100 && !hasPort)
                    try {
                        count++;
                        listener = new TcpListener(Addr, Port);
                        listener.Start();
                        hasPort = true;
                    } catch (SocketException ex) {
                        if (ex.ErrorCode == 10048) {
                            Port = Port + 1;
                        } else {
                            throw ex;
                        }
                    }
                Trace.WriteLine(ServerName + " Running at: " + Authority);
                Listen();
            } catch (Exception e) {
                Trace.WriteLine("An Exception Occurred while Listening :" + e.ToString());
            }
        }
        protected TcpListener listener;

        public Byte[] MakeHeader(string httpVersion, string mimeHeader, int totalBytes, string statusCode) {
            StringBuilder buffer = new StringBuilder();

            // if Mime type is not provided set default to text/html
            if (mimeHeader == null || mimeHeader.Length == 0) {
                mimeHeader = "text/html";  // Default Mime Type is text/html
            }

            buffer.Append(httpVersion + statusCode + "\r\n");
            buffer.Append("Server: " + ServerName + "\r\n");
            buffer.Append("Content-Type: " + mimeHeader + "\r\n");
            buffer.Append("Accept-Ranges: bytes\r\n");
            buffer.Append("Content-Length: " + totalBytes + "\r\n\r\n");

            Byte[] sendData = Encoding.ASCII.GetBytes(buffer.ToString());


            return sendData;
        }

        public abstract void Listen();
        public virtual void Close() {
            Sleep();
            if (listener != null) {
                listener.Stop();
                listener = null;
            }


        }

        public virtual void Sleep() {
            if (listenerThread != null) {
                listenerThread.Abort();
                listenerThread = null;
            }
        }

        public virtual void Dispose() {
            Close();
            ContentGetter = null;
            Contents.Clear();
        }

    }
}