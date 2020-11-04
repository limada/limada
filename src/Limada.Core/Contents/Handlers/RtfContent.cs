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

using System.IO;
using System.Text;
using Limaki.Common;
using Limaki.Common.Text;
using Limaki.Contents;

namespace Limaki.Contents.IO {

    public class RtfContentSpot : ContentDetector {
        
        public static long RTF = unchecked ((long)0x720F7A018B8FF1D5);

        public RtfContentSpot()
            : base(
                new ContentInfo[] {
                                      new ContentInfo(
                                          "Rich Text Format",
                                          RTF,
                                          "rtf",
                                          "application/rtf", 
                                          CompressionType.bZip2,
                                          new Magic[] {new Magic(Encoding.ASCII.GetBytes(@"{\rtf"), 0)}
                                          )
                                  }
                ) {}
    }

    public class RtfStreamContentIo : StreamContentIo {
        public RtfStreamContentIo () : base(new RtfContentSpot()) {
            this.IoMode = Common.IoMode.ReadWrite;
        }
    }

    public class RtfContentDigger : ContentDigger {

        private static RtfContentSpot _spot = new RtfContentSpot ();

        public RtfContentDigger () : base () { this.DiggUse = Digg; }

        protected virtual Content<Stream> Digg (Content<Stream> source, Content<Stream> sink) {
            if (!_spot.Supports (source.ContentType))
                return sink;
            var buffer = source.Data.GetBuffer ();

            // rtf must not be a unicode stream, convert it:
            if (TextHelper.IsUnicode (buffer)) {
                var s = Encoding.Unicode.GetString (buffer);
                sink.Data = s.AsAsciiStream ();
            }
            return sink;
        }
    }
}