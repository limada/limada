/*
 * Limaki 
 * Version 0.081
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
    public struct Pair<T> {
        public T One;
        public T Two;
        public Pair(T one, T two) {
            One = one;
            Two = two;
        }
    }

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

    public struct Record<T1, T2, T3> {
        public T1 One;
        public T2 Two;
        public T3 Three;
        public Record(T1 one, T2 two, T3 three) {
            One = one;
            Two = two;
            Three = three;
        }
    }

    public struct Record<T1, T2, T3,T4> {
        public T1 One;
        public T2 Two;
        public T3 Three;
        public T4 Four;
        public Record(T1 one, T2 two, T3 three, T4 four) {
            One = one;
            Two = two;
            Three = three;
            Four = four;
        }
    }
}
