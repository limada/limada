using System.IO;
using Limaki.Common;
using Limaki.Model.Content;
using Limaki.Model.Content.IO;
using Xwt;
using System.Linq;
using System.Collections.Generic;
using System;
using Limaki.Common.Collections;
using System.Text;

namespace Limaki.View.DragDrop {
    /// <summary>
    /// this class handles clipboard and drag-drop
    /// </summary>
    public class TransferDataManager {

        private IoProvider<Stream, Content<Stream>> _contentProvider;
        public IoProvider<Stream, Content<Stream>> ContentProvider { get { return _contentProvider ?? (_contentProvider = Registry.Pool.TryGetCreate<IoProvider<Stream, Content<Stream>>>()); } }

        private TransferContentProvider _transferContentProvider;
        public TransferContentProvider TransferContentProvider { get { return _transferContentProvider ?? (_transferContentProvider = Registry.Pool.TryGetCreate<TransferContentProvider>()); } }

        private TransferContentTypes _transferContentTypes;
        public TransferContentTypes TransferContentTypes { get { return _transferContentTypes ?? (_transferContentTypes = Registry.Pool.TryGetCreate<TransferContentTypes>()); } }

        public virtual IEnumerable<Tuple<TransferDataType,ISinkIo<Stream>>> SinksOf (IEnumerable<TransferDataType> sources) {
            foreach (var source in sources) {
                var sourceId = source.Id;
                long contentType = 0;
                Func<ISinkIo<Stream>, bool> lookUp = null;
                if (TransferContentTypes.TryGetValue(sourceId.ToLower(), out contentType)) {
                    lookUp = sinkIo => sinkIo.InfoSink.SupportedContents
                        .Any(info => info.ContentType == contentType);
                } else {
                    lookUp = sinkIo => sinkIo.InfoSink.SupportedContents
                        .Any(info => info.MimeType == sourceId);
                }
                var done = new Set<long>();
                foreach (var sinkIo in TransferContentProvider.Where(lookUp)) {
                    done.AddRange(sinkIo.InfoSink.SupportedContents.Select(info => info.ContentType));
                    yield return Tuple.Create(source,sinkIo);
                }
                foreach (var sinkIo in ContentProvider.Where(lookUp)
                    .Where(io => ! io.InfoSink.SupportedContents.Any(info => done.Contains(info.ContentType)))) {
                        yield return Tuple.Create(source, sinkIo);
                }
            }
        }

        public Stream AsUnicodeStream (string source) {
            var buffer = Encoding.Unicode.GetBytes(source);
            return new MemoryStream(buffer);
            Encoding.Convert(Encoding.Unicode, Encoding.ASCII, buffer);
        }

        public Stream AsAsciiStream (string source) {
            var buffer = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, 
                Encoding.Unicode.GetBytes(source));
            return new MemoryStream(buffer);
        }

        // move this to Resourceloader of OS:
        public void RegisterSome() {
            TransferContentTypes.Add("text", ContentTypes.Text);
            TransferContentTypes.Add("html", ContentTypes.HTML);
            TransferContentTypes.Add("rtf", ContentTypes.RTF);
            //...
        }
    }

    /// <summary>
    /// class to register the type values for Clipboard and DragDrop operations
    /// </summary>
    public class TransferContentTypes : Dictionary<string, long> {}

    /// <summary>
    /// class to register special SinkIo's for Clipboard and DragDrop operations
    /// they override the common IoProvider<Stream, Content<Stream>>
    /// </summary>
    public class TransferContentProvider:IoProvider<Stream, Content<Stream>> {}
}