/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2012 Lytico
 *
 * http://www.limada.org
 * 
 */


using System;
using System.Runtime.Serialization;
using Limaki.Common.Reflections;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Limaki.Common {
    
    public class Copier<T> {
        
        public CopierOptions Option { get; protected set; }

        public Copier () {
            this.Attribute = typeof(DataMemberAttribute);
            this.Option = CopierOptions.DataMember;
        }

        public Copier (CopierOptions options)
            : this() {
            this.Option = options;
        }

        public Type Attribute { get; protected set; }

        protected virtual Func<PropertyInfo, bool> MemberFilter () {

            if (Option == CopierOptions.DataMember) {
                return p => p != null && p.IsDefined(Attribute, true);

            } else if (Option == CopierOptions.ValueTypes) {
                return p => {
                    if (p == null) return false;
                    var type = p.PropertyType;
                    return type.IsPublic && p.CanWrite &&
                    (type.IsEnum || type.IsPrimitive || type.IsValueType || type == typeof(string));
                };
            }
            return p => false;
        }

        protected static IEqualityComparer comparer = EqualityComparer<object>.Default;
        public bool AreEqual (T source, T target) {

            if (source == null && target == null)
                return true;

            if (source == null || target == null)
                return false;

            bool result = true;

            var sourceType = source.GetType();
            var desttype = target.GetType();
            var memberFilter = MemberFilter();
            cache.AddType(desttype, memberFilter);
            var dataMembers = cache.Members(sourceType, memberFilter);

            foreach (var info in dataMembers) {
                var sourceValue = info.GetValue(source, null);
                if (cache.ValidMember(desttype, info.PropertyType, info.Name)) {
                    var destValue = cache.GetValue(desttype, info.PropertyType, info.Name, target);
                    result = result && comparer.Equals(sourceValue, destValue);
                }
                if (!result)
                    break;
            }

            return result;
        }

        protected static readonly Type _thisGenericParameter = typeof(T);
        protected static readonly MemberReflectionCache _dataMemberCache = new MemberReflectionCache();
        protected static readonly MemberReflectionCache _valueTypeCache = new MemberReflectionCache();
        protected MemberReflectionCache cache {
            get {
                if (this.Option == CopierOptions.DataMember)
                    return _dataMemberCache;
                else
                    return _valueTypeCache;
            }
        }

        protected static readonly IDictionary<int, Delegate> copyActionCache = new Dictionary<int, Delegate>();

        public T Copy (T source, T sink) {
            if (source == null || sink == null) {
                return sink;
            }
            var clazz = typeof(T);
            var sourceType = source.GetType();
            var sinkType = sink.GetType();

            var key = KeyMaker.GetHashCode(clazz, sourceType, sinkType);
            //var delegateType = typeof(Action<,>).MakeGenericType(sourceType, sinkType);

            Delegate copyAction = null;

            if (!copyActionCache.TryGetValue(key, out copyAction)) {
                var delegateType = typeof (Action<,>).MakeGenericType (sourceType, sinkType);
                var sourceExpr = Expression.Variable(sourceType, "source");
                var destExpr = Expression.Variable(sinkType, "sink");

                var memberFilter = MemberFilter();
                cache.AddType(sinkType, memberFilter);
                var sourceMembers = cache.Members(sourceType, memberFilter);

                var paras = new ParameterExpression[] { sourceExpr, destExpr };
                var blockExpressions = new List<Expression>();
                try {
                    foreach (var sourceInfo in sourceMembers) {
                        if (cache.ValidMember(sinkType, sourceInfo.PropertyType, sourceInfo.Name)) {
                            var sourceMember = Expression.Property(sourceExpr, sourceInfo);
                            var destInfo = sinkType.GetProperty(sourceInfo.Name, sourceInfo.PropertyType);
                            var destMember = Expression.Property(destExpr, destInfo);
                            blockExpressions.Add(Expression.Assign(destMember, sourceMember));
                        }
                    }

                    var copyExpression = Expression.Lambda(delegateType, Expression.Block(blockExpressions), paras);
                    copyAction = copyExpression.Compile();

                } catch (Exception ex) {
                    // Trace.WriteLine(this.GetType().Name + "-Error. Fallback to Copy3");
                    // return Copy3(source, sink);
                    throw ex;
                }
                copyActionCache.Add(key, copyAction);
            }
            copyAction.DynamicInvoke (new object[] { source, sink });
            // this is slower:
            //delegateType.InvokeMember("Invoke",
             //BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod,
             //null, copyAction, new object[] { source, sink });

            return sink;
        }

        public IEnumerable<PropertyInfo> Difference (T source, T target) {
            if (source == null || target == null)
                yield break;

            var sourceType = typeof(T);
            var desttype = typeof(T);
            var memberFilter = MemberFilter();
            cache.AddType(desttype, memberFilter);
            var dataMembers = cache.Members(sourceType, memberFilter);

            foreach (var info in dataMembers) {
                var sourceValue = info.GetValue(source, null);
                if (cache.ValidMember(desttype, info.PropertyType, info.Name)) {
                    var destValue = cache.GetValue(desttype, info.PropertyType, info.Name, target);
                    if (!comparer.Equals(sourceValue, destValue))
                        yield return info;
                }

            }

        }

    }


    public enum CopierOptions {
        DataMember,
        ValueTypes,
    }


}