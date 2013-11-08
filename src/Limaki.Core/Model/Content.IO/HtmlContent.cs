/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2013 Lytico
 *
 * http://www.limada.org
 * 
 */


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Limaki.Common.Text.HTML;

namespace Limaki.Model.Content.IO {

    public class HtmlContentInfo : ContentInfoSink {

        public HtmlContentInfo (): base (

                new ContentInfo[]{
                                 new ContentInfo(
                                     "HTML",
                                     ContentTypes.HTML,
                                     "html",
                                     "text/html",
                                     CompressionType.bZip2
                                     ),
                                 new ContentInfo(
                                     "XHTML",
                                     XHTML,
                                     "xhtml",
                                     "application/xhtml+xml",
                                     CompressionType.bZip2
                                     )
                             }
            ) {}

        public static long XHTML = 0x280efaf080c35e30;

        public override bool SupportsMagics { 
            get { return true; }
        }

        public override ContentInfo Use (Stream stream) {

            ContentInfo result = null;

            var oldPos = stream.Position;
            int buflen = Math.Min(2048, (int)stream.Length);
            var buffer = new byte[buflen];

            stream.Read(buffer, 0, buflen);
            
            var s = (HtmlHelper.IsUnicode(buffer) ? Encoding.Unicode.GetString(buffer) : Encoding.ASCII.GetString(buffer)).ToLower();
            if (
                s.Contains("<!doctype html") ||
                s.Contains("<html") ||
                s.Contains("<head") ||
                s.Contains("<body")) {
                    result = SupportedContents.First(t => t.ContentType == ContentTypes.HTML);
            }

            if (
                s.Contains("<!doctype xhtml") ||
                s.Contains("<xhtml")
                ) {
                    result = SupportedContents.First(t => t.ContentType == XHTML);
            }

            stream.Position = oldPos;
            return result;
        }

    }

    public class HtmlContentStreamIo : ContentStreamSinkIo {
        public HtmlContentStreamIo (): base(new HtmlContentInfo()) {}
    }

    public class HtmlContentDigger : ContentDigger {
        private static HtmlContentInfo _info = new HtmlContentInfo();
        public HtmlContentDigger () : base() {
            this.DiggUse = Digg;

        }

        protected virtual Content<Stream> Digg (Content<Stream> source, Content<Stream> sink) {
            if (!_info.Supports(source.ContentType))
                return sink;
            var buffer = new byte[source.Data.Length];
            source.Data.Read(buffer, 0, buffer.Length);
            Digg(Encoding.Unicode.GetString(buffer), sink);
            return sink;
        }

        protected virtual void Digg (string source, Content<Stream> sink) {
            // TODO: refactor to be useable with source as Content<Stream>
            // TODO: find a sink.Description
            try {
                int startIndex = -1;
                int endIndex = -1;
                string subText = Between(source, "StartHTML:", "\r\n", 0);
                if (subText != null) int.TryParse(subText, out startIndex);
                subText = Between(source, "EndHTML:", "\r\n", 0);
                if (subText != null)
                    int.TryParse(subText, out endIndex);
                if (startIndex != -1 && endIndex != -1) {
                    endIndex = Math.Min(source.Length, endIndex);
                    sink.Source = Between(source, "SourceURL:", "\r\n", 0);
                    sink.Data = new MemoryStream(Encoding.Unicode.GetBytes(source.Substring(startIndex, endIndex - startIndex)));
                }
            } catch (Exception e) {
                throw e;
            }
        }

        protected virtual string Between (string text, string start, string end, int startIndex) {
            int posStart = text.IndexOf(start, startIndex);
            if (posStart == -1) return null;
            posStart += start.Length;
            int posEnd = text.IndexOf(end, posStart);
            if (posEnd == -1) return null;
            return text.Substring(posStart, posEnd - posStart);
        }
    }
}