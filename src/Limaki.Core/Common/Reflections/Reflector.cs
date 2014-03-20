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
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Common.Collections;
using System.Reflection;

namespace Limaki.Common.Reflections {

    public static class Reflector {

        public static bool IsStorable (Type type) {
            return !(type.IsPrimitive || type == typeof(string));
        }

        public static Set<Tuple<Type, Type>> _implements = new Set<Tuple<Type, Type>>();
        public static bool Implements (Type clazz, Type interfaze) {
            if (clazz.IsClass) {
                var key = Tuple.Create(clazz, interfaze);
                if (_implements.Contains(key))
                    return true;
                bool result = (interfaze.IsAssignableFrom(clazz));
                if (!result && interfaze.IsInterface) {
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

        public static string ClassName (Type type) {
            var result = type.Name;
            if (type.IsNested && !type.IsGenericParameter) {
                result = type.FullName.Replace(type.DeclaringType.FullName + "+", ClassName(type.DeclaringType));
            }
            if (type.IsGenericType) {

                var genPos = result.IndexOf('`');
                if (genPos > 0)
                    result = result.Substring(0, genPos);
                else
                    result = result + "";
                bool isNullable = result == "Nullable";
                if (!isNullable)
                    result += "<";
                else
                    result = "";
                foreach (var item in type.GetGenericArguments())
                    result += ClassName(item) + ",";
                result = result.Remove(result.Length - 1, 1);
                if (isNullable)
                    result += "?";
                else
                    result += ">";
            }
            return result;
        }

        private static A GetSingleAttribute<A>(object[] attributes)
           where A : Attribute {
            if (attributes.Length > 0)
                return (A)attributes[0];
            return null;
        }

        /// <summary>
        /// Returns a requested attribute for a given type
        /// </summary>
        /// <typeparam name="A">The requested attribute type</typeparam>
        /// <param name="t">The class supposed to provide that attribute</param>
        /// <returns>An attribute of type A or null if none</returns>
        public static A GetAttribute<A>(this Type t)
            where A : Attribute {
            return GetSingleAttribute<A>(t.GetCustomAttributes(typeof(A), true));
        }

        /// <summary>
        /// Returns a requested attribute for a given member
        /// </summary>
        /// <typeparam name="A">The requested attribute type</typeparam>
        /// <param name="m">The member supposed to provide that attribute</param>
        /// <returns>An attribute of type A or null if none</returns>
        public static A GetAttribute<A>(this MemberInfo m)
            where A : Attribute {
            return GetSingleAttribute<A>(m.GetCustomAttributes(typeof(A), true));
        }
    }
}
