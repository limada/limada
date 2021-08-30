﻿/*
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

        public static IEnumerable<T> OnEach<T> (this IEnumerable<T> items, Func<T, T> func) {
            foreach (var item in items)
                yield return func(item);
        }

        public static IEnumerable<T> OnEach<T> (this IEnumerable<T> items, Action<T> action) {
            foreach (var item in items) {
                action (item);
                yield return item;
            }
        }

    }

    public static class Repeat { 
        
        public static IEnumerable<T> For<T> (this Func<T> it, int count) {
            for (var i = 0; i < count; i++) yield return it ();
        }

        public static void Action (Action it, int count) {
            for (var i = 0; i < count; i++) { 
                it (); 
            }
        }

    }
}