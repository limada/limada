using Limaki.Common;
using System.Collections.Generic;
using System.Text;

namespace Limaki.Contents.Text {

    [TODO]
    public class MarkdownDocument : IDocument {

        public string Body { get { return BodyText.ToString (); } }

        protected StringBuilder BodyText = new StringBuilder ();

        int inDiv = 0;
        [TODO]
        public virtual void Add (string text, RtfSectionStyle style, int startX, bool newLine) {

            if (text == null)
                return;

            var attrs = new List<string> ();
            if (style.SectionAttribute.HasFlag (SectionAttribute.Bold))
                attrs.Add ("**");
            if (style.SectionAttribute.HasFlag (SectionAttribute.Italic))
                attrs.Add ("_");
            if (style.SectionAttribute.HasFlag (SectionAttribute.Strikeout))
                attrs.Add ("~~");
            //if (style.SectionAttribute.HasFlag (SectionAttribute.Underline))
            //    attrs.Add ("u");
            //if (style.SectionAttribute.HasFlag (SectionAttribute.Superscript))
            //    attrs.Add ("sup");
            //if (style.SectionAttribute.HasFlag (SectionAttribute.Subscript))
            //    attrs.Add ("sub");

            foreach (var attr in attrs)
                BodyText.Append (string.Format ("{0}", attr));

            BodyText.Append (text);

            foreach (var attr in attrs)
                BodyText.Append (string.Format ("{0}", attr));

            BodyText.AppendLine ();

            if (newLine)
                BodyText.AppendLine ();
        }

    }
}