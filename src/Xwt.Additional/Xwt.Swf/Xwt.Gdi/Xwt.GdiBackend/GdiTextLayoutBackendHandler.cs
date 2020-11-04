// 
// GdiTextLayoutBackendHandler.cs
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
using Xwt.Backends;
using Xwt.Drawing;

namespace Xwt.GdiBackend {

    public class GdiTextLayoutBackendHandler : TextLayoutBackendHandler {
        
        public override void SetWidth (object backend, double value) {
            var tl = (GdiTextLayoutBackend) backend;
            tl.Width = value;
        }

        public override void SetHeight (object backend, double value) {
            var tl = (GdiTextLayoutBackend) backend;
            tl.Heigth = value;
        }

        public override void SetTrimming (object backend, Drawing.TextTrimming value) {
            var tl = (GdiTextLayoutBackend) backend;
            tl.Trimming = value.ToGdi();
        }

        public override void SetWrapMode (object backend, WrapMode value) {
            var tl = (GdiTextLayoutBackend)backend;
            tl.WrapMode = value;
        }

        public override void SetText (object backend, string text) {
            var tl = (GdiTextLayoutBackend) backend;
            tl.Text = text;
        }

        public override void SetFont (object backend, Xwt.Drawing.Font font) {
            var tl = (GdiTextLayoutBackend) backend;
            tl.Font = font;
        }

        public override void SetAlignment (object backend, Alignment alignment)
        {
            var tl = (GdiTextLayoutBackend)backend;
            tl.Alignment = alignment.ToGdi ();
        }
        
        public override Size GetSize (object backend) {
            var tl = (GdiTextLayoutBackend) backend;
            return tl.Size;
        }

        private static System.Drawing.Graphics _deviceContext = null;
        public static System.Drawing.Graphics DeviceContext {
            get {
                
                if (_deviceContext == null) {
                    _deviceContext =
                        System.Drawing.Graphics.FromImage (
                        new System.Drawing.Bitmap (1000, 1000,
                            System.Drawing.Imaging.PixelFormat.Format32bppArgb));
                    GdiConverter.SetQuality (_deviceContext, GdiConverter.DrawTextHighQuality);
                }
                return _deviceContext;
            }
            set { _deviceContext = value; }
        }

        public override object Create () {
            var result = new GdiTextLayoutBackend () { WrapMode = WrapMode.Word };
            result.Context = new GdiContext (DeviceContext);
            return result;
        }
        
        public override object Create (Context context) {
            var result = Create () as GdiTextLayoutBackend;
            result.Context = context.GetBackend () as GdiContext;
            return result;
        }

        public override int GetIndexFromCoordinates (object backend, double x, double y) {
            throw new System.NotImplementedException();
        }

        public override Point GetCoordinateFromIndex (object backend, int index) {
            throw new System.NotImplementedException();
        }

        public override void AddAttribute (object backend, Drawing.TextAttribute attribute) {
            throw new System.NotImplementedException();
        }

        public override void ClearAttributes (object backend) {
            throw new System.NotImplementedException();
        }

        public override double GetBaseline(object backend)
        {
            var tl = (GdiTextLayoutBackend)backend;
            return tl.Baseline;
        }

        public override double GetMeanline(object backend)
        {
            var tl = (GdiTextLayoutBackend)backend;
            return tl.Meanline;
        }
    }
}
