using System.Collections.Generic;
using System.Text;
using System.Linq;
using XD = Xwt.Drawing;

namespace Limaki.Contents.Text {

    public class HtmlDocument : IDocument {

        public string Body { get { return BodyText.ToString (); } }

        protected StringBuilder BodyText = new StringBuilder ();

        int inDiv = 0;
        int lineLen = 0;
        public virtual void Add (string text, RtfSectionStyle style, int curX, bool newLine) {
            var tagStyle = "";
            var tag = "span"; //newLine ? "div" : "span";

            if (newLine && inDiv > 0) {
                inDiv--;
                // write end div
                // BodyText.Append (string.Format ("</{0}>", tag));
            }

            if (text == null)
                return;
            
            if (newLine) inDiv++;
 
            var styles = new List<string> ();
            if (style.Color != Xwt.SystemColors.WindowText) {
                styles.Add ($"color:{style.Color.ToHexString (false)}");
            }
            var trimmed = text.TrimStart();
            var padding = text.Length - trimmed.Length;
            if (padding > 0) {
                styles.Add ($"padding-left:{padding}em");
            }
            if (styles.Count > 0) {
                tagStyle = $"style=\"{string.Join(";",styles)}\"";
            }
            var attrs = new List<string> ();

            if (style.SectionAttribute.HasFlag (SectionAttribute.Bold))
                attrs.Add ("b");
            if (style.SectionAttribute.HasFlag (SectionAttribute.Italic))
                attrs.Add ("i");
            if (style.SectionAttribute.HasFlag (SectionAttribute.Underline))
                attrs.Add ("u");
            if (style.SectionAttribute.HasFlag (SectionAttribute.Strikeout))
                attrs.Add ("strike");
            if (style.SectionAttribute.HasFlag (SectionAttribute.Superscript))
                attrs.Add ("sup");
            if (style.SectionAttribute.HasFlag (SectionAttribute.Subscript))
                attrs.Add ("sub");

            BodyText.Append ($"<{tag} {tagStyle}>");

            foreach (var attr in attrs)
                BodyText.Append ($"<{attr}>");

            BodyText.Append (System.Net.WebUtility.HtmlEncode (trimmed));

            foreach (var attr in attrs)
                BodyText.Append ($"</{attr}>");

            //if (!newLine)
                BodyText.Append ($"</{tag}>");
            lineLen += text.Length;
            if (newLine) {
                lineLen = 0;
                BodyText.Append ("<br>");
                BodyText.AppendLine ();
            }
        }

    }

    
}