using System.Collections.Generic;

namespace Limaki.Usecases {

    public class UsecaseFactories<T> : IEnumerable<UsecaseFactory<T>> where T:new() {

        private ICollection<UsecaseFactory<T>> factories = new List<UsecaseFactory<T>>();

        public void Add(UsecaseFactory<T> f) {
            factories.Add(f); 
        }

        public IEnumerator<UsecaseFactory<T>> GetEnumerator() {
            return factories.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }
}