using System;
using System.Collections.Generic;

#if SILVERLIGHT
using System.Linq;
#endif

namespace Limaki.Common.Collections {
    public class History<T>:IEnumerable<T> {
        private List<T> back = new List<T>();
        private List<T> forward = new List<T>();
            
        public bool HasForward() {
            return forward.Count > 0;
        }

        public bool HasBack() {
            return back.Count > 0;
        }

        public void Add(T item) {
            back.Add (item);
            Current = item;
        }


        public T Current = default( T );

        public T Forward() {
            T result = default( T );
            if (HasForward()) {
                result = forward[0];
                forward.RemoveAt (0);
                back.Add (result);
                if (result.Equals(Current)) {
                    result = Forward();
                }
            }

            Current = result;
            return result;
        }

        public T Back() {
            T result = default(T);
            if (HasBack()) {
                result = back[back.Count-1];
                back.RemoveAt (back.Count - 1);
                forward.Insert(0,result);
                if (result.Equals(Current)) {
                    result = Back();
                }
            }

            Current = result;
            return result;
        }

        public T GetItem(Predicate<T> pred) {
            foreach(T item in forward) {
                if (pred(item)) {
                    return item;
                }
            }
            foreach (T item in back) {
                if (pred(item)) {
                    return item;
                }
            }
            return default(T);
        }

        public T Remove(Predicate<T> pred) {
            var result = default(T);
#if !SILVERLIGHT

            int i = forward.FindIndex (pred);


            if (i!=-1) {
                result = forward[i];
                forward.RemoveAt (i);
            }
            i = back.FindIndex(pred);
            if (i != -1) {
                result = back[i];
                back.RemoveAt(i);
            }
#endif

            if (Current != null && Current.Equals(result)) {
                Current = default(T);
            }

            return result;
        }

        public IEnumerator<T> GetEnumerator() {
            foreach (T t in back)
                yield return t;
            foreach (T t in forward)
                yield return t;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator ();
        }


    }
}