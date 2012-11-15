using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Limaki.Common.Text.RTF.Parser;

namespace Limaki.Common.Text.RTF {

    public class AdobeRTFFilter : RTFFilter {


        public bool IsAdobeRTF(Stream stream) {
            string info = GetDoccom (stream);
            return Regex.Matches(info, "aldus | adobe", RegexOptions.IgnoreCase).Count > 0;
        }

        public Stream RemoveAdobeParagraphTags(Stream stream) {
            stream.Position = 0;
            Parser.RTF rtf = new Parser.RTF(stream);

            bool newPara = false;
            int adobeTagPos = -1;
            int tcTagPos = -1;
            int groupStartPos = -1;
            var removeList = new List<Pair<int, int>>();

            rtf.ClassCallback[TokenClass.Control] = (r) => {
                if (rtf.Minor == Minor.ParDef) {
                    newPara = true;
                }
                if (rtf.Text == @"\tc") {
                    tcTagPos = r.CharPos;
                }
                adobeTagPos = -1;
            };

            rtf.ClassCallback[TokenClass.Group] = (r) => {
                adobeTagPos = -1;
                char s = r.Text[0];
                if (r.Major == Major.BeginGroup) {
                    groupStartPos = r.CharPos;
                } else if (r.Major == Major.EndGroup) {
                    if (tcTagPos > 0) {
                        removeList.Add(new Pair<int, int>(groupStartPos, tcTagPos - 1));
                        removeList.Add(new Pair<int, int>(r.CharPos, r.CharPos));
                        tcTagPos = -1;
                    }

                }
            };

            rtf.ClassCallback[TokenClass.Text] = (r) => {
                char s = r.Text[0];
                if (newPara && s == '<') {
                    adobeTagPos = rtf.CharPos;
                } else if (adobeTagPos > 0 && s == '>') {
                    removeList.Add(new Pair<int, int>(adobeTagPos, rtf.CharPos));
                    adobeTagPos = -1;
                }
                newPara = false;
            };

            rtf.Read();
            if (removeList.Count > 0) {
                removeList.Sort((a, b) => {
                    return a.One - b.One;
                });
                return Remove(stream, removeList);
            } else {
                stream.Position = 0;
                return stream;
            }
        }
    }
}