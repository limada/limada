/*
 * Limaki 
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
using System.Linq.Expressions;
using System.Reflection;
using Limaki.Common.Reflections;

namespace Limaki.Common.Linqish {

    public class ExpressionChangerVisit {

        public ExpressionVisitVisitor Visitor { get; set; }

        public static ExpressionChangerVisit Attach<S, T> (ExpressionVisitVisitor visitor) {
            return new ExpressionChangerVisit (visitor, typeof (S), typeof (T));
        }

        public ExpressionChangerVisit (ExpressionVisitVisitor visitor, Type source, Type sink) {
            this.Visitor = visitor;
            this.Source = source;
            this.Sink = sink;
            visitor.VisitLambdaFunc += VisitLambda;
            visitor.VisitMemberFunc += VisitMember;
            visitor.VisitParameterFunc += VisitParameter;
            visitor.VisitMethodCallFunc += VisitMethodCall;
        }

        public Type Source { get; protected set; }
        public Type Sink { get; protected set; }

        protected virtual Expression VisitLambda (LambdaExpression node, Func<LambdaExpression, Expression> super) {
            
            var source = node.GetType().GetGenericArguments().First();
            var target = ChangeGenericArguments (source);
          
            if (source != target) {
                var body = Visitor.Visit (node.Body);
                var paras = node.Parameters.Select (p => Visitor.Visit (p) as ParameterExpression);
                var result = Expression.Lambda (target, body, paras);
                return result;
            } 
            return node;

        }

        protected virtual Expression VisitMember (MemberExpression node, Func<MemberExpression, Expression> super) {
            var source = node.Member.ReflectedType;
            var target = ChangeGenericArguments (source);
            return VisitMember (node, source, target);
        }

        protected virtual Expression VisitParameter (ParameterExpression node, Func<ParameterExpression, Expression> super) {
            // need to change the ParameterExpression if node.Type.IsGenericType and 
            var source = node.Type;
            if (source == Source && source != Sink) {
                return VisitParameter (Expression.Parameter (Sink, node.Name), super);
            }
            var target = ChangeGenericArguments (source);
            return VisitParameter (node, source, target);

        }

        protected virtual Expression VisitMethodCall (MethodCallExpression node, Func<MethodCallExpression, Expression> super) {
            if (node.Method.IsGenericMethod) {
                var genericArguments = node.Method.GetGenericArguments ();
                var changed = false;
                for (int ia = 0; ia < genericArguments.Length; ia++) {
                    if (genericArguments[ia] == Source) {
                        genericArguments[ia] = Sink;
                        changed = true;
                    }
                }

                if (changed) {
                    var method = node.Method.GetGenericMethodDefinition ().MakeGenericMethod (genericArguments);
                    var args = ExpressionVisitor.Visit (node.Arguments, Visitor.Visit);
                    var exp = Expression.Call (method, args);
                    return super(exp);
                }
            }
            return node;
        }

        protected virtual Expression VisitMember (MemberExpression node, Type source, Type target) {

            if (node.Member.ReflectedType == source) {

                //System.Reflection.FieldInfo
                //System.Reflection.MethodBase
                //System.Reflection.PropertyInfo
                // Use the Field, Property or PropertyOrField factory methods to create a MemberExpression

                if (node.Member is PropertyInfo) {
                    var info = node.Member as PropertyInfo;
                    var propertyType = ChangeGenericArguments (info.PropertyType);
                    info = GetPropertyInfo (target, propertyType, info.Name);
                    var ex = Visitor.Visit (node.Expression);
                    return Expression.MakeMemberAccess (ex, info);
                    //return Expression.Property(ex,info);
                }

                if (node.Member is FieldInfo) {
                    var info = node.Member as FieldInfo;
                    var propertyType = ChangeGenericArguments (info.FieldType);
                    info = target.GetField (info.Name);//GetPropertyInfo (target, propertyType, info.Name);
                    var ex = Visitor.Visit (node.Expression);
                    return Expression.MakeMemberAccess (ex, info);
                }

            }
            return Visitor.Visit (node);
        }

        protected virtual Expression VisitParameter (ParameterExpression node, Type source, Type target) {
            if (node.Type == source)
                return GetParameter (target, node.Name);
            return Visitor.Visit (node);
        }

        protected IDictionary<int, ParameterExpression> _params = new Dictionary<int, ParameterExpression> ();
        protected virtual ParameterExpression GetParameter (Type type, string name) {
            var key = type.GetHashCode () ^ name.GetHashCode ();
            ParameterExpression result = null;
            if (!_params.TryGetValue (key, out result)) {
                result = Expression.Parameter (type, name);
                _params[key] = result;
            }
            return result;
        }

        protected Type ChangeGenericArguments (Type generic, Type source, Type target) {
            if (generic == source)
                return target;

            if (generic.IsGenericType) {
                var argsSource = generic.GetGenericArguments ();
                if (!Array.Exists (argsSource, t => t == source))
                    return generic;

                var args = argsSource.Select (t => t == source ? target : t).ToArray ();
                var gen = generic.GetGenericTypeDefinition ();

                var result = gen.MakeGenericType (args);
                return result;
            }

            if (generic.IsArray && generic.GetElementType () == source) {
                return target.MakeArrayType ();
            }
   
            return generic;
        }

        protected virtual Type ChangeGenericArguments (Type generic) {
            return ChangeGenericArguments (generic, Source, Sink);
        }
        
        protected MemberReflectionCache _memberReflectionCache = new MemberReflectionCache ();
        protected PropertyInfo GetPropertyInfo (Type target, Type propertyType, string memberName) {
            if (!_memberReflectionCache.ValidMember (target, propertyType, memberName))
                _memberReflectionCache.AddType (target);

            return _memberReflectionCache.GetPropertyInfo (target, propertyType, memberName);
        }
 
    }
}