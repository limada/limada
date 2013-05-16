using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Limaki.Model.Content.IO {

    public class HtmlContentInfo : ContentInfoSink {

        public HtmlContentInfo (): base (

                new ContentInfo[]{
                                 new ContentInfo(
                                     "HTML",
                                     ContentTypes.HTML,
                                     "html",
                                     "text/html",
                                     CompressionType.bZip2
                                     ),
                                 new ContentInfo(
                                     "XHTML",
                                     XHTML,
                                     "xhtml",
                                     "application/xhtml+xml",
                                     CompressionType.bZip2
                                     )
                             }
            ) {}

        public static long XHTML = 0x280efaf080c35e30;

        public override bool StreamHasMagics { 
            get { return true; }
        }

        public override ContentInfo Sink (Stream stream) {

            ContentInfo result = null;

            var oldPos = stream.Position;
            int buflen = Math.Min(256, (int)stream.Length);
            var buffer = new byte[buflen];

            stream.Read(buffer, 0, buflen);

            var s = Encoding.ASCII.GetString(buffer).ToLower();
            if (
                s.Contains("<!doctype html") ||
                s.Contains("<html") ||
                s.Contains("<head") ||
                s.Contains("<body")) {
                    result = SupportedContents.First(t => t.ContentType == ContentTypes.HTML);
            }

            if (
                s.Contains("<!doctype xhtml") ||
                s.Contains("<xhtml")
                ) {
                    result = SupportedContents.First(t => t.ContentType == XHTML);
            }

            stream.Position = oldPos;
            return result;
        }

    }

    public class HtmlContentInStream : ContentInStreamSink {
        public HtmlContentInStream (): base(new HtmlContentInfo()) {}
    }

    public class HtmlContentOutStream : ContentOutStreamSink {
        public HtmlContentOutStream () : base(new HtmlContentInfo()) { }
    }
}