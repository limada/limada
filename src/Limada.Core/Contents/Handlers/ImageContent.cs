/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2014 Lytico
 *
 * http://www.limada.org
 * 
 */


using Limaki.Common;
using System.Text;
using Limaki.Contents;
using System.IO;

namespace Limaki.Contents.IO {

    public class ImageContentSpot : ContentDetector {

        public static long PNG = unchecked ((long)0x8256F278CBBA0AE3);
        public static long TIF = unchecked ((long)0x4EB7076141A00A0D);
        public static long JPG = unchecked ((long)0x296FB808C4559626);

        public static long GIF = unchecked ((long)0x66825CADB2730E3C);
        public static long BMP = unchecked ((long)0xF7D9A1343B792E0E);
        public static long DIB = unchecked ((long)0xa389c5274651f7fa);

        public ImageContentSpot (): base(

            // place prefered formats first
                new ContentInfo[]{
                                    new ContentInfo(
                                         "PNG image",
                                         PNG,
                                         "png",
                                         "image/png",
                                         CompressionType.neverCompress,
                                         new Magic[]{
                                                        new Magic(new byte[]{0x89,0x50,0x4e,0x47},0)
                                                    }),
                                    
                                     new ContentInfo(
                                         "Tagged Image File Format",
                                         TIF,
                                         "tif",
                                         "image/tiff",
                                         CompressionType.bZip2,
                                         new Magic[]{
                                                        new Magic(new byte[]{0x49,0x49,0x2A,0x00},0),
                                                        new Magic(new byte[]{0x4D,0x4D,0x00,0x2A},0)
                                                    }),

                                     new ContentInfo(
                                         "GIF image",
                                         GIF,
                                         "gif",
                                         "image/gif",
                                         CompressionType.neverCompress,
                                         new Magic[]{
                                                        new Magic(Encoding.ASCII.GetBytes(@"GIF"),0)
                                                    }),

                                    new ContentInfo(
                                         "Device Independent Bitmap",
                                         DIB,
                                         "dib",
                                         "DeviceIndependentBitmap", // TODO:look for dib mime, or register with MimeFingerPrints
                                         CompressionType.bZip2,
                                         null),
                                         
			                       new ContentInfo(
                                         "Bitmap",
                                         BMP,
                                         "bmp",
                                         "image/bmp", // TODO:look for bmp mime, or register with MimeFingerPrints
                                         CompressionType.bZip2,
                                         new Magic[]{
                                                        new Magic(new byte[]{ 0x42, 0x4D},0)
                                                    }),
                                new ContentInfo(
                                         "JPEG image",
                                         JPG,
                                         "jpg",
                                         "image/jpeg",
                                         CompressionType.neverCompress,
                                         new Magic[]{
                                                        new Magic(new byte[]{0xff,0xd8},0),
                                                        new Magic(ByteUtils.BytesOfArray(new int[]{377,330,377}),0)
                                                    }),

                                 }
                ) { }


        public override ContentInfo Use (Stream source, ContentInfo sink) {
            return base.Use (source, sink);
        }
        
    }

    public class ImageStreamContentIo : StreamContentIo {
        public ImageStreamContentIo() : base(new ImageContentSpot()) {
            this.IoMode = IoMode.ReadWrite;
        }
    }
}

