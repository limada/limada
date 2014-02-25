using System.IO;
using Limaki.Common;
using Limaki.Contents;
using Limaki.Model.Content;
using Limaki.Contents.IO;
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

        private StreamContentIoPool _contentPool;
        public StreamContentIoPool ContentIoPool { get { return _contentPool ?? (_contentPool = Registry.Pool.TryGetCreate<StreamContentIoPool>()); } }

        private TransferContentPool _transferContentPool;
        public TransferContentPool TransferContentPool { get { return _transferContentPool ?? (_transferContentPool = Registry.Pool.TryGetCreate<TransferContentPool>()); } }

        private TransferContentTypes _transferContentTypes;
        public TransferContentTypes TransferContentTypes { get { return _transferContentTypes ?? (_transferContentTypes = Registry.Pool.TryGetCreate<TransferContentTypes>()); } }

        public virtual IEnumerable<Tuple<TransferDataType,IContentIo<Stream>>> SinksOf (IEnumerable<TransferDataType> sources) {
            
            foreach (var source in sources) {
                var sourceId = source.Id;
                long contentType = 0;
                Func<IContentIo<Stream>, bool> lookUp = null;
                if (TransferContentTypes.TryGetValue(sourceId.ToLower(), out contentType)) {
                    lookUp = sinkIo => sinkIo.Detector.ContentSpecs
                        .Any(info => info.ContentType == contentType);
                } else {
                    lookUp = sinkIo => sinkIo.Detector.ContentSpecs
                        .Any(info => info.MimeType == sourceId);
                }
                var done = new Set<long>();
                foreach (var sinkIo in TransferContentPool.Where(lookUp)) {
                    done.AddRange(sinkIo.Detector.ContentSpecs.Select(info => info.ContentType));
                    yield return Tuple.Create(source,sinkIo);
                }
                foreach (var sinkIo in ContentIoPool.Where(lookUp)
                    .Where(io => ! io.Detector.ContentSpecs.Any(info => done.Contains(info.ContentType)))) {
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
    public class TransferContentTypes : Dictionary<string, long> {
        public virtual IEnumerable<TransferDataType> DataTypes {
            get {
                foreach (var c in this.Keys)
                    yield return TransferDataType.FromId(c);
            }
        }
    }

    /// <summary>
    /// class to register special ContentIo's for Clipboard and DragDrop operations
    /// they override the common IoProvider<Stream, Content<Stream>>
    /// </summary>
    public class TransferContentPool:ContentIoPool<Stream, Content<Stream>> {}
}