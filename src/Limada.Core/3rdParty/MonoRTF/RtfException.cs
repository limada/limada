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
using System.Text;

namespace Limaki.Common.Text.RTF.Parser {

    public class RtfException : ApplicationException {

        public RtfException (Rtf rtf, string errorMessage) {
            _pos = rtf.LinePos;
            _line = rtf.LineNumber;
            _tokenClass = rtf.TokenClass;
            _major = rtf.Major;
            _minor = rtf.Minor;
            _param = rtf.Param;
            _text = rtf.Text;
            this._errorMessage = errorMessage;
        }

        public override string Message {
            get {
                var sb = new StringBuilder ();
                sb.Append (_errorMessage);
                sb.Append ("\n");

                sb.Append ("RTF Stream Info: Pos:" + _pos + " Line:" + _line);
                sb.Append ("\n");

                sb.Append ("TokenClass:" + _tokenClass + ", ");
                sb.Append ("Major:" + $"{(int)_major}" + ", ");
                sb.Append ("Minor:" + $"{(int)_minor}" + ", ");
                sb.Append ("Param:" + $"{_param}" + ", ");
                sb.Append ("Text:" + _text);

                return sb.ToString ();
            }
        }
        private readonly int _pos;
        private readonly int _line;
        private readonly TokenClass _tokenClass;
        private readonly Major _major;
        private readonly Minor _minor;
        private readonly int _param;
        private readonly string _text;
        private readonly string _errorMessage;

    }

}