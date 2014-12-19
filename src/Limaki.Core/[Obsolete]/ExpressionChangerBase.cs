/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Limaki.Common;
using Limaki.Common.Reflections;

namespace Limaki.Common.Linqish {

    [Obsolete("use ExpressionChangerVisit")]
    public abstract class ExpressionChangerBase : ExpressionVisitor {

        protected MemberReflectionCache _memberReflectionCache = new MemberReflectionCache();

        protected Type ChangeGenericArguments(Type generic, Type source, Type target) {
            if (generic.IsGenericType) {
                var argsSource = generic.GetGenericArguments();
                if (!Array.Exists(argsSource, t => t == source))
                    return generic;

                var args = argsSource.Select(t => t == source ? target : t).ToArray();
                var gen = generic.GetGenericTypeDefinition();

                var result = gen.MakeGenericType(args);
                return result;
            }
            if (generic.IsArray && generic.GetElementType() == source) {
                return target.MakeArrayType();
            }
            if (generic == source)
                return target;
            return generic;
        }

        private static MethodInfo _genLamda = null;
        protected static MethodInfo GenericLamdaMethod {
            get {
                if (_genLamda == null) {
                    IEnumerable<ParameterExpression> paras = null;
                    Expression<Func<Expression>> method = () => Expression.Lambda<object>(null, paras);
                    _genLamda =
                        ((MethodCallExpression)((LambdaExpression)method).Body).Method.GetGenericMethodDefinition();
                }
                return _genLamda;
            }
        }

        protected MethodInfo ExpressionLamdaMethod(Type type) {
            var result = GenericLamdaMethod.MakeGenericMethod(type);
            return result;
        }

        protected IDictionary<int, ParameterExpression> _params = new Dictionary<int, ParameterExpression>();

        protected virtual ParameterExpression GetParameter(Type type, string name) {
            var key = KeyMaker.GetHashCode(type, name);
            ParameterExpression result = null;
            if (!_params.TryGetValue(key, out result)) {
                result = Expression.Parameter(type, name);
                _params[key] = result;
            }
            return result;
        }

        protected abstract Type ChangeGenericArguments(Type generic);

        protected override Expression VisitLambda<T>(Expression<T> node) {
            Expression result = null;
            var body = Visit(node.Body);
            var paras = node.Parameters.Select(p => VisitParameter(p) as ParameterExpression);
            var source = typeof(T);
            var target = ChangeGenericArguments(source);
            if (source != target) {
                var makeLamda = ExpressionLamdaMethod(target);
                result = makeLamda.Invoke(null, new object[] { body, paras }) as Expression;
            } else {
                result = Expression.Lambda(body, paras);
            }
            return result;

        }

        protected virtual Expression VisitMember(MemberExpression node, Type source, Type target) {

            if (node.Member.ReflectedType == source) {

                //System.Reflection.FieldInfo
                //System.Reflection.MethodBase
                //System.Reflection.PropertyInfo
                // Use the Field, Property or PropertyOrField factory methods to create a MemberExpression

                if (node.Member is PropertyInfo) {
                    var info = node.Member as PropertyInfo;
                    var propertyType = ChangeGenericArguments(info.PropertyType);
                    info = GetPropertyInfo(target, propertyType, info.Name);
                    var ex = Visit(node.Expression);
                    return Expression.MakeMemberAccess(ex, info);
                    //return Expression.Property(ex,info);
                }

                if (node.Member is FieldInfo) {
                    var info = node.Member as FieldInfo;
                    var propertyType = ChangeGenericArguments (info.FieldType);
                    info = target.GetField (info.Name);//GetPropertyInfo (target, propertyType, info.Name);
                    var ex = Visit (node.Expression);
                    return Expression.MakeMemberAccess (ex, info);
                }

            }
            return base.VisitMember(node);
        }

        private PropertyInfo GetPropertyInfo(Type target, Type propertyType, string memberName) {
            if (!_memberReflectionCache.ValidMember(target, propertyType, memberName))
                _memberReflectionCache.AddType(target);
            
            return _memberReflectionCache.GetPropertyInfo(target, propertyType, memberName);
        }

        protected virtual Expression VisitParameter(ParameterExpression node, Type source, Type target) {
            if (node.Type == source)
                return GetParameter(target, node.Name);
            return base.VisitParameter(node);
        }
    }
}