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
 * http://limada.sourceforge.net
 * 
 */

using System;
using Limaki.Common.Collections;

namespace Limaki.Common {
    public class Reflector {
        public static bool IsStorable (Type type){
            return !(type.IsPrimitive || type == typeof(string));
        }

        public static Set<Tuple<Type, Type>> _implements = new Set<Tuple<Type, Type>>();
        public static bool Implements(Type clazz, Type interfaze) {
            if (clazz.IsClass) {
                var key = Tuple.Create(clazz, interfaze);
                if (_implements.Contains(key))
                    return true;
                bool result = (interfaze.IsAssignableFrom(clazz));
                if (! result && interfaze.IsInterface) {
                    foreach (Type t in clazz.GetInterfaces()) {
                        if (t == interfaze) {
                            result = true;
                            _implements.Add(key);
                            break;
                        }
                    }
                }
                return result;
            } else {
                return false;
            }
        }
    }
}
