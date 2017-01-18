namespace Limaki.Contents.Text {

    public interface IDocument {

        void Add (string text, RtfSectionStyle style, int cursX, bool newline);

    }
}