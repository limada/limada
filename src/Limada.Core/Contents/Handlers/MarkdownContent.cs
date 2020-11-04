using System.Text;

namespace Limaki.Contents.IO {

    public class MarkdownContentSpot : ContentDetector {

        public MarkdownContentSpot ()
            : base (
                new ContentInfo[] {
                                      new ContentInfo(
                                          "Markdown",
                                          Markdown,
                                          "md",
                                          // https://tools.ietf.org/html/draft-ietf-appsawg-text-markdown-06
                                          "text/markdown",
                                          CompressionType.bZip2
                                          )
                                  }
                ) { }

        public static long Markdown = unchecked ((long)0x2B59900431E21A);
    }

    public class MarkdownStreamContentIo : StreamContentIo {
        public MarkdownStreamContentIo ()
            : base (new MarkdownContentSpot ()) {
            this.IoMode = Common.IoMode.ReadWrite;
        }
    }

}