/*
 Copyright (c) 2003-2006 Niels Kokholm and Peter Sestoft

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:
 
 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.
 
 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 SOFTWARE.
 * 
 * 
 * This is a copy of C5 Release 1.1.0 / 2008-02-10
 * UserGuideExamples\MultiDictionary.cs
 * 
 * changed: using Limaki.Common.Collections.Set instead of C5.HashSet
*/

using System;
using C5;
//using System.Collections.Generic;
using SCG = System.Collections.Generic;

namespace Limaki.Common.Collections {
    // Here we implement a multivalued dictionary as a hash dictionary
    // from keys to value collections.  The value collections may have
    // set or bag semantics.

    // The value collections are externally modifiable (as in Peter
    // Golde's PowerCollections library), and therefore:
    //
    //  * A value collection associated with a key may be null or
    //  non-empty.  Hence for correct semantics, the Contains(k) method
    //  must check that the value collection associated with a key is
    //  non-null and non-empty.
    //  
    //  * A value collection may be shared between two or more keys.
    //

    public class MultiDictionary<K, V> : C5.HashDictionary<K, SCG.ICollection<V>> {

        // Return total count of values associated with keys.  This basic
        // implementation simply sums over all value collections, and so
        // is a linear-time operation in the total number of values.  

        public new virtual int Count {
            get {
                int count = 0;
                foreach (KeyValuePair<K, SCG.ICollection<V>> entry in this)
                    if (entry.Value != null)
                        count += entry.Value.Count;
                return count;
            }
        }

        public override C5.Speed CountSpeed {
            get { return Speed.Linear; }
        }

        // Add a (key,value) pair

        public virtual void Add(K k, V v) {
            SCG.ICollection<V> values;
            if (!base.Find(k, out values) || values == null) {
                values = new Set<V>();
                Add(k, values);
            }
            values.Add(v);
        }

        // Remove a single (key,value) pair, if present; return true if
        // anything was removed, else false

        public virtual bool Remove(K k, V v) {
            SCG.ICollection<V> values;
            if (base.Find(k, out values) && values != null) {
                if (values.Remove(v)) {
                    if (values.Count==0)
                        base.Remove(k);
                    return true;
                }
            }
            return false;
        }

        // Determine whether key k is associated with a value

        public override bool Contains(K k) {
            //ICollection<V> values;
            return base.Contains (k);
                // we allow null or empty values
                //base.Find (k, out values) && values != null && !values.IsEmpty;
        }

        // Determine whether each key in ks is associated with a value

        public override bool ContainsAll<U>(SCG.IEnumerable<U> ks) {
            foreach (K k in ks)
                if (!Contains(k))
                    return false;
            return true;
        }

        // Get or set the value collection associated with key k

        public override SCG.ICollection<V> this[K k] {
            get {
                SCG.ICollection<V> values;
                return base.Find(k, out values) && values != null ? values : new Set<V>();
            }
            set {
                base[k] = value;
            }
        }

        // Inherited from base class HashDictionary<K,ICollection<V>>:

        // Add(K k, ICollection<V> values) 
        // AddAll(IEnumerable<KeyValuePair<K,ICollection<V>>> kvs) 
        // Clear
        // Clone
        // Find(K k, out ICollection<V> values)
        // Find(ref K k, out ICollection<V> values)
        // FindOrAdd(K k, ref ICollection<V> values) 
        // Remove(K k) 
        // Remove(K k, out ICollection<V> values) 
        // Update(K k, ICollection<V> values)
        // Update(K k, ICollection<V> values, out ICollection<V> oldValues)
        // UpdateOrAdd(K k, ICollection<V> values)
        // UpdateOrAdd(K k, ICollection<V> values, out ICollection<V> oldValues)
    }

    public class SortedMultiDictionary<K, V> : C5.TreeDictionary<K, SCG.ICollection<V>> {

        public SortedMultiDictionary(SCG.IComparer<K> comparer):base(comparer) {}
        // Return total count of values associated with keys.  This basic
        // implementation simply sums over all value collections, and so
        // is a linear-time operation in the total number of values.  

        public new virtual int Count {
            get {
                int count = 0;
                foreach (KeyValuePair<K, SCG.ICollection<V>> entry in this)
                    if (entry.Value != null)
                        count += entry.Value.Count;
                return count;
            }
        }

        public override C5.Speed CountSpeed {
            get { return Speed.Linear; }
        }

        // Add a (key,value) pair

        public virtual void Add(K k, V v) {
            SCG.ICollection<V> values;
            if (!base.Find(k, out values) || values == null) {
                values = new Set<V>();
                Add(k, values);
            }
            if (!values.Contains(v))
            values.Add(v);
        }

        // Remove a single (key,value) pair, if present; return true if
        // anything was removed, else false

        public virtual bool Remove(K k, V v) {
            SCG.ICollection<V> values;
            if (base.Find(k, out values) && values != null) {
                if (values.Remove(v)) {
                    if (values.Count == 0)
                        base.Remove(k);
                    return true;
                }
            }
            return false;
        }

        // Determine whether key k is associated with a value

        public override bool Contains(K k) {
            //ICollection<V> values;
            return base.Contains(k);
            // we allow null or empty values
            //base.Find (k, out values) && values != null && !values.IsEmpty;
        }

        // Determine whether each key in ks is associated with a value

        public override bool ContainsAll<U>(SCG.IEnumerable<U> ks) {
            foreach (K k in ks)
                if (!Contains(k))
                    return false;
            return true;
        }

        // Get or set the value collection associated with key k

        public override SCG.ICollection<V> this[K k] {
            get {
                SCG.ICollection<V> values;
                return base.Find(k, out values) && values != null ? values : new Set<V>();
            }
            set {
                base[k] = value;
            }
        }

        // Inherited from base class HashDictionary<K,ICollection<V>>:

        // Add(K k, ICollection<V> values) 
        // AddAll(IEnumerable<KeyValuePair<K,ICollection<V>>> kvs) 
        // Clear
        // Clone
        // Find(K k, out ICollection<V> values)
        // Find(ref K k, out ICollection<V> values)
        // FindOrAdd(K k, ref ICollection<V> values) 
        // Remove(K k) 
        // Remove(K k, out ICollection<V> values) 
        // Update(K k, ICollection<V> values)
        // Update(K k, ICollection<V> values, out ICollection<V> oldValues)
        // UpdateOrAdd(K k, ICollection<V> values)
        // UpdateOrAdd(K k, ICollection<V> values, out ICollection<V> oldValues)
    }
}
