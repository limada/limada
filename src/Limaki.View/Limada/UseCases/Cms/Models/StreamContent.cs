using System.IO;
using Limaki.Contents;

namespace Limada.UseCases.Cms.Models {
    public class StreamContent : Content<Stream> {
        public StreamContent () { }
        public StreamContent (Limaki.Contents.Content content):base(content){}
        public string MimeType { get; set; }
    }
}