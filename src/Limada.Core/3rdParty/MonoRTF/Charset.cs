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

#if RTF_LIB
	public
#else
    internal
#endif
    class Charset {
        #region Public Constructors

        public Charset () {
            Flags = CharsetFlags.Read | CharsetFlags.Switch;
            _id = CharsetType.General;
            _file = string.Empty;
            ReadMap ();
        }

        #endregion	// Public Constructors

        #region Local Variables

        private CharsetType _id;
        private string _file;

        #endregion	// Local Variables

        #region Public Instance Properties

        public Charcode Code { get; set; }

        public CharsetFlags Flags { get; set; }

        public CharsetType ID {
            get => _id;
            set {
                switch (value) {
                    case CharsetType.Symbol: {
                        _id = CharsetType.Symbol;
                        return;
                    }

                    default:
                    case CharsetType.General: {
                        _id = CharsetType.General;
                        return;
                    }
                }
            }
        }

        public string File {
            get => _file;
            set {
                if (_file != value) _file = value;
            }
        }

        public StandardCharCode this [int c] => Code[c];

        #endregion	// Public Instance Properties

        #region Public Instance Methods

        public bool ReadMap () {
            switch (_id) {
                case CharsetType.General: {
                    if (_file == string.Empty) {
                        Code = Charcode.AnsiGeneric;
                        return true;
                    }

                    // FIXME - implement reading charmap from file...
                    return true;
                }

                case CharsetType.Symbol: {
                    if (_file == string.Empty) {
                        Code = Charcode.AnsiSymbol;
                        return true;
                    }

                    // FIXME - implement reading charmap from file...
                    return true;
                }

                default: {
                    return false;
                }
            }
        }

        public char StdCharCode (string name) {
            // FIXME - finish this
            return ' ';
        }

        public string StdCharName (char code) {
            // FIXME - finish this
            return string.Empty;
        }

        #endregion	// Public Instance Methods
    }

}