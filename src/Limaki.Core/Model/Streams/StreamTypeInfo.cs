namespace Limaki.Model.Streams {
    public class StreamTypeInfo {

        public StreamTypeInfo(
            string streamTypeDescription,
            long streamType,
            string extension,
            string mimeType,
            CompressionType compression) {
            this.StreamType = streamType;
            this.Extension = extension;
            this.MimeType = mimeType;
            this.StreamTypeDescription = streamTypeDescription;
            this.Compression = Compression;
        }

        public StreamTypeInfo(
            string streamTypeDescription,
            long streamType,
            string extension,
            string mimeType,
            CompressionType compression,
            Magic[] magics
            )
            : this(streamTypeDescription, streamType, extension, mimeType, compression) {
            this.Magics = magics;
        }

        public CompressionType Compression { get; set; }
        public long StreamType { get; set; }
        public string StreamTypeDescription { get; set; }
        public string Extension { get; set; }
        public string MimeType { get; set; }
        public Magic[] Magics { get; set; }
    }
}