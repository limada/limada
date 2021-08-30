using System;
using System.Collections.Generic;

namespace Limaki.Common.Collections {
    /// <summary>
    /// A collection#T# behaves as a Dictionary#T,T#
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DictionaryAdapter<T> : IDictionary<T, T> {
        ICollection<T> collection = null;
        public DictionaryAdapter(ICollection<T> list) { this.collection = list; }
        #region IDictionary<T,T> Member

        public void Add(T key, T value) {collection.Add (key); }

        public bool ContainsKey(T key) {return collection.Contains(key);}

        public ICollection<T> Keys {
            get { return collection; }
        }

        public bool Remove(T key) {return collection.Remove(key);}

        public bool TryGetValue(T key, out T value) {
            bool result = collection.Contains (key);
            if (result) {
                value = key;
            } else {
                value = default(T);
            }
            return result;
        }

        public ICollection<T> Values {
            get { return collection; }
        }

        public T this[T key] {
            get {
                if (collection.Contains(key))
                    return key;
                else
                    return default(T);
            }
            set { collection.Add(value); }
        }

        #endregion

        #region ICollection<KeyValuePair<T,T>> Member

        public void Add(KeyValuePair<T, T> item) { collection.Add (item.Key);  }

        public void Clear() { collection.Clear (); }

        public bool Contains(KeyValuePair<T, T> item) {return collection.Contains(item.Key);}

        public void CopyTo(KeyValuePair<T, T>[] array, int arrayIndex) {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Count {
            get { return collection.Count; }
        }

        public bool IsReadOnly {
            get { return collection.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<T, T> item) {return collection.Remove(item.Key);}

        #endregion

        #region IEnumerable<KeyValuePair<T,T>> Member

        public IEnumerator<KeyValuePair<T, T>> GetEnumerator() {
            foreach(T item in collection) {
                yield return new KeyValuePair<T, T> (item, item);
            }
        }

        #endregion

        #region IEnumerable Member

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator ();
        }

        #endregion
    }
}