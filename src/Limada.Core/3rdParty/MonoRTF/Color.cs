// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Copyright (c) 2005 Novell, Inc. (http://www.novell.com)
//
// Authors:
//	Peter Bartok	(pbartok@novell.com)
//
//

// COMPLETE

namespace Limaki.Common.Text.RTF.Parser {

    public class Color {
        #region Constructors

        public Color (Rtf rtf) {
            Red = -1;
            Green = -1;
            Blue = -1;
            Num = -1;

            lock (rtf) {
                if (rtf.Colors == null) {
                    rtf.Colors = this;
                } else {
                    var c = rtf.Colors;
                    while (c._next != null)
                        c = c._next;
                    c._next = this;
                }
            }
        }

        #endregion	// Constructors

        #region	Local Variables

        private Color _next;

        #endregion	// Local Variables

        #region Properties

        public int Red { get; set; }

        public int Green { get; set; }

        public int Blue { get; set; }

        public int Num { get; set; }

        #endregion	// Properties

        #region Methods

        public static Color GetColor (Rtf rtf, int colorNumber) {
            Color c;

            lock (rtf) {
                c = GetColor (rtf.Colors, colorNumber);
            }

            return c;
        }

        public static Color GetColor (Color start, int colorNumber) {
            if (colorNumber == -1) return start;

            var c = start;

            while (c != null && c.Num != colorNumber) c = c._next;
            return c;
        }

        #endregion	// Methods
    }

}