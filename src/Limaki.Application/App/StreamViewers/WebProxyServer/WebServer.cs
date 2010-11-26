/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Limaki.Common.Text.HTML;

namespace Limaki.App.StreamViewers.WebProxyServer {
    public abstract class WebServerBase : IDisposable {
        public WebServerBase() {
            Port = 40110;
            ServerName = "Limaki.ContentServer";
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
                    _uri = new Uri("http://"+Authority, UriKind.Absolute);
                }
                return _uri;
            }
        }



        private object locker = new object();

        public Func<string, WebContent> GetContent = null;


        

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
            GetContent = null;
        }

    }
    public class WebServer : WebServerBase {
        public class StateObject {
            // Client  socket.
            public Socket workSocket = null;
            // Size of receive buffer.
            public const int BufferSize = 1024 * 4;
            // Receive buffer.
            public byte[] buffer = new byte[BufferSize];
        }

        public ManualResetEvent allDone = new ManualResetEvent(false);
        public override void Listen() {
            if (listenerThread == null) {
                //start the thread which calls the method 'StartListen'
                listenerThread = new Thread(new ThreadStart(StartListen));
                listenerThread.Name = ServerName + " running at " + Authority;
                listenerThread.Start();

            }
        }

        bool running = false;
        public virtual void StartListen() {
            try {
                running = true;
                while (running) {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    Trace.WriteLine("Waiting for a connection...");
                    listener.BeginAcceptSocket(
                        new AsyncCallback(AcceptRequest),
                        listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }

            } catch (Exception e) {
                Trace.WriteLine(e.ToString());
            }



        }
        public override void Sleep() {
            running = false;
            /// Create a connection to the port to unblock the listener thread
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(this.Addr, this.Port);
            sock.Connect(endPoint);
            sock = null;
            base.Sleep();
        }
        public override void Close() {

            base.Close();
        }
        public void AcceptRequest(IAsyncResult ar) {
            // Get the socket that handles the client request.
            TcpListener listener = (TcpListener)ar.AsyncState;
            Socket handler = listener.EndAcceptSocket(ar);

            // Signal the main thread to continue.
            allDone.Set();

            if (!handler.Connected || !running) {
                return;
            }
            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadRequest), state);

            // sync:
            //int bytesRead = handler.Receive(state.buffer);
            //if (bytesRead > 0) {
            //    state.sb.Write(state.buffer, 0, bytesRead);
            //    Respond(handler, state);
            //}
        }

        public void ReadRequest(IAsyncResult ar) {
            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            if (!handler.Connected || !running) {
                return;
            }

            // Read data from the client socket. 
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0) {
                Respond(handler, state);
            }
            // this does not work:
            //handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
            //        new AsyncCallback(ReadRequest), state);

            //} else  if (state.sb.Length>0) {
            //    Respond (handler, state);
            //}
        }

        private void Respond(Socket handler, StateObject state) {
            if (!handler.Connected || !running) {
                return;
            }

            var requestInfo = new RequestInfo(state.buffer);
            var url = requestInfo.Request;
            if (requestInfo.Uri != null)
                url = requestInfo.Uri.AbsoluteUri;

            Trace.WriteLine("Requested:\t" + url);

            ResponseInfo responseInfo = null;
            WebContent content = null;

            var statusCode = " 200 OK";
            if (!requestInfo.Success) {
                content = new WebContent();
                content.Content = requestInfo.Request;
                responseInfo = content.Respond(requestInfo);
            } else {
                if (GetContent != null) {
                    content = GetContent(url);
                }
                if (content == null) {
                    content = new WebContent();
                    content.Uri = new Uri(this.Uri.AbsoluteUri);
                }

                responseInfo = content.Respond(requestInfo);
                if (!responseInfo.Success) {
                    content = new WebContent();
                    content.Content = content.HtmlMessage(ServerName + " at " + this.Authority +
                                      ": <br>ERROR: " + url + "<br> not found<br>");
                    statusCode = " 404 Not Found";
                    responseInfo = content.Respond(requestInfo);
                    Trace.WriteLine("\trequest denied:\t " + requestInfo.Request);
                }

            }
            var header =
                MakeHeader(requestInfo.HttpVersion, responseInfo.MimeType, responseInfo.Data.Length, statusCode);

            byte[] buffer = new Byte[header.Length + responseInfo.Data.Length];
            Buffer.BlockCopy(header, 0, buffer, 0, header.Length);
            Buffer.BlockCopy(responseInfo.Data, 0, buffer, header.Length, responseInfo.Data.Length);

            handler.BeginSend(buffer, 0, buffer.Length, 0, new AsyncCallback(RespondCallback), handler);
            //handler.Send (buffer);
            //handler.Close ();
            
        }

        private void RespondCallback(IAsyncResult ar) {
            try {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;
                if (handler.Connected && running) {
                    // Complete sending the data to the remote device.
                    int bytesSent = handler.EndSend(ar);
                    Trace.WriteLine("Sent " + bytesSent + " bytes to client.");

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            } catch (Exception e) {
                Trace.WriteLine(e.ToString());
            }
        }


    }
}