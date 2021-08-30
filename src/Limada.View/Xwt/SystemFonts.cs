// 
// SystemColors.cs
//  
// Author:
//       Lytico 
// 
// Copyright (c) 2012 - 2014 Lytico (www.limada.org)
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
using Xwt.Drawing;

namespace Xwt {

    public class SystemFonts {

        static SystemFonts _fonts = null;
        public static SystemFonts Fonts {
            get { return _fonts ?? (_fonts = new SystemFonts ()); }
        }

        protected SystemFonts () {
            backend = Toolkit.CurrentEngine.CreateBackendHandler<SystemFontBackend> ();
        }

        public static void Clear () {
            _fonts = null;
        }

        private SystemFontBackend backend;

        public Font CaptionFont { get { return backend.CaptionFont; } }
        public Font DefaultFont { get { return backend.DefaultFont; } }
        public Font DialogFont { get { return backend.DialogFont; } }
        public Font IconTitleFont { get { return backend.IconTitleFont; } }
        public Font MenuFont { get { return backend.MenuFont; } }
        public Font MessageBoxFont { get { return backend.MessageBoxFont; } }
        public Font SmallCaptionFont { get { return backend.SmallCaptionFont; } }
        public Font StatusFont { get { return backend.StatusFont; } }
    }
}