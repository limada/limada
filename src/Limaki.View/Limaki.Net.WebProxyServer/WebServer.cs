/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

// Look to Manos as replacemet for this
// for threading problems see: Manos.Managed.IOLoop
namespace Limaki.Net.WebProxyServer {

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
            var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var endPoint = new IPEndPoint(this.Addr, this.Port);
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
            var socket = listener.EndAcceptSocket(ar);

            if (!socket.Connected || !running) {
                // Signal the main thread to continue.
                allDone.Set();
                return;
            }
            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = socket;
            if (Asycn) {
                try {
                    if (socket.Connected)
                        socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                                             new AsyncCallback(ReadRequest), state);
                } catch { }
            } else {
                var bytesRead = socket.Receive(state.buffer);
                if (bytesRead > 0) {
                    Respond(socket, state);
                }
            }
            // Signal the main thread to continue.
            allDone.Set();
        }

        public void ReadRequest(IAsyncResult ar) {
            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
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
                // this does not work:
                //handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                //        new AsyncCallback(ReadRequest), state);

                //} else  if (state.sb.Length>0) {
                //    Respond (handler, state);
                //}
            } catch(Exception ex) {
                Trace.WriteLine("ReadRequest failed "+ex.Message);
            }

        }

        private void Respond(Socket socket, StateObject state) {
            if (!socket.Connected || !running) {
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
                if (ContentGetter != null) {
                    content = ContentGetter(url);
                }
                //content = GetContent(url);
                //if (content != null)
                //    RemoveContent(url);
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
                Socket socket = (Socket)ar.AsyncState;
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