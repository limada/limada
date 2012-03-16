// 
// Colors.cs
//  
// Author:
//       Lytico 
// 
// Copyright (c) 2012 Lytico (http://limada.sourceforge.net)
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

using Xwt.Drawing;

namespace Xwt.Backends {

    public interface ISystemColorsBackend {
        Color ScrollBar { get; }
        Color Background { get; }
        Color ActiveCaption { get; }
        Color InactiveCaption { get; }
        Color Menu { get; }
        Color Window { get; }
        Color WindowFrame { get; }
        Color MenuText { get; }
        Color WindowText { get; }
        Color CaptionText { get; }
        Color ActiveBorder { get; }
        Color InactiveBorder { get; }
        Color ApplicationWorkspace { get; }
        Color Highlight { get; }
        Color HighlightText { get; }
        Color ButtonFace { get; }
        Color ButtonShadow { get; }
        Color GrayText { get; }
        Color ButtonText { get; }
        Color InactiveCaptionText { get; }
        Color ButtonHighlight { get; }
        Color TooltipText { get; }
        Color TooltipBackground { get; }
        Color MenuHighlight { get; }
        Color MenuBar { get; }
    }

}