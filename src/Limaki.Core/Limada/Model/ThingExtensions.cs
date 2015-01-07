/*
 * Limada
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Limada.Model {

    public static class ThingExtensions {

        /// <summary>
        /// copies Id, ChangeDate, CreationDate, State, Data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="r"></param>
        public static void CopyTo<T> (this T s, T r) where T:Thing {
            r.SetId (s.Id);
            r.SetChangeDate (s.ChangeDate);
            r.SetCreationDate (s.CreationDate);
            s.State.CopyTo (r.State);
            ((IThing) r).Data = ((IThing) s).Data;
        }

        public static IEnumerable<StringThing> AsThingStrings (this IEnumerable<Thing<string>> entities) {
            return entities.Select (e => {
                var r = e as StringThing;
                if (r != null)
                    return r;
                var s = (Thing<string>) e;
                r = new StringThing (s.Id, s.Data);
                s.CopyTo (r);
                return r;
            });
        }

        public static IEnumerable<NodeThing> AsThingNodes (this IEnumerable<Thing> entities) {
            return entities.Select (e => {
                var r = e as NodeThing;
                if (r != null)
                    return r;
                if (typeof (Thing) != e.GetType ())
                    throw new ArgumentException ("Must be exactly a Thing (and not inherited)");
                r = new NodeThing (e.Id);
                e.CopyTo (r);
                return r;
            });
        }
    }
}