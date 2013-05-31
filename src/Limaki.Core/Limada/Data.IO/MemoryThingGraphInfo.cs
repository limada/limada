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

    public class MemoryThingGraphInfo : ContentInfoSink {
        public MemoryThingGraphInfo ()
            : base(
                new ContentInfo[] {
                                  new ContentInfo(
                                      "Limada Things (Memory)",
                                      MemoryThingGraphContentType,
                                      "limemory",
                                      "application/limemory",
                                      CompressionType.None
                                      )
                              }
                ) { }

        public static long MemoryThingGraphContentType = unchecked((long)0xdf8779b0cf7e7cfc);
    }
}