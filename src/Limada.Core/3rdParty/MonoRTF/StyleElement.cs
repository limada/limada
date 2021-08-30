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

    public class StyleElement {

        public StyleElement (Style s, TokenClass tokenClass, Major major, Minor minor, int param, string text) {
            TokenClass = tokenClass;
            Major = major;
            Minor = minor;
            Param = param;
            Text = text;

            lock (s) {
                if (s.Elements == null) {
                    s.Elements = this;
                } else {
                    var se = s.Elements;
                    while (se._next != null)
                        se = se._next;
                    se._next = this;
                }
            }
        }

        private StyleElement _next;

        public TokenClass TokenClass { get; set; }

        public Major Major { get; set; }

        public Minor Minor { get; set; }

        public int Param { get; set; }

        public string Text { get; set; }


    }

}