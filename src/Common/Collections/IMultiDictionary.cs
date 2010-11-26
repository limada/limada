/*
 * Limaki 
 * Version 0.071
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
    public interface IMultiDictionary<K, V> : IDictionary<K, ICollection<V>> {
        void Add(K key, V value);
        bool Remove(K key, V v);
        bool Contains(K key);
    }
}