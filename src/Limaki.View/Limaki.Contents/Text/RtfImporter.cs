using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using Limaki.Common.Text.RTF.Parser;
using Xwt;
using XD = Xwt.Drawing;

namespace Limaki.Contents.Text {
    public class Line {
        public int indent;
        public LineEnding ending;
        public string Text;
    }

    public enum LineEnding {
        Rich,
        Wrap
    }

    public enum FormatSpecified {
        Color, Font
    }


    public class RtfImporter {
        IDocument document = null;
        Common.Text.RTF.Parser.RTF rtf = null;
        Stream stream = null;
        public RtfImporter (Stream stream, IDocument document) {
            this.stream = stream;
            this.rtf = new Common.Text.RTF.Parser.RTF (stream);
            this.document = document;
            _style = new RtfSectionStyle ();
        }

        private TextMap _textMap;
        private int _skipCount;
        private int _cursorX;
        private int _cursorY;
        private int _chars;
        private StringBuilder _line;
        private RtfSectionStyle _style;
        private Stack _sectionStack;

        public void Import () {
            int x = 0;
            int y = 0;
            int count = 0;
            InsertRtfFromStream (this.stream, 0, 0, out x, out y, out count);
        }

        private void InsertRtfFromStream (Stream data, int cursorX, int cursorY, out int toX, out int toY, out int chars) {

            // Prepare
            rtf.ClassCallback[TokenClass.Text] = HandleText;
            rtf.ClassCallback[TokenClass.Control] = HandleControl;
            rtf.ClassCallback[TokenClass.Group] = HandleGroup;

            _skipCount = 0;
            _line = new StringBuilder ();
            _style.Color = Xwt.Drawing.Colors.Black;
            _style.FontSize = 10;
            _style.Align = Alignment.Start;
            _style.FontStyleAtt = FontStyleAtt.Regular;
            _style.FontName = null;
            _style.Visible = true;
            _style.SkipWidth = 1;
            _cursorX = cursorX;
            _cursorY = cursorY;
            _chars = 0;
            rtf.DefaultFont ("Tahoma");

            _textMap = new TextMap ();
            TextMap.SetupStandardTable (_textMap.Table);

            try {
                rtf.Read ();	// That's it
                FlushText (rtf, false);

            } catch (RTFException e) {
#if DEBUG
                throw e;
#endif
                // Seems to be plain text or broken RTF
                Trace.WriteLine ("RTF Parsing failure: {0}", e.Message);
            }

            toX = _cursorX;
            toY = _cursorY;
            chars = _chars;

            // clear the section stack if it was used
            if (_sectionStack != null)
                _sectionStack.Clear ();

        }

        private void HandleText (Common.Text.RTF.Parser.RTF rtf) {
            string str = rtf.EncodedText;

            //todo - simplistically skips characters, should skip bytes?
            if (_skipCount > 0 && str.Length > 0) {
                int iToRemove = Math.Min (_skipCount, str.Length);

                str = str.Substring (iToRemove);
                _skipCount -= iToRemove;
            }

            /*
            if ((RTF.StandardCharCode)rtf.Minor != RTF.StandardCharCode.nothing) {
                rtf_line.Append(rtf_text_map[(RTF.StandardCharCode)rtf.Minor]);
            } else {
                if ((int)rtf.Major > 31 && (int)rtf.Major < 128) {
                    rtf_line.Append((char)rtf.Major);
                } else {
                    //rtf_line.Append((char)rtf.Major);
                    Console.Write("[Literal:0x{0:X2}]", (int)rtf.Major);
                }
            }
            */

            if (_style.Visible)
                _line.Append (str);
        }

        private void FlushText (Common.Text.RTF.Parser.RTF rtf, bool newline) {
            var length = _line.Length;
            if (!newline && (length == 0)) {
                document.Add (null, null, newline);
                return;
            }

            if (_style.FontName == null) {
                // First font in table is default
                _style.FontName = global::Limaki.Common.Text.RTF.Parser.Font.GetFont (rtf, 0).Name;
            }

            var font = XD.Font.FromName (_style.FontName + " " + _style.FontSize.ToString ());
                
            if (_style.Color == XD.Colors.Black) {
                // First color in table is default
                var color = Color.GetColor (rtf, 0);

                if ((color == null) || (color.Red == -1 && color.Green == -1 && color.Blue == -1)) {
                    _style.Color = SystemColors.WindowText;
                } else {
                    _style.Color = XD.Color.FromBytes ((byte) color.Red, (byte) color.Green, (byte) color.Blue);
                }

            }

            _chars += _line.Length;

            if (_cursorX == 0) {
                document.Add (_line.ToString (), _style, newline);
            }


            if (newline) {
                _cursorX = 0;
                _cursorY++;
            } else {
                _cursorX += length;
            }
            _line.Length = 0;	// Empty line
        }

