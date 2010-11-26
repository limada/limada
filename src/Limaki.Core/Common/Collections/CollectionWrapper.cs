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


using System.Collections.Generic;

namespace Limaki.Common.Collections {
    public class CollectionWrapper<T>:ICollection<T> {
        public CollectionWrapper(ICollection<T> source) {
            this._source = source;
        }

        protected ICollection<T> _source = null;
        #region ICollection<T> Member

        public virtual void Add(T item) {
            _source.Add (item);
        }

        public virtual void Clear() {
            _source.Clear ();
        }

        public virtual bool Contains(T item) {
            return _source.Contains (item);
        }

        public virtual void CopyTo(T[] array, int arrayIndex) {
            _source.CopyTo (array, arrayIndex);
        }

        public virtual int Count {
            get { return _source.Count; }
        }

        public virtual bool IsReadOnly {
            get { return _source.IsReadOnly; }
        }

        public virtual bool Remove(T item) {
            return _source.Remove (item);
        }

        #endregion

        #region IEnumerable<T> Member

        public virtual IEnumerator<T> GetEnumerator() {
            return _source.GetEnumerator ();
        }

        #endregion

        #region IEnumerable Member

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator ();
        }

        #endregion
    }
}