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
 * http://www.limada.org
 * 
 */

using System;
using System.Text;
using System.Diagnostics;

namespace Limaki.WebServers {

    public class RequestInfo {

        Uri _uri = null;
        public Uri Uri { get { return _uri; } set { _uri = value; } }

        public string Request = string.Empty;

        public string HttpVersion = string.Empty;
        public string Params { get; protected set; }
        public string Accept { get; protected set; }

        public bool Success { get; set; }

        public RequestInfo(Byte[] request)  {
            HandleRequest(request);
        }

        public string Url {
            get {
                var url = Request;
                if (Uri != null)
                    if (Uri.IsAbsoluteUri)
                        url = Uri.AbsoluteUri;
                    else {
                        url = Uri.AbsolutePath;
                    }
                if (!string.IsNullOrEmpty (Params))
                    url += "?" + Params;
                return url;
            }
        }

        public void HandleRequest(Byte[] request) {
            int startPos = 0;
            string requestetUri;
            //Convert Byte to String
            string buffer = Encoding.ASCII.GetString(request);

            //At present we will only deal with GET type
            if (buffer.Substring(0, 3) != "GET") {
                this.Request = "Only Get Method is supported..";
                this.Success = false;
                return ;
            }

            // Look for HTTP request
            startPos = buffer.IndexOf("HTTP/", 1);

            // Get the HTTP text and version e.g. it will return "HTTP/1.1"
            this.HttpVersion = buffer.Substring(startPos, 8);

            // Extract the Requested Uri (without host)
            requestetUri = buffer.Substring(4, startPos - 5);

            // Extract params, if some
            var paramPos = requestetUri.IndexOf ('?');
            if (paramPos != -1) {
                this.Params = requestetUri.Substring (paramPos+1);
                requestetUri = requestetUri.Substring (0, paramPos);
                
            }

            // Look for Host request
            startPos = buffer.IndexOf("Host:", startPos);
            if (startPos != -1) {
                startPos += 6;
                var endPos = buffer.IndexOf ("\r\n", startPos);
                if (endPos != -1) {
                    var host = buffer.Substring (startPos, endPos-startPos);
                    if (requestetUri.IndexOf("://")==-1) {
                        requestetUri = "http://" + host + requestetUri;
                    }
                }
            }

            // Look for Accept request
            startPos = buffer.IndexOf ("Accept:", 1);
            if (startPos != -1) {
                startPos += 7;
                 var endPos = buffer.IndexOf (",", startPos);
                if (endPos != -1) {
                    this.Accept = buffer.Substring (startPos, endPos - startPos);
                }
            }

            bool isWellFormedUriString = Uri.IsWellFormedUriString(requestetUri, UriKind.RelativeOrAbsolute);
            this.Uri = null;
            if (requestetUri.Length > 1 && isWellFormedUriString) {
                try {
                    Uri.TryCreate (requestetUri, UriKind.RelativeOrAbsolute, out this._uri);
                    if (this.Uri != null) {
                        this.Request = this.Uri.Segments[Uri.Segments.Length - 1];
                    }
                } catch (Exception e) {
                    Trace.WriteLine (e.Message);
                }
            } else {
                this.Request = requestetUri;
            }

            //result.MimeType = GetMimeType(result.Request);
            this.Success = true;
        }

        
    }

    public class ResponseInfo {
        public ResponseInfo () {
            MimeType = string.Empty;
            StatusCode = StatusCodeOK;
        }
        public byte[] Data { get; set; }
        public string MimeType { get; set; }
        public bool Success { get; set; }
        public string StatusCode { get; set; }

        public const string StatusCodeOK = " 200 OK";
        public const string StatusCodeNotFound = " 404 Not Found";
    }
}