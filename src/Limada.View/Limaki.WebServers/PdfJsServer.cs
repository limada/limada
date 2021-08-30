/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Limaki.Common.Linqish;
using Limaki.Contents;
using Limaki.Contents.IO;
using Limaki.Usecases;

namespace Limaki.WebServers {
    /// <summary>
    /// Serves pdf.js to show pdf-files
    /// </summary>
    public class PdfJsServer:IDisposable {

        public static string PdjJsDirectory = "pdf.js";
        public static string ViewerFileName = "web/viewer.html";

        public static bool PdfJsAvailable () {
            return File.Exists (new PluginLocator ().PluginDir (PdjJsDirectory) + Path.DirectorySeparatorChar + ViewerFileName);
        } 

        private int _port = -1;
        public int Port {
            get {
                if (_port == -1) {
                    _port = 41100;
                }
                return _port;

            }
            set { _port = value; }
        }

        private bool _webServerOwned = false;
        private WebServers.WebServer _webServer = null;
        public WebServers.WebServer WebServer {
            get {
                if (_webServer == null) {
                    _webServer = new WebServers.WebServer ();
                    _webServer.Port = this.Port;
                    _webServerOwned = true;
                    _webServer.Start ();
                }
                return _webServer;
            }
        }

        public string ViewerUri { get; protected set; }

        public Stream Pdf { get; set; }

        public Func<string, WebContent> ContentGetter { get; protected set; }

        public void Init () {

            var fileCache = new Dictionary<string, Content<Stream>> ();
            Func<string, Content<Stream>> file2Content = (path) => {
                Content<Stream> result = null;
                if (!fileCache.TryGetValue (path, out result)) {
                    var s = File.OpenRead (path);
                    s.Position = 0;
                    result = new Content<Stream> (s, CompressionType.None, ContentTypes.Unknown);
                    fileCache.Add (path, result);
                }
                return result;
            };

            var pdfResponse = new WebResponse {
                AbsoluteUri = WebServer.Uri + "pdf",
                ClearContentAfterServing = false,
                IsStreamOwner = false
            };

            var viewerDir = new PluginLocator ().PluginDir (PdjJsDirectory) + Path.DirectorySeparatorChar;
            var viewerServerPath = WebServer.Uri + PdjJsDirectory + "/";

            this.ContentGetter = (uri) => {

                if (pdfResponse.AbsoluteUri == uri)
                    return pdfResponse.GetContentFromContent (
                        new Content<Stream> (
                            this.Pdf,
                            CompressionType.None,
                            PdfContentSpot.PdfContentType));

                if (uri.StartsWith (viewerServerPath)) {
                    if (WebServer.HasContent (uri)) {
                        return WebServer.GetContent (uri);
                    } else {
                        var fileName = uri.Replace (viewerServerPath, viewerDir);
                        if (fileName.Contains ('?'))
                            fileName = fileName.Substring (0, fileName.IndexOf ('?'));
                        var c = file2Content (fileName);
                        var r = new WebResponse { AbsoluteUri = uri, ClearContentAfterServing = false, IsStreamOwner = false };
                        var fileGetter = r.Getter (c);
                        WebServer.AddContent (r.AbsoluteUri, fileGetter);
                        return fileGetter (uri);
                    }
                } else {
                    Trace.WriteLine ("Request not answered: " + uri);
                }

                return null;
            };

            WebServer.ContentGetter = this.ContentGetter;

            WebServer.Closed += (s, e) => {
                fileCache.Values.ForEach (c => c.Data.Dispose ());
                fileCache.Clear ();
            };

            ViewerUri = viewerServerPath + ViewerFileName + "?file=" +
                        System.Net.WebUtility.HtmlEncode (pdfResponse.AbsoluteUri);
        }

        public void Close () {
            if (_webServerOwned)
                WebServer.Close ();
        }

        public void Dispose () {
            Close ();
        }
    }
}