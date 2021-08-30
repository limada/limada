// 
// SystemColorsBackend.cs
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

using Xwt;
using Xwt.Drawing;

namespace Xwt.Backends {

    public abstract class SystemColorsBackend:XwtObject {
        public abstract Color ScrollBar { get; }
        public abstract Color Background { get; }
        public abstract Color ActiveCaption { get; }
        public abstract Color InactiveCaption { get; }
        public abstract Color Menu { get; }
        public abstract Color Window { get; }
        public abstract Color WindowFrame { get; }
        public abstract Color MenuText { get; }
        public abstract Color WindowText { get; }
        public abstract Color CaptionText { get; }
        public abstract Color ActiveBorder { get; }
        public abstract Color InactiveBorder { get; }
        public abstract Color ApplicationWorkspace { get; }
        public abstract Color Highlight { get; }
        public abstract Color HighlightText { get; }
        public abstract Color ButtonFace { get; }
        public abstract Color ButtonShadow { get; }
        public abstract Color GrayText { get; }
        public abstract Color ButtonText { get; }
        public abstract Color InactiveCaptionText { get; }
        public abstract Color ButtonHighlight { get; }
        public abstract Color TooltipText { get; }
        public abstract Color TooltipBackground { get; }
        public abstract Color MenuHighlight { get; }
        public abstract Color MenuBar { get; }
    }
}
