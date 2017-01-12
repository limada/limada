using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Limaki.Common.Text.HTML;
using Limaki.Contents;

namespace Limada.Usecases.Cms {

    public class HtmlRtfConverter : ContentConverter<Stream>, IContentConverter<Stream> {

        public override IEnumerable<Tuple<long, long>> SupportedTypes { get { yield return Tuple.Create (ContentTypes.RTF, ContentTypes.HTML); } }

        protected string StringFromStream (Stream source) {
            var memorystream = source as MemoryStream;
            if (memorystream != null) {
                return Encoding.Default.GetString (memorystream.GetBuffer());
            } else {
                var reader = new StreamReader (source, Encoding.Default);
                return reader.ReadToEnd();
            }
        }

        protected Stream StringToStream (string source, Stream sink) {
            if (sink == null)
                sink = new MemoryStream (source.Length * 2);
            var pos = sink.Position;
            var bytes = Encoding.Default.GetBytes (source);
            sink.Write (bytes, 0, bytes.Length);
            sink.Position = pos;
            return sink;
        }

        public override Content<Stream> Use (Content<Stream> source, Content<Stream> sink) {

            if (ProveTypes (source, ContentTypes.RTF, sink, ContentTypes.HTML)) {
                var converter = new Limaki.Swing.Converter {
                                    Source = source.Data,
                                    SourceType = source.ContentType
                                };

                converter.Read();

                converter.RemovePmTags();
                converter.SinkType = sink.ContentType;
                converter.Write();

                if (true) {
                    var html = StringFromStream (converter.Sink);
                    var cleaner = new HtmlCleaner (html) {

                        RemoveSpan = true,
                        RemoveFonts = true,
                        RemoveStrong = true,
                        RemoveTable = false,
                        RemoveCData = true,
                        RemoveStyle = true,
                        RemoveComment = true,

                    };

                    sink.Data = StringToStream (cleaner.Clean(), sink.Data);
                }
            }
            return sink;
        }
    }
}