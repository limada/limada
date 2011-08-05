using System.Collections.Generic;
using Limaki.Common.Collections;

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
    }
}