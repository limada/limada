// 
// SystemColorsBackend.cs
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

using Xwt.Drawing;
using Xwt.Backends;

namespace Limaki.Drawing.WPF {
    public class SystemColorsBackend : ISystemColorsBackend {
        public Color ScrollBar { get { return System.Windows.SystemColors.ScrollBarColor.ToXwt (); } }
        public Color Background { get { return System.Windows.SystemColors.ControlColor.ToXwt (); } }
        public Color ActiveCaption { get { return System.Windows.SystemColors.ActiveCaptionColor.ToXwt (); } }
        public Color InactiveCaption { get { return System.Windows.SystemColors.InactiveCaptionColor.ToXwt (); } }
        public Color Menu { get { return System.Windows.SystemColors.MenuColor.ToXwt (); } }
        public Color Window { get { return System.Windows.SystemColors.WindowColor.ToXwt (); } }
        public Color WindowFrame { get { return System.Windows.SystemColors.WindowFrameColor.ToXwt (); } }
        public Color MenuText { get { return System.Windows.SystemColors.MenuTextColor.ToXwt (); } }
        public Color WindowText { get { return System.Windows.SystemColors.WindowTextColor.ToXwt (); } }
        public Color CaptionText { get { return System.Windows.SystemColors.ActiveCaptionTextColor.ToXwt (); } }
        public Color ActiveBorder { get { return System.Windows.SystemColors.ActiveBorderColor.ToXwt (); } }
        public Color InactiveBorder { get { return System.Windows.SystemColors.InactiveBorderColor.ToXwt (); } }
        public Color ApplicationWorkspace { get { return System.Windows.SystemColors.WindowColor.ToXwt (); } }
        public Color Highlight { get { return System.Windows.SystemColors.HighlightColor.ToXwt (); } }
        public Color HighlightText { get { return System.Windows.SystemColors.HighlightTextColor.ToXwt (); } }
        public Color ButtonFace { get { return System.Windows.SystemColors.ControlColor.ToXwt (); } }
        public Color ButtonShadow { get { return System.Windows.SystemColors.ControlDarkColor.ToXwt (); } }
        public Color GrayText { get { return System.Windows.SystemColors.GrayTextColor.ToXwt (); } }
        public Color ButtonText { get { return System.Windows.SystemColors.ControlTextColor.ToXwt (); } }
        public Color InactiveCaptionText { get { return System.Windows.SystemColors.InactiveCaptionTextColor.ToXwt (); } }
        public Color ButtonHighlight { get { return System.Windows.SystemColors.ControlLightColor.ToXwt (); } }
        public Color TooltipText { get { return System.Windows.SystemColors.InfoTextColor.ToXwt (); } }
        public Color TooltipBackground { get { return System.Windows.SystemColors.InfoColor.ToXwt (); } }
        public Color MenuHighlight { get { return System.Windows.SystemColors.MenuHighlightColor.ToXwt (); } }
        public Color MenuBar { get { return System.Windows.SystemColors.MenuBarColor.ToXwt (); } }
    }
}