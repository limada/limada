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

namespace Limaki.Common.Collections {
    public class FilteredCollection<T>:CollectionWrapper<T> {
        Predicate<T> Filter = null;

        public FilteredCollection(ICollection<T> source, Predicate<T> filter):base(source) {
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