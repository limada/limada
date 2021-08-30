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
 */

using System;
using System.IO;
using Limaki.Contents;
using Limaki.Contents.IO;
using Limaki.WebServers;

namespace Limaki.View.ContentViewers {

    public class PdfContentViewer : HtmlContentViewerBase {

        public static bool Available () {
            return PdfJsServer.PdfJsAvailable ();
        }

        public override bool Supports (long streamType) {
            return streamType == PdfContentSpot.PdfContentType;
        }

        public override bool UseWebServer { get { return true; } set { base.UseWebServer = value; } }

        PdfJsServer _pdfJsServer = null;
        protected PdfJsServer PdfJsServer {
            get {
                if (_pdfJsServer == null) {
                    _pdfJsServer = new PdfJsServer ();
                    this.Port = _pdfJsServer.Port;
                    _pdfJsServer.Init ();
                }
                return _pdfJsServer;
            }
        }

        public override WebServer WebServer {
            get {
                if (_webServer == null) {
                   _webServer = PdfJsServer.WebServer;
                }
                return _webServer;
            }
        }

        public class PdfWebResponse : WebResponseBase, IWebResponse {

            public PdfWebResponse (PdfJsServer server) {
                this.Server = server;
            }

            public Func<string, WebContent> Getter (Content<Stream> content) {
                Server.Pdf = content.Data;
                AbsoluteUri = Server.ViewerUri;
                return Server.ContentGetter;
            }

            public virtual string AbsoluteUri { get; protected set; }

            protected PdfJsServer Server { get; set; }
        }

        public override IWebResponse CreateResponse () {
            return new PdfWebResponse (this.PdfJsServer);
        }

        
    }
}