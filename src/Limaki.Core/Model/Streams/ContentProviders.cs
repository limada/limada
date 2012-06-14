using System.Collections.Generic;
using Limaki.Common.Collections;
using System;

namespace Limaki.Model.Streams {
    public class ContentProviders : IEnumerable<IContentProvider> {
        private ICollection<IContentProvider> providers = new Set<IContentProvider>();

        public virtual void Add(IContentProvider provider) {
            providers.Add(provider);
        }

        public virtual void Remove(IContentProvider provider) {
            providers.Remove(provider);
        }

        public virtual IContentProvider Find(string extension) {
            foreach (var provider in providers) {
                if (provider.Supports(extension)) {
                    return provider;
                }
            }
            return null;
        }

        public virtual IContentProvider Find(long streamType) {
            foreach (var provider in providers) {
                if (provider.Supports(streamType)) {
                    return provider;
                }
            }
            return null;
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