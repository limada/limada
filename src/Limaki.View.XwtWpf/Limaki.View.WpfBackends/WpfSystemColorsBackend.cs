// 
// WpfSystemColorsBackend.cs
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

namespace Limaki.Drawing.WpfBackend {

    public class WpfSystemColorsBackend : SystemColorsBackend {
        public override Color ScrollBar { get { return System.Windows.SystemColors.ScrollBarColor.ToXwt (); } }
        public override Color Background { get { return System.Windows.SystemColors.ControlColor.ToXwt (); } }
        public override Color ActiveCaption { get { return System.Windows.SystemColors.ActiveCaptionColor.ToXwt (); } }
        public override Color InactiveCaption { get { return System.Windows.SystemColors.InactiveCaptionColor.ToXwt (); } }
        public override Color Menu { get { return System.Windows.SystemColors.MenuColor.ToXwt (); } }
        public override Color Window { get { return System.Windows.SystemColors.WindowColor.ToXwt (); } }
        public override Color WindowFrame { get { return System.Windows.SystemColors.WindowFrameColor.ToXwt (); } }
        public override Color MenuText { get { return System.Windows.SystemColors.MenuTextColor.ToXwt (); } }
        public override Color WindowText { get { return System.Windows.SystemColors.WindowTextColor.ToXwt (); } }
        public override Color CaptionText { get { return System.Windows.SystemColors.ActiveCaptionTextColor.ToXwt (); } }
        public override Color ActiveBorder { get { return System.Windows.SystemColors.ActiveBorderColor.ToXwt (); } }
        public override Color InactiveBorder { get { return System.Windows.SystemColors.InactiveBorderColor.ToXwt (); } }
        public override Color ApplicationWorkspace { get { return System.Windows.SystemColors.WindowColor.ToXwt (); } }
        public override Color Highlight { get { return System.Windows.SystemColors.HighlightColor.ToXwt (); } }
        public override Color HighlightText { get { return System.Windows.SystemColors.HighlightTextColor.ToXwt (); } }
        public override Color ButtonFace { get { return System.Windows.SystemColors.ControlColor.ToXwt (); } }
        public override Color ButtonShadow { get { return System.Windows.SystemColors.ControlDarkColor.ToXwt (); } }
        public override Color GrayText { get { return System.Windows.SystemColors.GrayTextColor.ToXwt (); } }
        public override Color ButtonText { get { return System.Windows.SystemColors.ControlTextColor.ToXwt (); } }
        public override Color InactiveCaptionText { get { return System.Windows.SystemColors.InactiveCaptionTextColor.ToXwt (); } }
        public override Color ButtonHighlight { get { return System.Windows.SystemColors.ControlLightColor.ToXwt (); } }
        public override Color TooltipText { get { return System.Windows.SystemColors.InfoTextColor.ToXwt (); } }
        public override Color TooltipBackground { get { return System.Windows.SystemColors.InfoColor.ToXwt (); } }
        public override Color MenuHighlight { get { return System.Windows.SystemColors.MenuHighlightColor.ToXwt (); } }
        public override Color MenuBar { get { return System.Windows.SystemColors.MenuBarColor.ToXwt (); } }
    }
}