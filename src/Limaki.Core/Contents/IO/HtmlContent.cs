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
using Limaki.Common.Text;
using Limaki.Model.Content;

namespace Limaki.Contents.IO {

    public class HtmlContentSpot : ContentDetector {

        public HtmlContentSpot (): base (

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
            
            var s = (TextHelper.IsUnicode(buffer) ? Encoding.Unicode.GetString(buffer) : Encoding.ASCII.GetString(buffer)).ToLower();
            if (
                s.Contains("<!doctype html") ||
                s.Contains("<html") ||
                s.Contains("<head") ||
                s.Contains("<body")) {
                    result = ContentSpecs.First(t => t.ContentType == ContentTypes.HTML);
            }

            if (
                s.Contains("<!doctype xhtml") ||
                s.Contains("<xhtml")
                ) {
                    result = ContentSpecs.First(t => t.ContentType == XHTML);
            }

            stream.Position = oldPos;
            return result;
        }

    }

    public class HtmlContentStreamIo : ContentStreamIo {
        public HtmlContentStreamIo () : base(new HtmlContentSpot()) {
            this.IoMode = IO.IoMode.ReadWrite;
        }
    }
}