        // To allow us to keep track of the sections and revert formatting
        // as we go in and out of sections of the document.
        private void HandleGroup (Common.Text.RTF.Parser.RTF rtf) {
            //start group - save the current formatting on to a stack
            //end group - go back to the formatting at the current group
            if (_sectionStack == null) {
                _sectionStack = new Stack ();
            }

            if (rtf.Major == Major.BeginGroup) {
                _sectionStack.Push (_style.Clone ());
                //spec specifies resetting unicode ignore at begin group as an attempt at error
                //recovery.
                _skipCount = 0;
            } else if (rtf.Major == Major.EndGroup) {
                if (_sectionStack.Count > 0) {
                    FlushText (rtf, false);

                    _style = (RtfSectionStyle) _sectionStack.Pop ();
                }
            }
        }

        private XD.Color ForeColor = SystemColors.WindowText;
        private int DpiX = 100;

        private void HandleControl (Common.Text.RTF.Parser.RTF rtf) {
            switch (rtf.Major) {
                case Major.Unicode: {
                        switch (rtf.Minor) {
                            case Minor.UnicodeCharBytes: {
                                    _style.SkipWidth = rtf.Param;
                                    break;
                                }

                            case Minor.UnicodeChar: {
                                    FlushText (rtf, false);
                                    _skipCount += _style.SkipWidth;
                                    _line.Append ((char) rtf.Param);
                                    break;
                                }
                        }
                        break;
                    }

                case Major.Destination: {
                        //					Console.Write("[Got Destination control {0}]", rtf.Minor);
                        rtf.SkipGroup ();
                        break;
                    }

                case Major.PictAttr:
                    if (rtf.Picture != null && rtf.Picture.IsValid ()) {
                        //var line = document.GetLine (_cursorY);
                        //document.InsertPicture (line, 0, rtf.Picture);
                        _cursorX++;

                        FlushText (rtf, true);
                        rtf.Picture = null;
                    }
                    break;

                case Major.CharAttr: {
                        switch (rtf.Minor) {
                            case Minor.ForeColor: {
                                    var color = Color.GetColor (rtf, rtf.Param);

                                    if (color != null) {
                                        FlushText (rtf, false);
                                        if (color.Red == -1 && color.Green == -1 && color.Blue == -1) {
                                            this._style.Color = ForeColor;
                                        } else {
                                            this._style.Color = XD.Color.FromBytes ((byte) color.Red, (byte) color.Green, (byte) color.Blue);
                                        }
                                        FlushText (rtf, false);
                                    }
                                    break;
                                }

                            case Minor.FontSize: {
                                    FlushText (rtf, false);
                                    this._style.FontSize = rtf.Param / 2;
                                    break;
                                }

                            case Minor.FontNum: {
                                    var font = Font.GetFont (rtf, rtf.Param);
                                    if (font != null) {
                                        FlushText (rtf, false);
                                        this._style.FontName = font.Name;
                                    }
                                    break;
                                }

                            case Minor.Plain: {
                                    FlushText (rtf, false);
                                    _style.FontStyleAtt = FontStyleAtt.Regular;
                                    break;
                                }

                            case Minor.Bold: {
                                    FlushText (rtf, false);
                                    if (rtf.Param == Common.Text.RTF.Parser.RTF.NoParam) {
                                        _style.FontStyleAtt |= FontStyleAtt.Bold;
                                    } else {
                                        _style.FontStyleAtt &= ~FontStyleAtt.Bold;
                                    }
                                    break;
                                }

                            case Minor.Italic: {
                                    FlushText (rtf, false);
                                    if (rtf.Param == Common.Text.RTF.Parser.RTF.NoParam) {
                                        _style.FontStyleAtt |= FontStyleAtt.Italic;
                                    } else {
                                        _style.FontStyleAtt &= ~FontStyleAtt.Italic;
                                    }
                                    break;
                                }

                            case Minor.StrikeThru: {
                                    FlushText (rtf, false);
                                    if (rtf.Param == Common.Text.RTF.Parser.RTF.NoParam) {
                                        _style.FontStyleAtt |= FontStyleAtt.Strikeout;
                                    } else {
                                        _style.FontStyleAtt &= ~FontStyleAtt.Strikeout;
                                    }
                                    break;
                                }

                            case Minor.Underline: {
                                    FlushText (rtf, false);
                                    if (rtf.Param == Common.Text.RTF.Parser.RTF.NoParam) {
                                        _style.FontStyleAtt |= FontStyleAtt.Underline;
                                    } else {
                                        _style.FontStyleAtt &= ~FontStyleAtt.Underline;
                                    }
                                    break;
                                }

                            case Minor.SubScrShrink: {
                                    FlushText (rtf, false);
                                    if (rtf.Param == Common.Text.RTF.Parser.RTF.NoParam) {
                                        _style.FontStyleAtt |= FontStyleAtt.Subscript;
                                    } else {
                                        _style.FontStyleAtt &= ~FontStyleAtt.Subscript;
                                    }
                                    break;
                                }

                            case Minor.SuperScrShrink: {
                                    FlushText (rtf, false);
                                    if (rtf.Param == Common.Text.RTF.Parser.RTF.NoParam) {
                                        _style.FontStyleAtt |= FontStyleAtt.Superscript;
                                    } else {
                                        _style.FontStyleAtt &= ~FontStyleAtt.Superscript;
                                    }
                                    break;
                                }

                            case Minor.Invisible: {
                                    FlushText (rtf, false);
                                    _style.Visible = false;
                                    break;
                                }

                            case Minor.NoUnderline: {
                                    FlushText (rtf, false);
                                    _style.FontStyleAtt &= ~FontStyleAtt.Underline;
                                    break;
                                }
                           
                        }
                        break;
                    }

                case Major.ParAttr: {
                        switch (rtf.Minor) {

                            case Minor.ParDef:
                                FlushText (rtf, false);
                                _style.ParLineLeftIndent = 0;
                                _style.Align = Alignment.Start;
                                break;

                            case Minor.LeftIndent:
                                _style.ParLineLeftIndent =
                                    (int) (((float) rtf.Param / 1440.0F) * DpiX + 0.5F);
                                break;

                            case Minor.QuadCenter:
                                FlushText (rtf, false);
                                _style.Align = Alignment.Center;
                                break;

                            case Minor.QuadJust:
                                FlushText (rtf, false);
                                _style.Align = Alignment.Center;
                                break;

                            case Minor.QuadLeft:
                                FlushText (rtf, false);
                                _style.Align = Alignment.Start;
                                break;

                            case Minor.QuadRight:
                                FlushText (rtf, false);
                                _style.Align = Alignment.End;
                                break;
                        }
                        break;
                    }

                case Major.SpecialChar: {
                        //Console.Write("[Got SpecialChar control {0}]", rtf.Minor);
                        SpecialChar (rtf);
                        break;
                    }
            }
        }

