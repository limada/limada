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

using System;

namespace Limaki.Common.Text.RTF.Parser {

    public class Style {
        #region Constructors

        public Style (Rtf rtf) {
            Num = -1;
            Type = StyleType.Paragraph;
            BasedOn = NoStyleNum;
            NextPar = -1;

            lock (rtf) {
                if (rtf.Styles == null) {
                    rtf.Styles = this;
                } else {
                    var s = rtf.Styles;
                    while (s.next != null)
                        s = s.next;
                    s.next = this;
                }
            }
        }

        #endregion	// Constructors

        #region Local Variables

        public const int NoStyleNum = 222;
        public const int NormalStyleNum = 0;

        private Style next;

        #endregion Local Variables

        #region Properties

        public string InternalName { get; set; }

        public string Name { get; set; }

        public StyleType Type { get; set; }

        public bool Additive { get; set; }

        public int BasedOn { get; set; }

        public StyleElement Elements { get; set; }

        public bool Expanding { get; set; }

        public int NextPar { get; set; }

        public int Num { get; set; }

        #endregion	// Properties

        #region Methods

        public void Expand (Rtf rtf) {
            if (Num == -1) return;

            if (Expanding) throw new Exception ("Recursive style expansion");
            Expanding = true;

            if (Num != BasedOn) {
                rtf.SetToken (TokenClass.Control, Major.ParAttr, Minor.StyleNum, BasedOn, "\\s");
                rtf.RouteToken ();
            }

            var se = Elements;
            while (se != null) {
                rtf.TokenClass = se.TokenClass;
                rtf.Major = se.Major;
                rtf.Minor = se.Minor;
                rtf.Param = se.Param;
                rtf.Text = se.Text;
                rtf.RouteToken ();
            }

            Expanding = false;
        }

        public static Style GetStyle (Rtf rtf, int styleNumber) {
            Style s;

            lock (rtf) {
                s = GetStyle (rtf.Styles, styleNumber);
            }

            return s;
        }

        public static Style GetStyle (Style start, int styleNumber) {
            Style s;

            if (styleNumber == -1) return start;

            s = start;

            while (s != null && s.Num != styleNumber) s = s.next;
            return s;
        }

        #endregion	// Methods
    }

}