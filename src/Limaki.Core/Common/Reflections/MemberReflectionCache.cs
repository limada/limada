/*
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
using System.Reflection;

namespace Limaki.Common.Reflections {

    public class MemberReflectionCache {
        IDictionary<Type, PropertyInfo[]> _members = new Dictionary<Type, PropertyInfo[]>();
        HashSet<Type> _done = new HashSet<Type>();


        HashSet<int> _membersFiltered = new HashSet<int>();
        IDictionary<int, PropertyInfo> _propertyInfos = new Dictionary<int, PropertyInfo>();
        IDictionary<int, Delegate> _propertySetter = new Dictionary<int, Delegate>();
        IDictionary<int, MethodInfo> _propertySetMethod = new Dictionary<int, MethodInfo>();
        IDictionary<int, MethodInfo> _propertyGetMethod = new Dictionary<int, MethodInfo>();

        public MemberReflectionCache () {
            this.BindingFlags = BindingFlags.Public | BindingFlags.Instance;
        }

        public MemberReflectionCache (BindingFlags bindingFlags) {
            this.BindingFlags = bindingFlags;
        }

        protected int GetHashCode (Type item1, Type item2, string item3) {
            int h = item1.GetHashCode();
            h = (h << 5) - h + item2.GetHashCode();
            h = (h << 5) - h + item3.GetHashCode();
            return h;
        }

        public BindingFlags BindingFlags { get; protected set; }
        public void AddType (Type type, Func<PropertyInfo, bool> memberFilter) {
            if (!_done.Contains(type)) {

                //TODO: look at this:http://www.codeproject.com/KB/cs/HyperPropertyDescriptor.aspx

                var propterties = type.GetProperties(BindingFlags).Where(memberFilter);
                _done.Add(type);
                foreach (var prop in propterties) {
                    var key = GetHashCode(type, prop.PropertyType, prop.Name);
                    _membersFiltered.Add(key);
                    _propertyInfos.Add(key, prop);
                    _propertySetMethod.Add(key, prop.GetSetMethod());
                    _propertyGetMethod.Add(key, prop.GetGetMethod());
                    // this is slower and uses more memory cause of dynamikinvoke:
                    //var delegateType = typeof (Action<,>).MakeGenericType(type, prop.PropertyType);
                    //_propertySetter.Add(key, Delegate.CreateDelegate(delegateType, null, prop.GetSetMethod()));
                }
            }
        }
        public void AddType (Type type) {
            AddType(type, p => p != null);
        }
        public PropertyInfo[] Members (Type type, Func<PropertyInfo, bool> memberFilter) {
            PropertyInfo[] members = null;
            if (!_members.TryGetValue(type, out members)) {
                members = type.GetProperties(BindingFlags).Where(memberFilter).ToArray();
                _members.Add(type, members);
            }
            return members;
        }

        public bool ValidMember (Type type, Type propertyType, string memberName) {
            var key = GetHashCode(type, propertyType, memberName);
            return _membersFiltered.Contains(key);
        }

        public void SetValue (Type type, Type propertyType, string memberName, object obj, object value) {
            var key = GetHashCode(type, propertyType, memberName);

            var meth = _propertySetMethod[key];
            meth.Invoke(obj, new object[] { value });

        }

        public object GetValue (Type type, Type propertyType, string memberName, object obj) {
            var key = GetHashCode(type, propertyType, memberName);

            var meth = _propertyGetMethod[key];
            return meth.Invoke(obj, null);

        }

        public PropertyInfo GetPropertyInfo (Type type, Type propertyType, string memberName) {
            var key = GetHashCode(type, propertyType, memberName);
            return _propertyInfos[key];
        }


    }
}