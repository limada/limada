/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */


using System.Text;

namespace Limaki.Model.Content.IO {

    public class PdfContentInfo : ContentInfoSink {
        public PdfContentInfo(): base(
            new ContentInfo[] {
                                  new ContentInfo(
                                      "Portable Document Format",
                                      PdfContentType,
                                      "pdf",
                                      "application/pdf",
                                      CompressionType.None,
                                      new Magic[] {new Magic(Encoding.ASCII.GetBytes(@"%PDF-"), 0)}
                                      )
                              }
            ) {}

        public static long PdfContentType = unchecked((long) 0x90b88c3977443860);
    }

    public class PdfContentInStream : ContentInStreamSink {
        public PdfContentInStream () : base(new PdfContentInfo()) { }
    }

    public class PdfContentOutStream : ContentOutStreamSink {
        public PdfContentOutStream () : base(new PdfContentInfo()) { }
    }
}