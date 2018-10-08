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

// COMPLETE

#undef RTF_DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Limaki.Common.Text.RTF.Parser {

    public class Rtf {
        #region	Local Variables

        public const char EOF = unchecked((char)-1);
        public const int NoParam = -1000000;

        private Encoding _encoding;
        private int _encodingCodePage = 1252;
        private StringBuilder _textBuffer;

        private char _pushedChar;

        //private StringBuilder	pushed_text_buffer;
        private TokenClass _pushedClass;
        private Major _pushedMajor;
        private Minor _pushedMinor;
        private int _pushedParam;

        private char _prevChar;
        private bool _bumpLine;

        private Font _fontList;

        private Charset _curCharset;
        private Stack _charsetStack;

        private StreamReader _source;

        private static readonly Dictionary<string, KeyStruct> KeyTable = new Dictionary<string, KeyStruct> ();
        private static readonly KeyStruct[] Keys = KeysInit.Init ();

        #endregion	// Local Variables

        #region Constructors

        static Rtf () {
            foreach (var key in Keys) {
                KeyTable[key.Symbol] = key;
            }
        }

        public Rtf (Stream stream) {
            _source = new StreamReader (stream);

            _textBuffer = new StringBuilder (1024);
            //pushed_text_buffer = new StringBuilder(1024);

            TokenClass = TokenClass.None;
            _pushedClass = TokenClass.None;
            _pushedChar = unchecked((char)-1);

            LineNumber = 0;
            LinePos = 0;
            _prevChar = unchecked((char)-1);
            _bumpLine = false;
            _fontList = null;
            _charsetStack = null;

            _curCharset = new Charset ();

            DestinationCallback = new DestinationCallback ();
            ClassCallback = new ClassCallback ();

            DestinationCallback[Minor.OptDest] = HandleOptDest;
            DestinationCallback[Minor.FontTbl] = ReadFontTbl;
            DestinationCallback[Minor.ColorTbl] = ReadColorTbl;
            DestinationCallback[Minor.StyleSheet] = ReadStyleSheet;
            DestinationCallback[Minor.Info] = ReadInfoGroup;
            DestinationCallback[Minor.Pict] = ReadPictGroup;
            DestinationCallback[Minor.Object] = ReadObjGroup;
        }

        #endregion	// Constructors

        #region Properties

        public TokenClass TokenClass { get; set; }
        public Major Major { get; set; }
        public Minor Minor { get; set; }
        public int Param { get; set; }

        public string Text {
            get => _textBuffer.ToString ();
            set {
                if (value == null) {
                    _textBuffer.Length = 0;
                } else {
                    _textBuffer = new StringBuilder (value);
                }
            }
        }

        public string EncodedText { get; private set; }
        public Picture Picture { get; set; }
        public Color Colors { get; set; }
        public Font Fonts { get; set; }
        public Style Styles { get; set; }

        public int LinePos { get; protected set; }
        public int LineNumber { get; protected set; }

        // lytico: added CharPos
        public int CharPos { get; protected set; } = 0;

        public DestinationCallback DestinationCallback { get; set; }
        public ClassCallback ClassCallback { get; set; }

        #endregion	// Properties

        #region Methods

        /// <summary>Set the default font for documents without font table</summary>
        public void DefaultFont (string name) {
            var font = new Font (this) {
                Num = 0,
                Name = name
            };
        }

        /// <summary>Read the next character from the input - skip any crlf</summary>
        private char GetChar () => GetChar (true);


        /// <summary>Read the next character from the input</summary>
        private char GetChar (bool skipCrLf) {
            int c;
            bool oldBumpLine;

            SkipCRLF:
            if ((c = _source.Read ()) != -1) {
                _textBuffer.Append ((char)c);
                CharPos++;
            }

            if (_prevChar == EOF) {
                _bumpLine = true;
            }

            oldBumpLine = _bumpLine;
            _bumpLine = false;

            if (skipCrLf) {
                if (c == '\r') {
                    _bumpLine = true;
                    _textBuffer.Length--;
                    goto SkipCRLF;
                }

                if (c == '\n') {
                    _bumpLine = true;
                    if (_prevChar == '\r') {
                        oldBumpLine = false;
                    }

                    _textBuffer.Length--;
                    goto SkipCRLF;
                }
            }

            LinePos++;
            if (oldBumpLine) {
                LineNumber++;
                LinePos = 1;
            }

            _prevChar = (char)c;
            return (char)c;
        }

        /// <summary>Parse the RTF stream</summary>
        public void Read () {
            while (GetToken () != TokenClass.EOF) {
                RouteToken ();
            }
        }

        /// <summary>Route a token</summary>
        public void RouteToken () {
            if (CheckCM (TokenClass.Control, Major.Destination)) {
                var d = DestinationCallback[Minor];
                d?.Invoke (this);
            }

            // Invoke class callback if there is one

            var c = ClassCallback[TokenClass];
            c?.Invoke (this);
        }

        /// <summary>Skip to the end of the next group to start or current group we are in</summary>
        public void SkipGroup () {
            var level = 1;

            while (GetToken () != TokenClass.EOF) {
                if (TokenClass != TokenClass.Group) continue;
                if (Major == Major.BeginGroup) {
                    level++;
                } else if (Major == Major.EndGroup) {
                    level--;
                    if (level < 1) {
                        break;
                    }
                }
            }
        }

        /// <summary>Return the next token in the stream</summary>
        public TokenClass GetToken () {
            if (_pushedClass != TokenClass.None) {
                TokenClass = _pushedClass;
                Major = _pushedMajor;
                Minor = _pushedMinor;
                Param = _pushedParam;
                _pushedClass = TokenClass.None;
                return TokenClass;
            }

            GetToken2 ();

            if (TokenClass == TokenClass.Text) {
                Minor = (Minor)_curCharset[(int)Major];
                if (_encoding == null) {
                    _encoding = Encoding.GetEncoding (_encodingCodePage);
                }

                EncodedText = new String (_encoding.GetChars (new byte[] {(byte)Major}));
            }

            if (_curCharset.Flags == CharsetFlags.None) {
                return TokenClass;
            }

            if (CheckCMM (TokenClass.Control, Major.Unicode, Minor.UnicodeAnsiCodepage)) {
                if (Param == NoParam)
                    _encodingCodePage = 1252;
                else {
                    if (Param > 0 && Param < 65535)
                        _encodingCodePage = Param;
                    else
                        _encodingCodePage = 1252;
                }
            }

            if (((_curCharset.Flags & CharsetFlags.Read) != 0) && CheckCM (TokenClass.Control, Major.CharSet)) {
                _curCharset.ReadMap ();
            } else if (((_curCharset.Flags & CharsetFlags.Switch) != 0) && CheckCMM (TokenClass.Control, Major.CharAttr, Minor.FontNum)) {
                
                var fp = Font.GetFont (_fontList, Param);

                if (fp != null) {
                    _curCharset.ID = fp.Name.StartsWith ("Symbol") ? CharsetType.Symbol : CharsetType.General;
                    
                } else if (((_curCharset.Flags & CharsetFlags.Switch) != 0) && (TokenClass == TokenClass.Group)) {
                    if (Major == Major.BeginGroup)
                        _charsetStack.Push (_curCharset);
                    else if (Major == Major.EndGroup) {
                        _curCharset = (Charset)_charsetStack.Pop ();
                    }
                }
            }

            return TokenClass;
        }

        private void GetToken2 () {
            char c;

            TokenClass = TokenClass.Unknown;
            Param = NoParam;

            _textBuffer.Length = 0;

            if (_pushedChar != EOF) {
                c = _pushedChar;
                _textBuffer.Append (c);
                _pushedChar = EOF;
            } else if ((c = GetChar ()) == EOF) {
                TokenClass = TokenClass.EOF;
                return;
            }

            if (c == '{') {
                TokenClass = TokenClass.Group;
                Major = Major.BeginGroup;
                return;
            }

            if (c == '}') {
                TokenClass = TokenClass.Group;
                Major = Major.EndGroup;
                return;
            }

            if (c != '\\') {
                if (c != '\t') {
                    TokenClass = TokenClass.Text;
                    Major = (Major)c; // FIXME - typing?
                    return;
                }

                TokenClass = TokenClass.Control;
                Major = Major.SpecialChar;
                Minor = Minor.Tab;
                return;
            }

            if ((c = GetChar ()) == EOF) {
                // Not so good
                return;
            }

            if (!char.IsLetter (c)) {
                if (c == '\'') {
                    char c2;

                    if ((c = GetChar ()) == EOF) {
                        return;
                    }

                    if ((c2 = GetChar ()) == EOF) {
                        return;
                    }

                    TokenClass = TokenClass.Text;
                    Major = (Major)((char)((Convert.ToByte (c.ToString (), 16) * 16 + Convert.ToByte (c2.ToString (), 16))));
                    return;
                }

                // Escaped char
                if (c == ':' || c == '{' || c == '}' || c == '\\') {
                    TokenClass = TokenClass.Text;
                    Major = (Major)c;
                    return;
                }

                Lookup (_textBuffer.ToString ());
                return;
            }

            while (char.IsLetter (c)) {
                if ((c = GetChar (false)) == EOF) {
                    break;
                }
            }

            if (c != EOF) {
                _textBuffer.Length--;
            }

            Lookup (_textBuffer.ToString ());

            if (c != EOF) {
                _textBuffer.Append (c);
            }

            var sign = 1;
            if (c == '-') {
                sign = -1;
                c = GetChar ();
            }

            if (c != EOF && char.IsDigit (c) && Minor != Minor.PngBlip) {
                Param = 0;
                while (char.IsDigit (c)) {
                    Param = Param * 10 + Convert.ToByte (c) - 48;
                    if ((c = GetChar ()) == EOF) {
                        break;
                    }
                }

                Param *= sign;
            }

            if (c == EOF) return;

            if (c != ' ' && c != '\r' && c != '\n') {
                _pushedChar = c;
            }

            _textBuffer.Length--;
        }

        public void SetToken (TokenClass cl, Major maj, Minor min, int par, string text) {
            TokenClass = cl;
            Major = maj;
            Minor = min;
            Param = par;
            _textBuffer = par == NoParam ? new StringBuilder (text) : new StringBuilder (text + par.ToString ());
        }

        public void UngetToken () {
            if (_pushedClass != TokenClass.None) {
                throw new RtfException (this, "Cannot unget more than one token");
            }

            if (TokenClass == TokenClass.None) {
                throw new RtfException (this, "No token to unget");
            }

            _pushedClass = TokenClass;
            _pushedMajor = Major;
            _pushedMinor = Minor;
            _pushedParam = Param;
            //pushed_text_buffer = new StringBuilder(text_buffer.ToString());
        }

        public TokenClass PeekToken () {
            GetToken ();
            UngetToken ();
            return TokenClass;
        }

        public void Lookup (string token) {
            if (!KeyTable.TryGetValue (token.Substring (1), out var key)) {
                TokenClass = TokenClass.Unknown;
                Major = (Major)(-1);
                Minor = (Minor)(-1);
                return;
            }

            TokenClass = TokenClass.Control;
            Major = key.Major;
            Minor = key.Minor;
        }

        public bool CheckCM (TokenClass rtf_class, Major major) => (TokenClass == rtf_class) && (Major == major);

        public bool CheckCMM (TokenClass rtf_class, Major major, Minor minor) => (TokenClass == rtf_class) && (Major == major) && (Minor == minor);

        public bool CheckMM (Major major, Minor minor) => (Major == major) && (Minor == minor);

        #endregion	// Methods

        #region Default Delegates

        private void HandleOptDest (Rtf rtf) {
            var groupLevels = 1;

            while (true) {
                GetToken ();

                // Here is where we should handle recognised optional
                // destinations.
                //
                // Handle a picture group 
                //
                if (rtf.CheckCMM (TokenClass.Control, Major.Destination, Minor.Pict)) {
                    ReadPictGroup (rtf);
                    return;
                }

                if (rtf.CheckCM (TokenClass.Group, Major.EndGroup)) {
                    if ((--groupLevels) == 0) {
                        break;
                    }
                }

                if (rtf.CheckCM (TokenClass.Group, Major.BeginGroup)) {
                    groupLevels++;
                }
            }
        }

        public void ReadFontTbl (Rtf rtf) {
            var old = -1;
            Font font = null;

            while (true) {
                rtf.GetToken ();

                if (rtf.CheckCM (TokenClass.Group, Major.EndGroup)) {
                    break;
                }

                if (old < 0) {
                    if (rtf.CheckCMM (TokenClass.Control, Major.CharAttr, Minor.FontNum)) {
                        old = 1;
                    } else if (rtf.CheckCM (TokenClass.Group, Major.BeginGroup)) {
                        old = 0;
                    } else {
                        throw new RtfException (rtf, "Cannot determine format");
                    }
                }

                if (old == 0) {
                    if (!rtf.CheckCM (TokenClass.Group, Major.BeginGroup)) {
                        throw new RtfException (rtf, "missing \"{\"");
                    }

                    rtf.GetToken ();
                }

                font = new Font (rtf);

                while ((rtf.TokenClass != TokenClass.EOF) && (!rtf.CheckCM (TokenClass.Text, (Major)';')) && (!rtf.CheckCM (TokenClass.Group, Major.EndGroup))) {
                    if (rtf.TokenClass == TokenClass.Control) {
                        switch (rtf.Major) {
                            case Major.FontFamily: {
                                font.Family = (int)rtf.Minor;
                                break;
                            }

                            case Major.CharAttr: {
                                switch (rtf.Minor) {
                                    case Minor.FontNum: {
                                        font.Num = rtf.Param;
                                        break;
                                    }

                                    default: {
#if RTF_DEBUG
                                        Console.WriteLine ("Got unhandled Control.CharAttr.Minor: " + rtf.Minor);
#endif
                                        break;
                                    }
                                }

                                break;
                            }

                            case Major.FontAttr: {
                                switch (rtf.Minor) {
                                    case Minor.FontCharSet: {
                                        font.Charset = (CharsetType)rtf.Param;
                                        break;
                                    }

                                    case Minor.FontPitch: {
                                        font.Pitch = rtf.Param;
                                        break;
                                    }

                                    case Minor.FontCodePage: {
                                        font.Codepage = rtf.Param;
                                        break;
                                    }

                                    case Minor.FTypeNil:
                                    case Minor.FTypeTrueType: {
                                        font.Type = rtf.Param;
                                        break;
                                    }
                                    default: {
#if RTF_DEBUG
                                        Console.WriteLine ("Got unhandled Control.FontAttr.Minor: " + rtf.Minor);
#endif
                                        break;
                                    }
                                }

                                break;
                            }

                            default: {
#if RTF_DEBUG
                                Console.WriteLine ("ReadFontTbl: Unknown Control token " + rtf.Major);
#endif
                                break;
                            }
                        }
                    } else if (rtf.CheckCM (TokenClass.Group, Major.BeginGroup)) {
                        rtf.SkipGroup ();
                    } else if (rtf.TokenClass == TokenClass.Text) {
                        var sb = new StringBuilder ();

                        while ((rtf.TokenClass != TokenClass.EOF) && (!rtf.CheckCM (TokenClass.Text, (Major)';')) && (!rtf.CheckCM (TokenClass.Group, Major.EndGroup)) && (!rtf.CheckCM (TokenClass.Group, Major.BeginGroup))) {
                            sb.Append ((char)rtf.Major);
                            rtf.GetToken ();
                        }

                        if (rtf.CheckCM (TokenClass.Group, Major.EndGroup)) {
                            rtf.UngetToken ();
                        }

                        font.Name = sb.ToString ();
                        continue;
#if RTF_DEBUG
                    } else {
                        Console.WriteLine ("ReadFontTbl: Unknown token " + rtf._textBuffer);
#endif
                    }

                    rtf.GetToken ();
                }

                if (old == 0) {
                    rtf.GetToken ();

                    if (!rtf.CheckCM (TokenClass.Group, Major.EndGroup)) {
                        throw new RtfException (rtf, "Missing \"}\"");
                    }
                }
            }

            if (font == null) {
                throw new RtfException (rtf, "No font created");
            }

            if (font.Num == -1) {
                throw new RtfException (rtf, "Missing font number");
            }

            rtf.RouteToken ();
        }

        private void ReadColorTbl (Rtf rtf) {
            var num = 0;

            while (true) {
                rtf.GetToken ();

                if (rtf.CheckCM (TokenClass.Group, Major.EndGroup)) {
                    break;
                }

                var color = new Color (rtf) {Num = num++};

                while (rtf.CheckCM (TokenClass.Control, Major.ColorName)) {
                    switch (rtf.Minor) {
                        case Minor.Red: {
                            color.Red = rtf.Param;
                            break;
                        }

                        case Minor.Green: {
                            color.Green = rtf.Param;
                            break;
                        }

                        case Minor.Blue: {
                            color.Blue = rtf.Param;
                            break;
                        }
                    }

                    rtf.GetToken ();
                }

                if (!rtf.CheckCM (TokenClass.Text, (Major)';')) {
                    throw new RtfException (rtf, "Malformed color entry");
                }
            }

            rtf.RouteToken ();
        }

        private void ReadStyleSheet (Rtf rtf) {
            var sb = new StringBuilder ();

            while (true) {
                rtf.GetToken ();

                if (rtf.CheckCM (TokenClass.Group, Major.EndGroup)) {
                    break;
                }

                var style = new Style (rtf);

                if (!rtf.CheckCM (TokenClass.Group, Major.BeginGroup)) {
                    throw new RtfException (rtf, "Missing \"{\"");
                }

                while (true) {
                    rtf.GetToken ();

                    if ((rtf.TokenClass == TokenClass.EOF) || rtf.CheckCM (TokenClass.Text, (Major)';')) {
                        break;
                    }

                    if (rtf.TokenClass == TokenClass.Control) {
                        if (rtf.CheckMM (Major.ParAttr, Minor.StyleNum)) {
                            style.Num = rtf.Param;
                            style.Type = StyleType.Paragraph;
                            continue;
                        }

                        if (rtf.CheckMM (Major.CharAttr, Minor.CharStyleNum)) {
                            style.Num = rtf.Param;
                            style.Type = StyleType.Character;
                            continue;
                        }

                        if (rtf.CheckMM (Major.StyleAttr, Minor.SectStyleNum)) {
                            style.Num = rtf.Param;
                            style.Type = StyleType.Section;
                            continue;
                        }

                        if (rtf.CheckMM (Major.StyleAttr, Minor.BasedOn)) {
                            style.BasedOn = rtf.Param;
                            continue;
                        }

                        if (rtf.CheckMM (Major.StyleAttr, Minor.Additive)) {
                            style.Additive = true;
                            continue;
                        }

                        if (rtf.CheckMM (Major.StyleAttr, Minor.Next)) {
                            style.NextPar = rtf.Param;
                            continue;
                        }

                        new StyleElement (style, rtf.TokenClass, rtf.Major, rtf.Minor, rtf.Param, rtf._textBuffer.ToString ());
                    } else if (rtf.CheckCM (TokenClass.Group, Major.BeginGroup)) {
                        // This passes over "{\*\keycode ... }, among other things
                        rtf.SkipGroup ();
                    } else if (rtf.TokenClass == TokenClass.Text) {
                        var name = new StringBuilder ();
                        while (rtf.TokenClass == TokenClass.Text) {
                            if (rtf.Major == (Major)';') {
                                rtf.UngetToken ();
                                break;
                            }

                            sb.Append ((char)rtf.Major);
                            name.Append ((char)rtf.Major);
                            rtf.GetToken ();
                        }

                        style.InternalName = sb.ToString ();
                        style.Name = name.ToString ();
#if RTF_DEBUG
                    } else {
                        Console.WriteLine ("ReadStyleSheet: Ignored token " + rtf._textBuffer);
#endif
                    }
                }

                rtf.GetToken ();

                if (!rtf.CheckCM (TokenClass.Group, Major.EndGroup)) {
                    throw new RtfException (rtf, "Missing EndGroup (\"}\"");
                }

                // Sanity checks
                if (style.InternalName == null) {
                    throw new RtfException (rtf, "Style must have name");
                }

                if (style.Num < 0) {
                    if (!sb.ToString ().StartsWith ("Normal") && !sb.ToString ().StartsWith ("Standard")) {
                        throw new RtfException (rtf, "Missing style number");
                    }

                    style.Num = Style.NormalStyleNum;
                }

                if (style.NextPar == -1) {
                    style.NextPar = style.Num;
                }
            }

            rtf.RouteToken ();
        }

        private void ReadInfoGroup (Rtf rtf) {
            rtf.SkipGroup ();
            rtf.RouteToken ();
        }

        private void ReadPictGroup (Rtf rtf) {
            var readImageData = false;

            var picture = new Picture ();
            while (true) {
                rtf.GetToken ();

                if (rtf.CheckCM (TokenClass.Group, Major.EndGroup))
                    break;

                switch (Minor) {
                    case Minor.PngBlip:
                        picture.ImageType = Minor;
                        readImageData = true;
                        break;
                    case Minor.WinMetafile:
                        picture.ImageType = Minor;
                        readImageData = true;
                        continue;
                    case Minor.PicWid:
                        continue;
                    case Minor.PicHt:
                        continue;
                    case Minor.PicGoalWid:
                        picture.SetWidthFromTwips (Param);
                        continue;
                    case Minor.PicGoalHt:
                        picture.SetHeightFromTwips (Param);
                        continue;
                }

                var hexDigit1 = (char)rtf.Major;
                if (readImageData && rtf.TokenClass == TokenClass.Text) {
                    picture.Data.Seek (0, SeekOrigin.Begin);

                    //char c = (char) rtf.Major;

                    while (true) {
                        while (hexDigit1 == '\n' || hexDigit1 == '\r') {
                            hexDigit1 = (char)_source.Peek ();
                            if (hexDigit1 == '}')
                                break;
                            hexDigit1 = (char)_source.Read ();
                        }

                        var hexDigit2 = (char)_source.Peek ();
                        if (hexDigit2 == '}')
                            break;
                        hexDigit2 = (char)_source.Read ();
                        while (hexDigit2 == '\n' || hexDigit2 == '\r') {
                            hexDigit2 = (char)_source.Peek ();
                            if (hexDigit2 == '}')
                                break;
                            hexDigit2 = (char)_source.Read ();
                        }

                        uint digitValue1;
                        if (char.IsDigit (hexDigit1))
                            digitValue1 = (uint)(hexDigit1 - '0');
                        else if (char.IsLower (hexDigit1))
                            digitValue1 = (uint)(hexDigit1 - 'a' + 10);
                        else if (char.IsUpper (hexDigit1))
                            digitValue1 = (uint)(hexDigit1 - 'A' + 10);
                        else if (hexDigit1 == '\n' || hexDigit1 == '\r')
                            continue;
                        else
                            break;

                        uint digitValue2;
                        if (char.IsDigit (hexDigit2))
                            digitValue2 = (uint)(hexDigit2 - '0');
                        else if (char.IsLower (hexDigit2))
                            digitValue2 = (uint)(hexDigit2 - 'a' + 10);
                        else if (char.IsUpper (hexDigit2))
                            digitValue2 = (uint)(hexDigit2 - 'A' + 10);
                        else if (hexDigit2 == '\n' || hexDigit2 == '\r')
                            continue;
                        else
                            break;

                        picture.Data.WriteByte ((byte)checked(digitValue1 * 16 + digitValue2));

                        // We get the first hex digit at the end, since in the very first
                        // iteration we use rtf.Major as the first hex digit
                        hexDigit1 = (char)_source.Peek ();
                        if (hexDigit1 == '}')
                            break;
                        hexDigit1 = (char)_source.Read ();
                    }


                    readImageData = false;
                    break;
                }
            }

            if (picture.ImageType != Minor.Undefined && !readImageData) {
                Picture = picture;
                SetToken (TokenClass.Control, Major.PictAttr, picture.ImageType, 0, String.Empty);
            }
        }

        private void ReadObjGroup (Rtf rtf) {
            rtf.SkipGroup ();
            rtf.RouteToken ();
        }

        #endregion	// Default Delegates
    }

}