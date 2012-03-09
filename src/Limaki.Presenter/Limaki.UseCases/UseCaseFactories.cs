using System.Collections.Generic;

namespace Limaki.UseCases {
    public class UseCaseFactories<T> : IEnumerable<UseCaseFactory<T>> where T:new() {
        private ICollection<UseCaseFactory<T>> factories = new List<UseCaseFactory<T>>();
        public void Add(UseCaseFactory<T> f) {
            factories.Add(f); 
        }
        public IEnumerator<UseCaseFactory<T>> GetEnumerator() {
            return factories.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }
}