namespace Limaki.Contents.Text {

    public interface IDocument {

        void Add (string text, RtfSectionStyle style, bool newline);

    }
}