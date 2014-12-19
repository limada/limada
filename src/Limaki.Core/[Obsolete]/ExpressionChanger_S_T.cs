/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012 - 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Linq.Expressions;
using System.Linq;

namespace Limaki.Common.Linqish {

    [Obsolete("use ExpressionChangerVisit")]
    public class ExpressionChanger<Source, Target> : ExpressionChangerBase
        where Target : Source {

        public ExpressionChanger() {
            _memberReflectionCache.AddType(typeof(Source));
            _memberReflectionCache.AddType(typeof(Target));
        }

        public override Expression Visit (Expression node) {
            return base.Visit (node);
        }

        public virtual Expression<Func<Target, TResult>> Replace<TResult>(Expression<Func<Source, TResult>> source) {
            return Visit(source) as Expression<Func<Target, TResult>>;
        }

        public virtual Expression<Action<Target>> Replace(Expression<Action<Source>> source) {
            return Visit(source) as Expression<Action<Target>>;
        }

        protected override Expression VisitMember(MemberExpression node) {
            var source = node.Member.ReflectedType;
            var target = ChangeGenericArguments(source);
            return VisitMember(node, source, target);
        }

        protected override Type ChangeGenericArguments(Type generic) {
            return base.ChangeGenericArguments(generic, typeof(Source), typeof(Target));
        }

        protected override Expression VisitConstant(ConstantExpression node) {
            return base.VisitConstant(node);
        }

        protected override Expression VisitParameter(ParameterExpression node) {
            // need to change the ParameterExpression if node.Type.IsGenericType and 
            var source = node.Type;
            if (source == typeof (Source) && source != typeof (Target)) {
                return VisitParameter(Expression.Parameter (typeof (Target), node.Name));
            }
            var target = ChangeGenericArguments(source);
            return VisitParameter(node, source, target);
        }

        protected override Expression VisitMethodCall (MethodCallExpression node) {
            if (node.Method.IsGenericMethod) {
                var genericArguments = node.Method.GetGenericArguments ();
                var changed = false;
                for (int ia = 0; ia < genericArguments.Length; ia++) {
                    if (genericArguments[ia] == typeof (Source)) {
                        genericArguments[ia] = typeof (Target);
                        changed = true;
                    }
                }

                if (changed) {
                    var method = node.Method.GetGenericMethodDefinition ().MakeGenericMethod (genericArguments);
                    var args = Visit (node.Arguments, Visit);
                    var exp = Expression.Call (method, args);
                    return base.VisitMethodCall (exp);
                }
            }
            return base.VisitMethodCall (node);
        }
    }

}