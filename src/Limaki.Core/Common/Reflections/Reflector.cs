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
using System.Text;
using System.Linq;
using Limaki.Common.Text;

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
                var result = (interfaze.IsAssignableFrom(clazz));
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

        /// <summary>
        /// gives back the name of integral types
        /// </summary>
        /// <returns>The integral type name.</returns>
        /// <param name="type">Type.</param>
        public static string FriendlyIntegralTypeName (Type type) {
            switch (type) {
                case Type t when t == typeof (Int16):
                    return "short";
                case Type t when t == typeof (Int32):
                    return "int";
                case Type t when t == typeof (Int64):
                    return "long";
                case Type t when t == typeof (UInt16):
                    return "ushort";
                case Type t when t == typeof (UInt32):
                    return "uint";
                case Type t when t == typeof (UInt64):
                    return "ulong";
                case Type t when t == typeof (Single):
                    return "float";
                case Type t when
                        t == typeof (Object) ||
                        t == typeof (String) ||
                        t == typeof (Byte) ||
                        t == typeof (Char) ||
                        t == typeof (Decimal) ||
                        t == typeof (SByte) ||
                        t == typeof (Double)
                        :
                    return t.Name.ToLower ();
                default:
                    return type.Name;
            }
        }

        public static string FriendlyClassName (this Type type) {
            var result = new StringBuilder();
            if (type.IsNested && !type.IsGenericParameter) {
                result.Append (type.FullName.Replace (type.DeclaringType.FullName + "+", FriendlyClassName (type.DeclaringType)));
            } else {
                result.Append (FriendlyIntegralTypeName (type));
            }
            if (type.IsGenericType) {

                var genPos = result.IndexOf ("`");
                if (genPos > 0)
                    result = result.Remove (genPos,result.Length-genPos);

                bool isNullable = Nullable.GetUnderlyingType (type) != null;
                if (isNullable) {
                    result.Clear ();
                } else
                    result.Append ("<");

                result.Append (string.Join(",", type.GetGenericArguments ().Select(t=>t.FriendlyClassName())));
                if (isNullable)
                    result.Append ("?");
                else
                    result.Append (">");
            }
            return result.ToString();
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
