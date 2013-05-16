namespace Limaki.Model.Content {

    public class ContentInfo {

        public ContentInfo(string streamTypeDescription, long streamType, string extension, string mimeType, CompressionType compression) {
            this.ContentType = streamType;
            this.Extension = extension;
            this.MimeType = mimeType;
            this.Description = streamTypeDescription;
            this.Compression = Compression;
        }

        public ContentInfo(string streamTypeDescription, long streamType, string extension, string mimeType, CompressionType compression, Magic[] magics
            )
            : this(streamTypeDescription, streamType, extension, mimeType, compression) {
            this.Magics = magics;
        }

        public CompressionType Compression { get; set; }
        public long ContentType { get; set; }
        public string Description { get; set; }
        public string Extension { get; set; }
        public string MimeType { get; set; }
        public Magic[] Magics { get; set; }

    }
}