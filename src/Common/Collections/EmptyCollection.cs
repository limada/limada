using System.Collections.Generic;

namespace Limaki.Common.Collections {
    public class EmptyCollection<T>:ICollection<T> {

        #region ICollection<T> Member

        public void Add(T item) {}

        public void Clear() {}

        public bool Contains(T item) {
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex) {}

        public int Count { get {return 0;}}

        public bool IsReadOnly {get { return true; } }

        public bool Remove(T item) {return false; }

        #endregion

        #region IEnumerable<T> Member

        public IEnumerator<T> GetEnumerator() {
            yield break;
        }

        #endregion

        #region IEnumerable Member

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator ();
        }

        #endregion
    }
    public struct EmptyEnumerable<T>:IEnumerable<T> {
        #region IEnumerable<T> Member

        public IEnumerator<T> GetEnumerator() {
            yield break;
        }

        #endregion

        #region IEnumerable Member

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator ();
        }

        #endregion
    }
}