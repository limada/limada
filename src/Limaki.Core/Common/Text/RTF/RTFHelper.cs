using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Limaki.Common.Text.RTF.Parser;


namespace Limaki.Common.Text.RTF {
    [Flags]
    public enum FontStyle {
        Normal = 0,
        Bold = 1,
        Italic = 2,
        Underline = 4,
        Strikeout = 8,
        AllStyles = 0xF
    }

    public class RTFHelper : RTFFilter {
        public Stream SetToSameSize(Stream stream, int size) {
            stream.Position = 0;
            Parser.RTF rtf = new Parser.RTF(stream);

            var replaceList = new List<Record<int, int, byte[]>>();

            rtf.ClassCallback[TokenClass.Control] = (r) => {
                if (r.Major == Major.CharAttr && r.Minor == Minor.FontSize) {
                    var len = r.Param.ToString().Length;
                    replaceList.Add(new Record<int, int, byte[]>(
                                        r.CharPos - len, len, Encoding.ASCII.GetBytes(size.ToString())));
                }
            };


            rtf.Read();

            if (replaceList.Count > 0) {
                replaceList.Sort((a, b) => {
                    return a.One - b.One;
                });
                return Replace(stream, replaceList);
            } else {
                stream.Position = 0;
                return stream;
            }
        }

        public Stream SetToSameFont(Stream stream, string fontName) {

            stream.Position = 0;
            Parser.RTF rtf = new Parser.RTF(stream);
            var replaceList = new List<Record<int, int, byte[]>>();
            bool isInFontTbl = false;
            bool isInFontDef = false;

            bool firstFont = false;

            int fontNamePos = 0;

            int otherFontsStartPos = 0;
            int otherFontsEndPos = 0;

            StringBuilder fontNameB = null;

            int fontGroups = 0;

            rtf.DestinationCallback[Minor.FontTbl] = (r) => {
                isInFontTbl = true;
                isInFontDef = true;
                fontGroups = 1;
            };

            rtf.ClassCallback[TokenClass.Control] = (r) => {
                if (isInFontDef) {
                    if (r.Major == Major.CharAttr && r.Minor == Minor.FontNum && r.Param == 0) {
                        firstFont = true;
                        otherFontsStartPos = 0;
                    }
                } else if (otherFontsStartPos != 0) {
                    if (r.Major == Major.CharAttr && r.Minor == Minor.FontNum) {
                        var len = r.Param.ToString().Length;
                        replaceList.Add(new Record<int, int, byte[]>(
                                            r.CharPos - len,
                                            len, // the ";"
                                            Encoding.ASCII.GetBytes("0")
                                            ));
                    }
                }
            };

            rtf.ClassCallback[TokenClass.Group] = (r) => {
                if (isInFontTbl) {
                    if (r.Major == Major.BeginGroup) {
                        fontGroups++;
                        isInFontDef = fontGroups == 2;
                        if (isInFontDef && otherFontsStartPos == 0) {
                            otherFontsStartPos = r.CharPos;
                        }

                    }
                    if (r.Major == Major.EndGroup) {
                        fontGroups--;
                        isInFontDef = fontGroups == 2;
                        isInFontTbl = fontGroups > 0;
                        if (firstFont && fontGroups == 1) {
                            fontNameB.Remove(fontNameB.Length - 1, 1);
                            if (fontNameB.ToString() != fontName) {
                                replaceList.Add(new Record<int, int, byte[]>(
                                                    fontNamePos,
                                                    fontNameB.Length, // the ";"
                                                    Encoding.ASCII.GetBytes(fontName)
                                                    ));
                                firstFont = false;
                            }
                        } else if (fontGroups == 1) {
                            otherFontsEndPos = r.CharPos;
                        } else {
                            if (fontGroups == 0 && otherFontsStartPos != 0) {
                                replaceList.Add(new Record<int, int, byte[]>(
                                                    otherFontsStartPos,
                                                    otherFontsEndPos - otherFontsStartPos + 1,
                                                    null
                                                    ));
                            }
                        }

                    }
                }
            };


            rtf.ClassCallback[TokenClass.Text] = (r) => {
                if (isInFontDef && firstFont) {
                    if (fontNameB == null) {
                        fontNamePos = r.CharPos;
                        fontNameB = new StringBuilder();
                    }
                    fontNameB.Append(r.EncodedText);
                }
            };

            rtf.Read();

            if (replaceList.Count > 0) {
                replaceList.Sort((a, b) => {
                    return a.One - b.One;
                });
                return Replace(stream, replaceList);
            } else {
                stream.Position = 0;
                return stream;
            }

            return stream;

        }

