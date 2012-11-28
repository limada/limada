/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2011 Lytico
 *
* http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Limaki.Common.Linqish {
    public static class EnumerableExtensions {

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action) {
            foreach (var item in items)
                action(item);

        }

        public static IEnumerable<T> Yield<T> (this IEnumerable<T> items) {
            foreach (var item in items)
                yield return item;
        }
    }
}