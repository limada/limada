// 
// SystemColors.cs
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

using Xwt.Backends;
using Xwt.Engine;
namespace Xwt.Drawing {

    public static class SystemColors {

        static SystemColors() {
            backend = WidgetRegistry.MainRegistry.CreateBackend<ISystemColorsBackend>(typeof(SystemColors));
        }

        static ISystemColorsBackend backend;

        public static Color ScrollBar { get { return backend.ScrollBar; } }
        public static Color Background { get { return backend.Background; } }
        public static Color ActiveCaption { get { return backend.ActiveCaption; } }
        public static Color InactiveCaption { get { return backend.InactiveCaption; } }
        public static Color Menu { get { return backend.Menu; } }
        public static Color Window { get { return backend.Window; } }
        public static Color WindowFrame { get { return backend.WindowFrame; } }
        public static Color MenuText { get { return backend.MenuText; } }
        public static Color WindowText { get { return backend.WindowText; } }
        public static Color CaptionText { get { return backend.CaptionText; } }
        public static Color ActiveBorder { get { return backend.ActiveBorder; } }
        public static Color InactiveBorder { get { return backend.InactiveBorder; } }
        public static Color ApplicationWorkspace { get { return backend.ApplicationWorkspace; } }
        public static Color Highlight { get { return backend.Highlight; } }
        public static Color HighlightText { get { return backend.HighlightText; } }
        public static Color ButtonFace { get { return backend.ButtonFace; } }
        public static Color ButtonShadow { get { return backend.ButtonShadow; } }
        public static Color GrayText { get { return backend.GrayText; } }
        public static Color ButtonText { get { return backend.ButtonText; } }
        public static Color InactiveCaptionText { get { return backend.InactiveCaptionText; } }
        public static Color ButtonHighlight { get { return backend.ButtonHighlight; } }
        public static Color TooltipText { get { return backend.TooltipText; } }
        public static Color TooltipBackground { get { return backend.TooltipBackground; } }
        public static Color MenuHighlight { get { return backend.MenuHighlight; } }
        public static Color MenuBar { get { return backend.MenuBar; } }

    }
}

