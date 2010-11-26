/*
 * Limaki 
 * Version 0.08
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Collections.Generic;
using Db4objects.Db4o;

namespace Limaki.Data.db4o {
    public class NativeQueryWrapper<T>:ICollection<T> {
        private IList<IObjectSet> _set = new List<IObjectSet> ();
        public NativeQueryWrapper(IObjectSet set) {
            AddSet (set);
        }

        public void AddSet(IObjectSet set) {
            this._set.Add (set);
        }
        #region ICollection<T> Member

        public void Add(T item) {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear() {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Contains(T item) {
            foreach(IObjectSet set in this._set) {
                if (set.Contains(item))
                    return true;
            }
            return false;

        }

        public void CopyTo(T[] array, int arrayIndex) {
            int count = Count - arrayIndex;
            T[] result = new T[count];
            int i = 0;
            foreach(T item in this) {
                if (i==arrayIndex) {
                    i = 0;
                    arrayIndex = 0;
                }
                if (i>=arrayIndex)
                    array[i] = item;
                i++;
            }

        }

        public int Count {
            get {
                int result = 0;
                foreach (IObjectSet set in this._set) {
                    result += set.Count;
                }
                return result;
            }
        }

        public bool IsReadOnly {
            get {
                return true;
            }
        }

        public bool Remove(T item) { return false; }

        #endregion

        #region IEnumerable<T> Member

        public IEnumerator<T> GetEnumerator() {
            foreach (IObjectSet set in this._set)
                foreach (object o in set) {
                    yield return (T)o;
                }
        }

        #endregion

        #region IEnumerable Member

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        #endregion
    }
}