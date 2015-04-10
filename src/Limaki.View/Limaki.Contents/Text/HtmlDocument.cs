using System.Collections.Generic;
using System.Text;

namespace Limaki.Contents.Text {

    public class HtmlDocument : IDocument {

        public string Body { get { return BodyText.ToString (); } }

        protected StringBuilder BodyText = new StringBuilder ();

        int inDiv = 0;
        public virtual void Add (string text, RtfSectionStyle style, bool newLine) {
            var tagStyle = "";
            var tag = newLine ? "div" : "span";

            if (newLine && inDiv > 0) {
                inDiv--;
                // write end div
                // BodyText.Append (string.Format ("</{0}>", tag));
            }

            if (text == null)
                return;

            if (newLine) inDiv++;
 
            var attrs = new List<string> ();
            if (style.FontStyleAtt.HasFlag (FontStyleAtt.Bold))
                attrs.Add ("b");
            if (style.FontStyleAtt.HasFlag (FontStyleAtt.Italic))
                attrs.Add ("i");
            if (style.FontStyleAtt.HasFlag (FontStyleAtt.Underline))
                attrs.Add ("u");
            if (style.FontStyleAtt.HasFlag (FontStyleAtt.Strikeout))
                attrs.Add ("strike");
            if (style.FontStyleAtt.HasFlag (FontStyleAtt.Superscript))
                attrs.Add ("sup");
            if (style.FontStyleAtt.HasFlag (FontStyleAtt.Subscript))
                attrs.Add ("sub");

            BodyText.Append (string.Format ("<{0}{1}>", tag, tagStyle));

            foreach (var attr in attrs)
                BodyText.Append (string.Format ("<{0}>", attr));
            
            BodyText.Append (System.Net.WebUtility.HtmlEncode (text));

            foreach (var attr in attrs)
                BodyText.Append (string.Format ("</{0}>", attr));

            //if (!newLine)
                BodyText.Append (string.Format ("</{0}>", tag));

            if (newLine)
                BodyText.AppendLine ();
        }

    }
}