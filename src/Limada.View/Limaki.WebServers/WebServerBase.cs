using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Limaki.Common;

namespace Limaki.WebServers {

    public abstract class WebServerBase : IDisposable {
        public WebServerBase() {
            Port = 40110;
            ServerName = "Limaki.ContentServer";
            Asycn = true; // !OS.Mono;
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

        #region content-handlers

        // TODO: replace this with ConcurrentDictionary<TKey,TValue>
        protected IDictionary<string,Func<string, WebContent>> Contents = new Dictionary<string,Func<string, WebContent>>();

        protected object ContentsLock = new object ();
        public void AddContent (string uri, Func<string, WebContent> content) {
            lock (ContentsLock) {
                Contents[uri] = content;
            }
        }

        public void RemoveContent(string uri) {
            lock (ContentsLock) {
                Contents.Remove(uri);
            }
        }

        public WebContent GetContent(string uri) {
            lock (ContentsLock) {
                Func<string, WebContent> result = null;
                if(Contents.TryGetValue(uri, out result))
                    return result(uri);
                return null;
            }
        }

        public bool HasContent (string uri) {
            lock (ContentsLock) {
                return Contents.ContainsKey (uri);
            }
        }

        #endregion

        public Func<string, WebContent> ContentGetter { get; set; }

        public bool Asycn { get; set; }
        
        protected Task ListenerTask { get; set; }
        protected CancellationTokenSource ListenerTaskCancellationTokenSource { get; set; }

        public static int PortFree (IPAddress adress, int port) {
            var ipProperties = IPGlobalProperties.GetIPGlobalProperties ();
            var ipEndPoints = ipProperties.GetActiveTcpListeners ();

            bool InUse (int aport) {
                foreach (var endPoint in ipEndPoints) {
                    if (endPoint.Port == aport && endPoint.Address.Equals (adress)) {
                        return true;
                    }
                }

                return false;
            }

            for (var i = port; i < port + 100; i++) {
                if (!InUse (i))
                    return i;
            }

            return default;
        }


        public virtual void Start() {
            try {
                //start listing on the given port

                var freePort = PortFree (Addr, Port);
                if (freePort == default) {
                    throw new SocketException(98);
                }

                Port = freePort;
                Listener = new TcpListener(Addr, Port);
                Listener.Start();
                if (false) {
                    int count = 0;
                    bool hasPort = false;
                    while (count < 100 && !hasPort)
                        try {
                            count++;
                            Listener = new TcpListener (Addr, Port);
                            Listener.Start ();
                            hasPort = true;
                        } catch (SocketException ex) {
                            if (ex.ErrorCode == 10048 || ex.ErrorCode == 98) {
                                Port = Port + 1;
                            } else {
                                throw ex;
                            }
                        }
                }

                Trace.WriteLine(ServerName + " Running at: " + Authority);
                Listen();
            } catch (Exception e) {
                Trace.WriteLine("An Exception Occurred while Listening :" + e.ToString());
            }
        }

        protected TcpListener Listener { get; set; }

        public Byte[] MakeHeader(string httpVersion, string mimeHeader, int totalBytes, string statusCode) {
            var buffer = new StringBuilder();

            // if Mime type is not provided set default to text/html
            if (mimeHeader == null || mimeHeader.Length == 0) {
                mimeHeader = "text/html";  // Default Mime Type is text/html
            }

            buffer.Append(httpVersion + statusCode + "\r\n");
            buffer.Append("Server: " + ServerName + "\r\n");
            buffer.Append("Content-Type: " + mimeHeader + "\r\n");
            buffer.Append("Accept-Ranges: bytes\r\n");
            buffer.Append("Content-Length: " + totalBytes + "\r\n\r\n");

            var sendData = Encoding.ASCII.GetBytes(buffer.ToString());


            return sendData;
        }

        public abstract void Listen();

        public event EventHandler Closed;

        public virtual void Close() {
            Sleep();
            if (Listener != null) {
                Listener.Stop();
                Listener = null;
            }

            if (Closed != null) {
                Closed (this, new EventArgs ());
            }
        }

        public virtual void Sleep() {
            if (ListenerTask != null) {
                ListenerTaskCancellationTokenSource.Cancel();
                ListenerTask = null;
                ListenerTaskCancellationTokenSource = null;
            }

        }

        public virtual void Dispose() {
            Close();
            ContentGetter = null;
            Contents.Clear();
        }

    }
}