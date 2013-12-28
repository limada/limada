using System.Collections.Generic;
using System;
using System.Linq;
using Limaki.Common.Linqish;

namespace Limaki.Model.Content {

    public class ContentInfo {

        public ContentInfo(string description, long contentType, string extension, string mimeType, CompressionType compression) {
            this.ContentType = contentType;
            this.Extension = extension;
            this.MimeType = mimeType;
            this.Description = description;
            this.Compression = Compression;
        }

        public ContentInfo(string description, long contentType, string extension, string mimeType, CompressionType compression, Magic[] magics)
            : this(description, contentType, extension, mimeType, compression) {
            this.Magics = magics;
        }

        public CompressionType Compression { get; set; }
        public long ContentType { get; set; }
        public string Description { get; set; }
        public string Extension { get; set; }
        public string MimeType { get; set; }
        public Magic[] Magics { get; set; }

    }

    public class ContentInfos : IEnumerable<ContentInfo>, IContentSpec {
        private ISet<ContentInfo> _contentInfos = new HashSet<ContentInfo>();
        public void AddRange (IEnumerable<ContentInfo> contentInfos) {
            contentInfos.ForEach(ci => _contentInfos.Add(ci));
        }
        public void Add (ContentInfo contentInfo) {
            _contentInfos.Add(contentInfo);
        }

        public IEnumerator<ContentInfo> GetEnumerator () {
            return _contentInfos.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator () {
            return this.GetEnumerator();
        }

        public IEnumerable<ContentInfo> ContentSpecs { get { return this; } }
    }

    
}