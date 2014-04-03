// 
// GdiImage.cs
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


using System;
using SD = System.Drawing;
using XD = Xwt.Drawing;
using System.Drawing.Imaging;
using Xwt.Backends;
using System.IO;

namespace Xwt.GdiBackend {
    /// <summary>
    /// this is the backend for GdiImageBackendHandler
    /// </summary>
    public class GdiImage : IGdiGraphicsBackend, IDisposable {

        public GdiImage(int width, int height, XD.ImageFormat format) {
            this.Image = new SD.Bitmap(width, height, format.ToGdi());
        }

        public GdiImage (ImageDrawCallback drawCallback) {
            this._drawCallback = drawCallback;
        }

        public GdiImage (SD.Image image) {
            this.Image = image;
        }

        public SD.Image Image {
            get {
                return _image;
            }
            protected set { _image = value; }
        }

        SD.Image _image = null;
        SD.Graphics _graphics = null;
        ImageDrawCallback _drawCallback = null;

        public SD.Graphics Graphics {
            get {
                if (Image == null)
                    return null;
                if(_graphics==null) {
                    _graphics = SD.Graphics.FromImage(Image);
                    _graphics.SetQuality(GdiConverter.PaintHighQuality);
                }
                return _graphics;
            }
        }

        protected void Dispose(bool disposing) {
            // this makes Images unusable outside:
            //if (_graphics != null) {
            //    _graphics.Dispose();
            //    
            //}
           
            //if (Image != null)
            //    Image.Dispose();
            _graphics = null;
            Image = null;
        }

        public void Dispose() {
            Dispose(true);
        }

        ~GdiImage() {
            Dispose(false);
        }


        public double Width { get { return Image == null ? 0 : Image.Width; } }

        public double Height { get { return Image == null ? 0 : Image.Height; } }

        internal void SaveToStream (Stream stream, XD.ImageFileType fileType) {
            Image.Save(stream, fileType.ToGdi());
        }

        public object Resize (double width, double height) {
            var source = Image;
            SD.Image result = new SD.Bitmap(source, (int)width, (int)height);
            return new GdiImage(result);

            // try if this has better quality:
            result = new SD.Bitmap((int)width, (int)height, source.PixelFormat);

            using (var g = SD.Graphics.FromImage(result)) {
                g.SetQuality(GdiConverter.CopyHighQuality);
                g.DrawImage(source, 0, 0, source.Width, source.Height);
                g.Flush();
            }
            return new GdiImage(result);
        }

        public object CopyBitmap () {
            var source = this.Image;
            SD.Image result = new SD.Bitmap(source, source.Width, source.Height);
            return new GdiImage(result);
        }

        public void CopyBitmapArea (int srcX, int srcY, int width, int height, GdiImage destImage, int destX, int destY) {
            var source = this.Image;
            var dest = destImage.Image;

            using (var g = SD.Graphics.FromImage(dest)) {
                g.SetQuality(GdiConverter.CopyHighQuality);
                var sr = new SD.Rectangle(srcX, srcY, width, height);
                var dr = new SD.Rectangle(destX, destY, width, height);
                g.DrawImage(source, dr, sr, SD.GraphicsUnit.Pixel);
                g.Flush();
            }
        }

        public object CropBitmap (int srcX, int srcY, int width, int height) {
            throw new NotImplementedException();
            // TODO: this is not correct:
            var source = this.Image;
            SD.Image result = new SD.Bitmap(source, source.Width, source.Height);

            using (var g = SD.Graphics.FromImage(result)) {
                g.SetQuality(GdiConverter.CopyHighQuality);
                var sr = new SD.Rectangle(srcX, srcY, width, height);
                var dr = new SD.Rectangle(0, 0, width, height);
                g.DrawImage(source, dr, sr, SD.GraphicsUnit.Pixel);
                g.Flush();
            }
            return new GdiImage(result);
        }

        public bool IsBitmap { get { return _image == null ? false : Image is SD.Bitmap; } }

        public object ConvertToBitmap (double width, double height, double scaleFactor, XD.ImageFormat format) {
            if (_drawCallback !=null) {
                var image = new SD.Bitmap((int)width, (int)height, format.ToGdi());
                using (var g = SD.Graphics.FromImage(image)) {
                    g.SetQuality(GdiConverter.PaintHighQuality);
                    g.ScaleTransform((float) scaleFactor, (float) scaleFactor);
                    Draw(g, 0, 0, width,height);
                    g.Flush();
                }
                return new GdiImage(image);
            }

            throw new NotSupportedException();
        }

        public bool HasMultipleSizes { get { return false; } }

        public void SetBitmapPixel (int x, int y, Drawing.Color color) {
            throw new NotImplementedException();
        }

        public Drawing.Color GetBitmapPixel (int x, int y) {
            throw new NotImplementedException();
        }

        public void Draw (SD.Graphics g, double x, double y, double width, double height) {
            if (_drawCallback != null) {
                var c = new GdiContext(g);
                _drawCallback(c, new Rectangle(x, y, width, height));
            }
        }
    }
}