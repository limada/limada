/*
 * Limaki 
 * Version 0.064
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

namespace Limaki.Common {
    public struct Pair<T,U> {
        public T One;
        public U Two;
        public Pair (T one, U two) {
            One = one;
            Two = two;
        }
    }
    public struct Triple<T,U,V> {
        public T One;
        public U Two;
        public V Three;
        public Triple(T one, U two, V three) {
            One = one;
            Two = two;
            Three = three;
        }
    }
}
