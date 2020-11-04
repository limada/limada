// 
// GdiImageBackendHandler.cs
//  
// Author:
//       Lytico 
// 
// Copyright (c) 2012 Lytico (www.limada.org)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Xwt.Backends;
using SD=System.Drawing;
using XD = Xwt.Drawing;
using System.Drawing.Drawing2D;
using System;
using System.Collections.Generic;
using System.IO;
using Xwt.Drawing;

namespace Xwt.GdiBackend {

    public class GdiImageBackendHandler : ImageBackendHandler {

        public override object LoadFromStream (System.IO.Stream stream) {
            return new GdiImage(SD.Image.FromStream(stream));
        }

        public override void SaveToStream (object backend, Stream stream, XD.ImageFileType fileType) {
            var image = (GdiImage)backend;
            image.SaveToStream(stream, fileType);
        }

        public override Size GetSize (object handle) {
            var image = (GdiImage)handle;
            return new Size(image.Width, image.Height);
        }

        public virtual object Resize(object handle, double width, double height) {
            var image = (GdiImage)handle;
            return image.Resize(width,height);
        }

        public override object CopyBitmap (object handle) {
            var image = (GdiImage)handle;
            return image.CopyBitmap();
        }

        public override void CopyBitmapArea (object srcHandle, int srcX, int srcY, int width, int height, object destHandle, int destX, int destY) {
            var source = (GdiImage)srcHandle;
            var dest = (GdiImage)destHandle;
            source.CopyBitmapArea(srcX, srcY, width, height, dest, destX, destY);
        }

        public override object CropBitmap (object handle, int srcX, int srcY, int width, int height) {
            var image = (GdiImage)handle;
            return image.CropBitmap(srcX, srcY, width, height);
        }

        public override object CreateCustomDrawn (ImageDrawCallback drawCallback) {
            return new GdiImage(drawCallback);
        }

        public override bool IsBitmap (object handle) {
            var image = (GdiImage)handle;
            return image.IsBitmap;
        }

        // public override ImageFormat GetFormat (object handle) {
        //     var image = (GdiImage)handle;
        //     if (!image.IsBitmap)
        //         return ImageFormat.Other;
        //     return ((SD.Bitmap)image.Image).PixelFormat.ToXwt();
        // }

        public override object ConvertToBitmap (ImageDescription idesc, double scaleFactor, XD.ImageFormat format) {
            var image = (GdiImage)idesc.Backend;
            return image.ConvertToBitmap(idesc.Size.Width, idesc.Size.Height, scaleFactor, format);
        }

        public override bool HasMultipleSizes (object handle) {
            var image = (GdiImage)handle;
            return image.HasMultipleSizes;
        }

        public override void SetBitmapPixel (object handle, int x, int y, XD.Color color) {
            var image = (GdiImage)handle;
            image.SetBitmapPixel(x,y,color);
        }

        public override XD.Color GetBitmapPixel (object handle, int x, int y) {
            var image = (GdiImage)handle;
            return image.GetBitmapPixel(x,y);
        }

        /// <summary>
        /// Creates an image with multiple representations in different sizes
        /// </summary>
        /// <returns>The image backend</returns>
        /// <param name="images">Backends of the different image representations</param>
        /// <remarks>The first image of the list if the reference image, the one with scale factor = 1</remarks>
        public override object CreateMultiResolutionImage (IEnumerable<object> images) {
            throw new NotSupportedException();
        }

        public override object CreateMultiSizeIcon (IEnumerable<object> images) {
            throw new NotSupportedException();
        }


        public override Drawing.Image GetStockIcon (string id) {
            throw new NotImplementedException();
        }

        public override Size GetSize (string file) {
            throw new NotImplementedException ();
        }
    }
}