        private void SpecialChar (Common.Text.RTF.Parser.RTF rtf) {
            switch (rtf.Minor) {
                case Minor.Page:
                case Minor.Sect:
                case Minor.Row:
                case Minor.Line:
                case Minor.Par: {
                        FlushText (rtf, true);
                        break;
                    }

                case Minor.Cell: {
                        Trace.Write (" ");
                        break;
                    }

                case Minor.NoBrkSpace: {
                        Trace.Write (" ");
                        break;
                    }

                case Minor.Tab: {
                        _line.Append ("\t");
                        //					FlushText (rtf, false);
                        break;
                    }

                case Minor.NoReqHyphen:
                case Minor.NoBrkHyphen: {
                        _line.Append ("-");
                        //					FlushText (rtf, false);
                        break;
                    }

                case Minor.Bullet: {
                        Trace.WriteLine ("*");
                        break;
                    }

                case Minor.WidowCtrl:
                    break;

                case Minor.EmDash: {
                        _line.Append ("\u2014");
                        break;
                    }

                case Minor.EnDash: {
                        _line.Append ("\u2013");
                        break;
                    }
                /*
                                case RTF.Minor.LQuote: {
                                    Console.Write("\u2018");
                                    break;
                                }

                                case RTF.Minor.RQuote: {
                                    Console.Write("\u2019");
                                    break;
                                }

                                case RTF.Minor.LDblQuote: {
                                    Console.Write("\u201C");
                                    break;
                                }

                                case RTF.Minor.RDblQuote: {
                                    Console.Write("\u201D");
                                    break;
                                }
                */
                default: {
                        //					Console.WriteLine ("skipped special char:   {0}", rtf.Minor);
                        //					rtf.SkipGroup();
                        break;
                    }
            }
        }

    }

    public class RtfSectionStyle : ICloneable {
        public XD.Color Color;
        public string FontName;
        public int FontSize;
        public FontStyleAtt FontStyleAtt;
        public Xwt.Alignment Align;
        public int ParLineLeftIndent;
        public bool Visible;
        public int SkipWidth;

        public object Clone () {
            var newStyle = new RtfSectionStyle ();

            newStyle.Color = Color;
            newStyle.ParLineLeftIndent = ParLineLeftIndent;
            newStyle.Align = Align;
            newStyle.FontName = FontName;
            newStyle.FontSize = FontSize;
            newStyle.FontStyleAtt = FontStyleAtt;
            newStyle.Visible = Visible;
            newStyle.SkipWidth = SkipWidth;

            return newStyle;
        }
    }

    [Flags]
    public enum FontStyleAtt {
        Regular =0,
        Bold = 1,
        Italic = 2,
        Strikeout = 4,
        Underline = 8,
        Superscript = 16,
        Subscript = 32

    }

    public static class Missing {

    }
}