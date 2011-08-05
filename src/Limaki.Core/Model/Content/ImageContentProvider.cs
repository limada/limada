/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
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
    public class ImageContentProvider : ContentProvider {
        StreamTypeInfo[] _supportedStreamTypes =
            new StreamTypeInfo[]{
					new StreamTypeInfo(
			            "JPEG image",
	                    StreamTypes.JPG,
	                    "jpg",
	                    "image/jpeg",
	                    CompressionType.neverCompress,
				 		new Magic[]{
							new Magic(new byte[]{0xff,0xd8},0),
							new Magic(ByteUtils.BytesOfArray(new int[]{377,330,377}),0)
						}),
                      new StreamTypeInfo(
			            "Tagged Image File Format",
	                    StreamTypes.TIF,
	                    "tif",
	                    "image/tiff",
	                    CompressionType.bZip2,
				 		new Magic[]{
							new Magic(new byte[]{0x49,0x49,0x2A,0x00},0),
                            new Magic(new byte[]{0x4D,0x4D,0x00,0x2A},0)
						}),
                      new StreamTypeInfo(
			            "GIF image",
	                    StreamTypes.GIF,
	                    "gif",
	                    "image/gif",
	                    CompressionType.neverCompress,
				 		new Magic[]{
							new Magic(Encoding.ASCII.GetBytes(@"GIF"),0)
						}),
                     new StreamTypeInfo(
			            "PNG image",
	                    StreamTypes.PNG,
	                    "png",
	                    "image/png",
	                    CompressionType.neverCompress,
				 		new Magic[]{
							new Magic(new byte[]{0x89,0x50,0x4e,0x47},0)
						}),
			
		};


        public override IEnumerable<StreamTypeInfo> SupportedStreamTypes {
            get { return _supportedStreamTypes; }
        }


        public override Content<Stream> Open(Stream stream) {
            if (!Supports(stream))
                return null;
            var result = new Content<Stream>(
                                stream,
                                _supportedStreamTypes[0].Compression,
                                _supportedStreamTypes[0].StreamType);

            return result;
        }

        public override bool Saveable { get { return true; } }
        public override bool Readable { get { return true; } }
    }
}
