using System.IO;
using System.Text;
using Limaki.Common.Text;
using Limaki.Model.Content;
using System.Linq;
using System.Text.RegularExpressions;
using Limaki.Common;

namespace Limaki.Contents.IO {

    public class TextContentSpot : ContentDetector {
        public TextContentSpot ()
            : base(
                new ContentInfo[] {
                                      new ContentInfo(
                                          "Text",
                                          ContentTypes.Text,
                                          "txt",
                                          "text/plain",
                                          CompressionType.bZip2,
                                          null),
                                      new ContentInfo(
                                          "ASCII",
                                          ContentTypes.ASCII,
                                          "txt",
                                          "text/plain",
                                          CompressionType.bZip2,
                                          null),
                                  }
                ) { }

        public override ContentInfo Use (Stream source) {
            ContentInfo result = null;

            var buffer = ByteUtils.GetBuffer (source, (int)source.Length);
            var isUnicode = TextHelper.IsUnicode(buffer); 
            if (isUnicode)
               return ContentSpecs.First(t => t.ContentType == ContentTypes.Text);

            return null;

        }
    }

    public class TextStreamContentIo : StreamContentIo {
        public TextStreamContentIo ()
            : base(new TextContentSpot()) {
            this.IoMode = IO.IoMode.ReadWrite;
        }
    }

    public class TextContentDigger : ContentDigger {

        private static TextContentSpot _spot = new TextContentSpot();

        public TextContentDigger ()
            : base() { this.DiggUse = Digg; }

        protected virtual Content<Stream> Digg (Content<Stream> source, Content<Stream> sink) {
            if (!_spot.Supports(source.ContentType))
                return sink;
            var buffer = ByteUtils.GetBuffer(source.Data, 2048);
            var s = (TextHelper.IsUnicode(buffer) ? Encoding.Unicode.GetString(buffer) : Encoding.ASCII.GetString(buffer));

            // find lines
            var rx = new Regex("\r\n|\n|\r|\n|\f");
            var matches = rx.Matches(s);

            // extract first line
            if (matches.Count > 0) {
                sink.Description = s.Substring(0, matches[0].Index);
            } else {
                // TODO: if there is only one line,don't use a sink stream!!
                sink.Description = s;
                sink.Data = null;
            }
            return sink;
        }
    }
}
