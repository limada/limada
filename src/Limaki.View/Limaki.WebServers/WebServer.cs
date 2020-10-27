/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

// Look to Manos as replacemet for this
// for threading problems see: Manos.Managed.IOLoop
namespace Limaki.WebServers {

    public class WebServer : WebServerBase {

        public class StateObject {
            // Client  socket.
            public Socket workSocket = null;
            // Size of receive buffer.
            public const int BufferSize = 1024 * 4;
            // Receive buffer.
            public byte[] buffer = new byte[BufferSize];
        }

        protected ManualResetEvent AllDone = new ManualResetEvent(false);

        public override void Listen() {
            if (ListenerTask == null) {
                ListenerTaskCancellationTokenSource = new CancellationTokenSource();
                ListenerTask = new Task (StartListen,ListenerTaskCancellationTokenSource.Token);
                ListenerTask.Start();
            }
        }

        bool running = false;
        public virtual void StartListen() {
            try {
                running = true;
                while (running) {
                    // Set the event to nonsignaled state.
                    AllDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    Trace.WriteLine("Waiting for a connection...");
                    Listener.BeginAcceptSocket(
                        new AsyncCallback(AcceptRequestAsync),
                        Listener);

                    // Wait until a connection is made before continuing.
                    AllDone.WaitOne();
                }

            } catch (Exception e) {
                Trace.WriteLine(e.ToString());
            }
        }

        public override void Sleep() {
            running = false;
            /// Create a connection to the port to unblock the listener thread
            var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var endPoint = new IPEndPoint(this.Addr, this.Port);
            sock.Connect(endPoint);
            sock = null;
            base.Sleep();
        }

        public override void Close() {
            running = false;
            base.Close();
        }

        public void AcceptRequestAsync(IAsyncResult ar) {
            if (!running)
                return;
            // Get the socket that handles the client request.
            var listener = (TcpListener)ar.AsyncState;
            var socket = listener.EndAcceptSocket(ar);

            if (!socket.Connected || !running) {
                // Signal the main thread to continue.
                AllDone.Set();
                return;
            }
            // Create the state object.
            var state = new StateObject();
            state.workSocket = socket;
            if (Asycn) {
                try {
                    if (socket.Connected)
                        socket.BeginReceive (state.buffer, 0, StateObject.BufferSize, 0,
                            new AsyncCallback (ReadRequestAsync), state);
                } catch (Exception ex) {
                    Trace.WriteLine (ex.Message);
                }
            } else {
                var bytesRead = socket.Receive(state.buffer);
                if (bytesRead > 0) {
                    Respond(socket, state);
                }
            }
            // Signal the main thread to continue.
            AllDone.Set();
        }

        public void ReadRequestAsync(IAsyncResult ar) {
            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            var state = (StateObject)ar.AsyncState;
            var requestInfo = new RequestInfo (state.buffer);
            var socket = state.workSocket;
            if (!socket.Connected || !running) {
                return;
            }
            try {
                // Read data from the client socket. 
                int bytesRead = socket.EndReceive(ar);

                if (bytesRead > 0) {
                    Respond(socket, state);
                }

            } catch(Exception ex) {
                Trace.WriteLine("ReadRequest failed "+ex.Message);
            }

        }

        public WebContent NotFoundContent (RequestInfo requestInfo, out ResponseInfo responseInfo) {
            var content = new WebContent ();
            content.Content = content.HtmlMessage (ServerName + " at " + this.Authority +
                                                  ": <br>ERROR: " + requestInfo.Url + "<br> not found<br>");
            responseInfo = content.Respond (requestInfo);
            responseInfo.Success = false;
            responseInfo.StatusCode = ResponseInfo.StatusCodeNotFound;
            Trace.WriteLine ("\trequest not found:\t " + requestInfo.Request);
            return content;
        }

        private void Respond(Socket socket, StateObject state) {
            if (!socket.Connected || !running) {
                return;
            }
            
            var requestInfo = new RequestInfo(state.buffer);

            Trace.WriteLine ("Requested:\t" + requestInfo.Url);

            ResponseInfo responseInfo = null;
            WebContent content = null;

            if (!requestInfo.Success) {
                content = NotFoundContent (requestInfo, out responseInfo);

            } else {
                if (ContentGetter != null) {
                    content = ContentGetter (requestInfo.Url);
                }

                if (content == null) {
                    content = NotFoundContent (requestInfo, out responseInfo);
                } else {
                    responseInfo = content.Respond (requestInfo);
                    if (!responseInfo.Success) {
                        content = NotFoundContent (requestInfo, out responseInfo);
                    }
                }
            }
            var header =
                MakeHeader (requestInfo.HttpVersion, responseInfo.MimeType, responseInfo.Data.Length, responseInfo.StatusCode);

            byte[] buffer = new Byte[header.Length + responseInfo.Data.Length];
            Buffer.BlockCopy(header, 0, buffer, 0, header.Length);
            Buffer.BlockCopy(responseInfo.Data, 0, buffer, header.Length, responseInfo.Data.Length);
            if (Asycn) {
                try {
                    if (socket.Connected)
                        socket.BeginSend(buffer, 0, buffer.Length, 0, new AsyncCallback(RespondCallback), socket);
                } catch (Exception ex) {
                    Debug.WriteLine("Error in webserver-respond:" + ex.Message);
                }
            } else {
                if (socket.Connected && running) {
                    int bytesSent = socket.Send(buffer);
                    Trace.WriteLine("Sent " + bytesSent + " bytes to client.");

                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
            }

        }

        private void RespondCallback(IAsyncResult ar) {
            try {
                // Retrieve the socket from the state object.
                var socket = (Socket)ar.AsyncState;
                if (socket.Connected && running) {
                    // Complete sending the data to the remote device.
                    int bytesSent = socket.EndSend(ar);
                    Trace.WriteLine("Sent " + bytesSent + " bytes to client.");

                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
            } catch (Exception e) {
                Trace.WriteLine("Error in webserver-respondcallback:"+e.Message);
            }
        }

    }
}