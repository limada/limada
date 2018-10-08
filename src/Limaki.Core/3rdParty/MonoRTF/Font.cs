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

    public
    class Font {
        #region Constructors

        public Font (Rtf rtf) {
            _rtf = rtf;
            _num = -1;
            Name = string.Empty;

            lock (rtf) {
                if (rtf.Fonts == null) {
                    rtf.Fonts = this;
                } else {
                    var f = rtf.Fonts;
                    while (f._next != null)
                        f = f._next;
                    f._next = this;
                }
            }
        }

        #endregion	// Constructors

        #region	Local Variables

        private int _num;
        private Font _next;
        private readonly Rtf _rtf;

        #endregion	// Local Variables

        #region Properties

        public string Name { get; set; }

        public string AltName { get; set; }

        public int Num {
            get => _num;
            set {
                // Whack any previous font with the same number
                DeleteFont (_rtf, value);
                _num = value;
            }
        }

        public int Family { get; set; }

        public CharsetType Charset { get; set; }

        public int Pitch { get; set; }

        public int Type { get; set; }

        public int Codepage { get; set; }

        #endregion	// Properties

        #region Methods

        public static bool DeleteFont (Rtf rtf, int fontNumber) {
            lock (rtf) {
                var f = rtf.Fonts;
                Font prev = null;
                while (f != null && f._num != fontNumber) {
                    prev = f;
                    f = f._next;
                }

                if (f != null) {
                    if (f == rtf.Fonts) {
                        rtf.Fonts = f._next;
                    } else {
                        if (prev != null)
                            prev._next = f._next;
                        else
                            rtf.Fonts = f._next;
                    }

                    return true;
                }
            }

            return false;
        }

        public static Font GetFont (Rtf rtf, int fontNumber) {
            Font f;

            lock (rtf) {
                f = GetFont (rtf.Fonts, fontNumber);
            }

            return f;
        }

        public static Font GetFont (Font start, int fontNumber) {
            if (fontNumber == -1) return start;

            var f = start;

            while (f != null && f._num != fontNumber) f = f._next;
            return f;
        }

        #endregion	// Methods
    }

}