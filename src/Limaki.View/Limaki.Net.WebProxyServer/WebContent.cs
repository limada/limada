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
using System.IO;
using System.Text;

namespace Limaki.Net.WebProxyServer {

    public class WebContent {

        public virtual Uri Uri { get; set; }
        private string _mimeType = null;
        
        public virtual string MimeType {
            get {
                if (_mimeType == null)
                    return "text/html";
                else
                    return _mimeType;
            }
            set { _mimeType = value; }
        }

        string _content = null;
        public virtual string Content {
            get {
                lock (lockObject) {
                    return _content;
                }
            }
            set {
                lock (lockObject) {
                    _content = value;
                }
            }
        }

        Stream _contentStream = null;
        public virtual Stream ContentStream {
            get {
                lock (lockObject) {
                    return _contentStream;
                }
            }
            set {
                lock (lockObject) {
                    if (IsStreamOwner && _contentStream != null) {
                        _contentStream.Dispose();
                        _contentStream = null;
                    }
                    _contentStream = value;
                }
            }
        }

        public virtual bool IsStreamOwner { get; set; }
        public virtual bool ContentIsStream { get; set; }
        
        public virtual bool ContentIsEmpty {
            get {
                if (ContentIsStream)
                    return _contentStream == null;
                else
                    return _content == null;
            }
        }

        private object lockObject = new object();

        public virtual void ClearContent() {
            Content = null;
            if (ContentStream != null && IsStreamOwner) {
                ContentStream.Close();
            }
            ContentStream = null;
        }
        public virtual bool ClearContentAfterServing { get; set; }

        string defaultContent() {
            return HtmlMessage(" running at " + Uri.AbsoluteUri);
        }

        public string HtmlMessage(string message) {
            var writer = new StringWriter();
            writer.Write("<HTML>");
            writer.Write(@"<head>
<style type=""text/css"">
.error
{
	font-size: medium;
	margin-left: 1em;
	font-family: Monospace;
	color: black;
	border-right: #339966 1px dashed;
	border-top: #339966 1px dashed;
	border-left: #339966 1px dashed;
	border-bottom: #339966 1px dashed;
	padding-right: 1em;
	padding-left: 1em;
	padding-bottom: 1em;
	padding-top: 1em;
}
</style>
</head>
");
            writer.Write("<BODY>");
            writer.Write(@"<div class=""error""><p>");
            writer.Write(message);
            writer.Write("</div>");
            writer.Write("</p></BODY></HTML>");
            writer.Flush();
            var result = writer.ToString();
            writer.Close();
            return result;
        }

        public virtual ResponseInfo Respond(RequestInfo requestInfo) {
            var result = new ResponseInfo();

            if (ContentIsStream && !ContentIsEmpty) {
                Stream stream = ContentStream;
                if (stream is MemoryStream) {
                    result.Data = ((MemoryStream)stream).GetBuffer();
                } else {
                    result.Data = new byte[stream.Length];
                    stream.Position = 0;
                    stream.Read(result.Data, 0, (int)stream.Length);
                    stream.Position = 0;
                }
            } else {
                var content = Content;
                if (ContentIsEmpty)
                    content = defaultContent();
                result.Data = Encoding.Convert(Encoding.Unicode, Encoding.ASCII,
                                               Encoding.Unicode.GetBytes(content));
            }
            result.Success = true;
            result.MimeType = this.MimeType;
            if (ClearContentAfterServing) {
                ClearContent();
            }

            return result;
        }


    }
}