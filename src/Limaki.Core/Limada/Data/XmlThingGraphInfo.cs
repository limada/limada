/*
 * Limada 
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

using Limaki.Model.Content;
using Limaki.Model.Content.IO;

namespace Limada.Data {
    public class XmlThingGraphInfo : ContentInfoSink {
        public XmlThingGraphInfo () : base(
            new ContentInfo[] {
                                  new ContentInfo(
                                      "Limada Things (XML)",
                                      ContentType,
                                      "limml",
                                      "application/limml",
                                      CompressionType.None
                                      )
                              }
            ) { }

        public static long ContentType = unchecked((long) 0x471cd142f3fc2634);
    }
}