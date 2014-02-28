/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing.Imaging;
using System.IO;
using Limaki.Common;

namespace Limaki.Contents.IO {

    public class ImageContentDigger : ContentDigger {

        private static ImageContentSpot _spot = new ImageContentSpot ();

        public ImageContentDigger () : base () {
            this.DiggUse = Digg;
        }

        protected virtual Content<Stream> Digg (Content<Stream> source, Content<Stream> sink) {
            if (!_spot.Supports (source.ContentType))
                return sink;
            if (source.ContentType == ContentTypes.DIB) {

                var bmp = new BitmapFromDibStream (source.Data);
                var sinkStream = bmp.Clone (bmp.Length);
                sink.Data = sinkStream;
                sink.ContentType = ContentTypes.BMP;
            }

            return sink;
        }
    }
}