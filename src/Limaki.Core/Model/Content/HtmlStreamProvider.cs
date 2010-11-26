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
using System.IO;
using System.Collections.Generic;
using Limaki.Common.Collections;
using Limaki.Common;
using System.Text;
using Limaki.Model.Streams;

namespace Limaki.Model.Content {

    public class HtmlStreamProvider : StreamProvider {
        static long XHTML = 0x280efaf080c35e30;

        StreamTypeInfo[] _supportedStreamTypes =
            new StreamTypeInfo[]{
				new StreamTypeInfo(
			        "HTML",
                    StreamTypes.HTML,
                    "html",
                    "text/html",
                    CompressionType.bZip2
			),
            new StreamTypeInfo(
			        "XHTML",
                    XHTML,
                    "xhtml",
                    "application/xhtml+xml",
                    CompressionType.bZip2
			)
			
		};

        public override IEnumerable<StreamTypeInfo> SupportedStreamTypes {
            get { return _supportedStreamTypes; }
        }

        public override StreamTypeInfo SupportingInfo(Stream stream) {

            StreamTypeInfo result = null;
            
            var oldPos = stream.Position;
            int buflen = Math.Min(256, (int)stream.Length);
            var buffer = new byte[buflen];

            stream.Read(buffer, 0, buflen);

            var s = Encoding.ASCII.GetString(buffer).ToLower();
            if(
                s.Contains("<!doctype html") ||
                s.Contains("<html") ||
                s.Contains("<head") ||
                s.Contains("<body")) {
                result = _supportedStreamTypes[0];
            }

            if (
               s.Contains("<!doctype xhtml") ||
               s.Contains("<xhtml") 
                ) {
                result = _supportedStreamTypes[1];
            }

            stream.Position = oldPos;
            return result;
        }

        public override bool Saveable { get { return true; } }
    }
}
