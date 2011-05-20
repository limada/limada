/*
 * Limada 
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

using System.Linq;
using System.Runtime.Serialization;

namespace Limada.Common {
    using System.Linq;
    using System.Collections.Generic;

    public class Copier<T> {
        public bool AreEqual(T source, T target) {
            bool result = true;
            var type = source.GetType();
            var desttype = target.GetType();
            var dataMembers = type.GetProperties()
                .Where(p => p.IsDefined(typeof(DataMemberAttribute), true))
                .ToList();


            foreach (var info in dataMembers) {
                var sourceValue = info.GetValue(source, null);
                var prop = desttype.GetProperty(info.Name, info.PropertyType);
                if (prop != null) {
                    var targetValue = prop.GetValue(target, null);
                    if (sourceValue == null && targetValue == null) {
                        result = result && true;
                    } else if (sourceValue!=null) {
                        result = result && sourceValue.Equals(targetValue);
                    } else {
                        result = false;
                    }
                }
                if (!result)
                    break;
            }
            return result;
        }

        public T Copy(T source, T target) {

            if (source == null || target == null) {
                return target;
            }

            var type = source.GetType();
            var desttype = target.GetType ();
            var dataMembers = type.GetProperties()
                .Where(p => p.IsDefined(typeof(DataMemberAttribute), true))
                .ToList();


            foreach(var info in dataMembers) {
                var sourceValue = info.GetValue (source, null);
                var prop = desttype.GetProperty (info.Name, info.PropertyType);
                if (prop != null) {
                    prop.SetValue (target, sourceValue, null);
                }
            }
            return target;

        }
    }
}