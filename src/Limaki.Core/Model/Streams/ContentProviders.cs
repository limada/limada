using System.Collections.Generic;
using Limaki.Common.Collections;
using System;
using Limaki.Common.Linqish;
using System.Linq;
using System.IO;

namespace Limaki.Model.Streams {
    public class ContentProviders : IEnumerable<IContentProvider> {
        private ICollection<IContentProvider> providers = new Set<IContentProvider>();

        public virtual void Add(IContentProvider provider) {
            providers.Add(provider);
            provider.SupportedStreamTypes.ForEach(t => MimeTypes[t.StreamType]=t.MimeType);
        }

        public virtual void Remove(IContentProvider provider) {
            providers.Remove(provider);
            provider.SupportedStreamTypes.ForEach (t => MimeTypes.Remove (t.StreamType));
        }

        public virtual IContentProvider Find(string extension) {
            return providers.Where(provider => provider.Supports(extension)).FirstOrDefault();
        }

        public virtual IContentProvider Find(long streamType) {
            return providers.Where (provider => provider.Supports (streamType)).FirstOrDefault ();
        }

        public virtual IContentProvider Find (Stream stream) {
            return providers.Where (provider => provider.Supports (stream)).FirstOrDefault ();
        }

        public IEnumerator<IContentProvider> GetEnumerator() {
            return providers.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        #region MimeTypes - Refactor this

        IDictionary<long, string> _mimeTypes = null;
        public virtual IDictionary<long, string> MimeTypes {
            get {
                if (_mimeTypes == null) {
                    _mimeTypes = new Dictionary<long, string> ();
                    _mimeTypes.Add (StreamTypes.HTML, "text/html");
                    _mimeTypes.Add (StreamTypes.ASCII, "text/plain");
                    _mimeTypes.Add (StreamTypes.Doc, "application/msword");
                    _mimeTypes.Add (StreamTypes.GIF, "image/gif");
                    _mimeTypes.Add (StreamTypes.JPG, "image/jpeg");
                    _mimeTypes.Add (StreamTypes.PNG, "image/png");
                    _mimeTypes.Add (StreamTypes.RTF, "text/rtf");
                    _mimeTypes.Add (StreamTypes.TIF, "image/tiff");
                }
                return _mimeTypes;
            }
        }

        public virtual string MimeType (Int64 streamType) {
            string result = null;
            MimeTypes.TryGetValue (streamType, out result);
            return result;
        }
        #endregion
    }
}