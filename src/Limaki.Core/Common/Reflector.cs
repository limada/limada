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

using System;

namespace Limaki.Common {
    public class Reflector {
        public static bool IsStorable (Type type){
            return !(type.IsPrimitive || type == typeof(string));
        }

        public static bool Implements(Type clazz, Type interfaze) {
            if (clazz.IsClass) {
                bool result = (interfaze.IsAssignableFrom(clazz));
                if (! result && interfaze.IsInterface) {
                    foreach (Type t in clazz.GetInterfaces()) {
                        if (t == interfaze) {
                            result = true;
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
