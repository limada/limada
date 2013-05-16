using System.Text;

namespace Limaki.Model.Content.IO {

    public class PdfContentInfo : ContentInfoSink {
        public PdfContentInfo(): base(
            new ContentInfo[] {
                                  new ContentInfo(
                                      "Portable Document Format",
                                      PdfStreamType,
                                      "pdf",
                                      "application/pdf",
                                      CompressionType.None,
                                      new Magic[] {new Magic(Encoding.ASCII.GetBytes(@"%PDF-"), 0)}
                                      )
                              }
            ) {}

        public static long PdfStreamType = unchecked((long) 0x90b88c3977443860);
    }
}