        public Stream SetAttributes(Stream stream, FontStyle style) {
            stream.Position = 0;
            Parser.RTF rtf = new Parser.RTF(stream);

            var replaceList = new List<Record<int, int, byte[]>>();

            // the parser is in the text (not in a paragraph definition)
            bool textRun = false;

            // the style gathered by CharAttrs
            FontStyle runningStyle = default(FontStyle);

            // the style gathered in the first paragaph
            FontStyle givenStyle = default(FontStyle);

            // the style to add to the paragraph definition
            // and to delete in the text run
            FontStyle paraStyle = default(FontStyle);

            FontStyle styleToApply = default(FontStyle);


            int parDefPos = 0;
            int stylePos = 0;
            bool firstPara = true;

            Action<FontStyle, Parser.RTF> remove = (fstyle, r) => {
                if (textRun && (paraStyle & fstyle) != 0) {
                    var len = r.Text.Length;
                    replaceList.Add(new Record<int, int, byte[]>(
                                        r.CharPos - len, len, null));
                }

                if (fstyle == style) {
                    stylePos = r.CharPos;
                }
            };

            rtf.ClassCallback[TokenClass.Control] = (r) => {
                if (r.Major == Major.ParAttr) {
                    if (r.Minor == Minor.ParDef) {
                        parDefPos = r.CharPos;
                    }
                    textRun = false;
                }

                if (r.Major == Major.CharAttr) {
                    bool replaceIt = false;
                    bool removeIt = false;
                    string replace = null;
                    if (r.Minor == Minor.Bold) {
                        if (r.Param == 0) { // bold end
                            runningStyle &= FontStyle.AllStyles ^ FontStyle.Bold;
                        } else {
                            runningStyle |= FontStyle.Bold;
                        }
                        remove(FontStyle.Bold, r);
                    }

                    if (r.Minor == Minor.Italic) {
                        if (r.Param == 0) { // Italic end
                            runningStyle &= FontStyle.AllStyles ^ FontStyle.Italic;
                        } else {
                            runningStyle |= FontStyle.Italic;
                        }
                        remove(FontStyle.Italic, r);
                    }
                    if (r.Minor == Minor.Underline) {
                        runningStyle |= FontStyle.Underline;
                        remove(FontStyle.Underline, r);
                    }

                    if (r.Minor == Minor.NoUnderline) {
                        runningStyle &= FontStyle.AllStyles ^ FontStyle.Underline;
                        remove(FontStyle.Underline, r);
                    }

                    if (r.Minor == Minor.Plain) { // what to do with this??

                    }

                    if (replaceIt || removeIt) {
                        var len = r.Param.ToString().Length;
                        byte[] bReplace = null;
                        if (replace != null)
                            bReplace = Encoding.ASCII.GetBytes(replace);
                        replaceList.Add(new Record<int, int, byte[]>(
                                            r.CharPos - len, len, bReplace));
                    }
                }
            };

            int groupLevel = 0;
            rtf.ClassCallback[TokenClass.Group] = (r) => {
                if (r.Major == Major.BeginGroup) {
                    groupLevel++;
                }
                if (r.Major == Major.EndGroup) {
                    if (r.Minor == Minor.Par) // paragraph end??
                        ;
                    groupLevel--;
                }
            };

            rtf.ClassCallback[TokenClass.Text] = (r) => {
                if (!textRun) {
                    if (firstPara) {
                        givenStyle = runningStyle;
                        firstPara = false;
                    }

                    styleToApply = runningStyle ^ style;
                    paraStyle = styleToApply ^ runningStyle;



                    string rtfStyle = "";
                    if (style == FontStyle.Italic) {
                        rtfStyle = @"\i";
                    }
                    if (style == FontStyle.Bold) {
                        rtfStyle = @"\b";
                    }
                    if (style == FontStyle.Underline) {
                        rtfStyle = @"\ul";
                    }

                    if ((runningStyle & style) == 0) {
                        // the style has to be added:
                        replaceList.Add(new Record<int, int, byte[]>(
                                            parDefPos, 0, Encoding.ASCII.GetBytes(rtfStyle)
                                            ));
                    } else {
                        // toogle; the style has to be removed from paragraph
                        var len = rtfStyle.Length;
                        replaceList.Add(new Record<int, int, byte[]>(
                                            stylePos - len, len, null
                                            ));
                    }


                    runningStyle = default(FontStyle);

                }
                textRun = true;
            };

            rtf.Read();

            if (replaceList.Count > 0) {
                replaceList.Sort((a, b) => {
                    return a.One - b.One;
                });
                return Replace(stream, replaceList);
            } else {
                stream.Position = 0;
                return stream;
            }
        }

    }
}