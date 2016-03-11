using System.Collections.Generic;
using System;
using System.Linq;
using Limaki.Common.Linqish;
using Limaki.Contents;

namespace Limaki.Contents {

    public class ContentInfo {

        public ContentInfo(string description, long contentType, string extension, string mimeType, CompressionType compression) {
            this.ContentType = contentType;
            this.Extension = extension;
            this.MimeType = mimeType;
            this.Description = description;
            this.Compression = compression;
        }

        public ContentInfo(string description, long contentType, string extension, string mimeType, CompressionType compression, Magic[] magics)
            : this(description, contentType, extension, mimeType, compression) {
            this.Magics = magics;
        }

        public virtual CompressionType Compression { get; protected set; }
        public virtual long ContentType { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual string Extension { get; protected set; }
        public virtual string MimeType { get; protected set; }
        public virtual Magic[] Magics { get; protected set; }

		public bool HasMagics { get { return Magics != null && Magics.Length > 0; } }
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