using System.IO;
using Limaki.Contents;

namespace Limada.Usecases.Cms.Models {
    public class StreamContent : Content<Stream> {
        public StreamContent () { }
        public StreamContent (Content content):base(content){}
        public string MimeType { get; set; }
    }
}