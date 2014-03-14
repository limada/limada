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


using Limaki.Model.Content;
using Limaki.Contents.IO;
using Limaki.Contents;

namespace Limada.IO {

    public class Db4oThingGraphSpot : ContentDetector {

        public Db4oThingGraphSpot ()
            : base(
                new ContentInfo[] {
                        new ContentInfo(
                            "Limada Things (db4o)",
                            Db4oThingGraphContentType,
                            "limo",
                            "application/limo",
                            CompressionType.None,
                            new Magic[]{
                                        new Magic(new byte[] {
                                            (byte) 'd',
                                            (byte) 'b',
                                            (byte) '4',
                                            (byte) 'o'
                                        },0)
                                    }
                            )
                    }
                ) { }

        public static long Db4oThingGraphContentType = unchecked((long) 0xf6399a943c1b2bf3);
    }
}