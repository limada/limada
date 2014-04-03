// 
// GdiFontBackendHandler.cs
//  
// Author:
//       Lytico 
// 
// Copyright (c) 2012 Lytico (http://www.limada.org)
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

using Font = System.Drawing.Font;
using Xwt.Backends;
using Xwt.Drawing;
using Xwt.GdiBackend;

namespace Xwt.GdiBackend {

    public class GdiFontBackendHandler : FontBackendHandler {

        public override object GetSystemDefaultFont () {
            return System.Drawing.SystemFonts.DefaultFont;
        }

        public override System.Collections.Generic.IEnumerable<string> GetInstalledFonts () {
            foreach (var family in System.Drawing.FontFamily.Families) {
                bool usable = true;
                try {
                    if (!family.IsStyleAvailable((System.Drawing.FontStyle)0xF)) {
                        usable = false;
                    }

                } catch {
                    usable = false;
                }
                if (usable)
                    yield return family.Name;

            }
        }

        public override object Create (string fontName, double size, FontStyle style, FontWeight weight, FontStretch stretch) {
            return new Font(fontName, (float) size, style.ToGdi(weight), System.Drawing.GraphicsUnit.Point);
        }


        public override object Copy (object handle) {
            Font d = (Font)handle;
            return d.Clone();
        }


        public override object SetSize (object handle, double size) {
            var d = (Font)handle;
            if (d.Size != (int)size) {
                d = new Font(d.FontFamily, (float)size, d.Style);
            }
            return d;
        }

        public override object SetFamily (object handle, string family) {
            var d = (Font)handle;
            if (d.FontFamily.Name != family) {
                d = new Font(family, d.Size, d.Style);
            }
            return d;
        }

        public override object SetStyle (object handle, FontStyle style) {
            var d = (Font)handle;
            var oldStyle = GdiConverter.ToXwt (d.Style);
            var w = GdiConverter.ToXwtWeight(d.Style);

            if (oldStyle != style) {
                d = new Font(d.FontFamily, d.Size, GdiConverter.ToGdi(style, w));
            }
            return d;

        }

        public override object SetWeight (object handle, FontWeight weight) {
            var d = (Font)handle;
            var oldW = GdiConverter.ToXwtWeight(d.Style);
            var s = GdiConverter.ToXwt (d.Style);

            if (oldW != weight) {
                d = new Font(d.FontFamily, d.Size, GdiConverter.ToGdi(s, weight));
            }
            return d;
        }

        public override object SetStretch (object handle, FontStretch stretch) {
           
            return handle;
        }

        public override double GetSize (object handle) {
            var d = (Font)handle;
            return d.SizeInPoints;
        }

        public override string GetFamily (object handle) {
            var d = (Font)handle;
            return d.FontFamily.Name;
        }

        public override FontStyle GetStyle (object handle) {
            var d = (Font)handle;
            return GdiConverter.ToXwt (d.Style);
        }

        public override FontWeight GetWeight (object handle) {
            var d = (Font)handle;
            return GdiConverter.ToXwtWeight(d.Style);
        }

        public override FontStretch GetStretch (object handle) {
            return FontStretch.Normal;
        }




       
    }
}


