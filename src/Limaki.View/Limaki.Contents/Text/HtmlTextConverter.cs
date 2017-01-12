using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Limaki.Common;

namespace Limaki.Contents.Text {

    public class HtmlTextConverter : ContentConverter<Stream>, IContentConverter<Stream> {

        public override IEnumerable<Tuple<long, long>> SupportedTypes { get { yield return Tuple.Create (ContentTypes.Text, ContentTypes.HTML); } }

        public override Content<Stream> Use (Content<Stream> source, Content<Stream> sink) {
            if (ProveTypes (source, ContentTypes.Text, sink, ContentTypes.HTML)) {
                using (var reader = new StreamReader (source.Data, Encoding.Unicode, false, (int)source.Data.Length, true)) {
                    var pos = source.Data.Position;
                    var s = reader.ReadToEnd ();
                    s = System.Net.WebUtility.HtmlEncode (s);
                    source.Data.Position = pos;
                    return new Content<Stream> (s.AsAsciiStream (), CompressionType.None, ContentTypes.HTML);
                }
            }
            return null;
        }
    }
}