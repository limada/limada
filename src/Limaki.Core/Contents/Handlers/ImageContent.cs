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


using Limaki.Common;
using System.Text;
using Limaki.Model.Content;

namespace Limaki.Contents.IO {

    public class ImageContentSpot : ContentDetector {
        public ImageContentSpot (): base(

                new ContentInfo[]{
                                     new ContentInfo(
                                         "JPEG image",
                                         ContentTypes.JPG,
                                         "jpg",
                                         "image/jpeg",
                                         CompressionType.neverCompress,
                                         new Magic[]{
                                                        new Magic(new byte[]{0xff,0xd8},0),
                                                        new Magic(ByteUtils.BytesOfArray(new int[]{377,330,377}),0)
                                                    }),

                                     new ContentInfo(
                                         "Tagged Image File Format",
                                         ContentTypes.TIF,
                                         "tif",
                                         "image/tiff",
                                         CompressionType.bZip2,
                                         new Magic[]{
                                                        new Magic(new byte[]{0x49,0x49,0x2A,0x00},0),
                                                        new Magic(new byte[]{0x4D,0x4D,0x00,0x2A},0)
                                                    }),

                                     new ContentInfo(
                                         "GIF image",
                                         ContentTypes.GIF,
                                         "gif",
                                         "image/gif",
                                         CompressionType.neverCompress,
                                         new Magic[]{
                                                        new Magic(Encoding.ASCII.GetBytes(@"GIF"),0)
                                                    }),

                                     new ContentInfo(
                                         "PNG image",
                                         ContentTypes.PNG,
                                         "png",
                                         "image/png",
                                         CompressionType.neverCompress,
                                         new Magic[]{
                                                        new Magic(new byte[]{0x89,0x50,0x4e,0x47},0)
                                                    }),
			
                                 }
                ) { }

        
    }

    public class ImageStreamContentIo : StreamContentIo {
        public ImageStreamContentIo() : base(new ImageContentSpot()) {
            this.IoMode = IO.IoMode.ReadWrite;
        }
    }
}

