using System;
using Limaki.Model.Streams;
using System.IO;
using Id = System.Int64;

namespace Limada.Model {
    public interface IStreamThing : IThing<Stream>, IContainerProxy<Id> {
        CompressionType Compression { get; set; }
        Id StreamType { get; set; }

        void Compress();
        void DeCompress();
    }
}