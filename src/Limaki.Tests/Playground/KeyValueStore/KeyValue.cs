/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2012 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Limaki.Playground.KeyValueStore {

    public class KeyValue<K,V> {
        public K ID { get; set; }
        public K Key { get; set; }
        public K Member { get; set; }
        public V Value { get; set; }
    }

    public class Kvs<K> {
        public static K Clazz;
        public static K Member;

        public static K Id() { return default( K ); }
        public static KeyValue<K, V> Value<V>(K key, K member, V value) {
            return new KeyValue<K, V> { ID = Id(), Key = key, Member = member, Value = value };
        }
    }

    
    public class DomainExample:Kvs<long> {

        public class Adress {
            public long ID { get; set; }
            public string Street { get; set; }
        }

        // todo: create Attributes to mark classes and members
        public class AdressSchema {
            public static long Class = 0xabc;
            public static long Street = 0xabc;
        }

        // todo: use Attributes create schema
        public static void CreateAdressMetadata() {
            var address = Value(
                Clazz,
                AdressSchema.Class,
               typeof( Adress ).Name
            );
            var street = Value(
                AdressSchema.Class,
                AdressSchema.Street,
                "Street"
            );
        }

        public void StoreAdress(Adress adress) {
           var s = Value( adress.ID, AdressSchema.Street, adress.Street );
        }
    }
}
