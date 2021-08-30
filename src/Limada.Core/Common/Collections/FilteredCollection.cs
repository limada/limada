/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */


using System;
using System.Collections.Generic;

namespace Limaki.Common.Collections {
    public class FilteredCollection<T>:CollectionWrapper<T> {
        Func<T, bool> Filter = null;

        public FilteredCollection(ICollection<T> source, Func<T, bool> filter):base(source) {
            this.Filter = filter;
        }

        public override void Add(T item) {
            if (Filter(item))
                base.Add(item);
        }

        public override void Clear() {
            foreach (T item in this) {
                if (Filter(item)) {
                    base.Remove (item);
                }
            }
        }

        public override bool Contains(T item) {
            return Filter(item) && base.Contains(item);
        }

        public override IEnumerator<T> GetEnumerator() {
            foreach (T item in _source) {
                if (Filter(item)) {
                    yield return item;
                }
            }
        }

        public override bool Remove(T item) {
            return Filter(item) && base.Remove(item);
        }
    }
}