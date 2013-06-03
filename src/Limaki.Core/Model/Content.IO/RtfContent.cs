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

using System.Text;

namespace Limaki.Model.Content.IO {

    public class RtfContentInfo : ContentInfoSink {
        public RtfContentInfo()
            : base(
                new ContentInfo[] {
                                      new ContentInfo(
                                          "Rich Text Format",
                                          ContentTypes.RTF,
                                          "rtf",
                                          "application/rtf",
                                          CompressionType.bZip2,
                                          new Magic[] {new Magic(Encoding.ASCII.GetBytes(@"{\rtf"), 0)}
                                          )
                                  }
                ) {}
    }

    public class RtfContentStreamIo : ContentStreamSinkIo {
        public RtfContentStreamIo () : base(new RtfContentInfo()) { }
    }

   
}