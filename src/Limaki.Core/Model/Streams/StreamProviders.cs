using System.Collections.Generic;
using Limaki.Common.Collections;

namespace Limaki.Model.Streams {
    public class StreamProviders : IEnumerable<IStreamProvider> {
        private ICollection<IStreamProvider> providers = new Set<IStreamProvider>();

        public virtual void Add(IStreamProvider provider) {
            providers.Add(provider);
        }

        public virtual void Remove(IStreamProvider provider) {
            providers.Remove(provider);
        }

        public virtual IStreamProvider Find(string extension) {
            foreach (var provider in providers) {
                if (provider.Supports(extension)) {
                    return provider;
                }
            }
            return null;
        }

        public virtual IStreamProvider Find(long streamType) {
            foreach (var provider in providers) {
                if (provider.Supports(streamType)) {
                    return provider;
                }
            }
            return null;
        }

        public IEnumerator<IStreamProvider> GetEnumerator() {
            return providers.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }
}