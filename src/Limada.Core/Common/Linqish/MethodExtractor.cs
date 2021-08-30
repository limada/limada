/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2015 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Limaki.Common.Linqish {

    public class MethodExtractor : ExpressionVisitor {

        private MethodInfo _member;

        public MethodInfo GetMethodInfo<T, R> (Expression<Func<T, R>> exp) => RunVisits (exp);

        public MethodInfo GetMethodInfo<T> (Expression<Action<T>> exp) => RunVisits (exp);

        public MethodInfo GetMethodInfo<T, P> (Expression<Action<T, P>> exp) => RunVisits (exp);
        public MethodInfo GetMethodInfo<T, P1, P2> (Expression<Action<T, P1, P2>> exp) => RunVisits (exp);
        public MethodInfo GetMethodInfo<T, P1, P2, P3> (Expression<Action<T, P1, P2, P3>> exp) => RunVisits (exp);
        public MethodInfo GetMethodInfo<T, P1, P2, P3, P4> (Expression<Action<T, P1, P2, P3, P4>> exp) => RunVisits (exp);


        protected MethodInfo RunVisits (Expression exp) { 
             _member = null;
            this.Visit (exp);
            return _member;
        }
        protected override Expression VisitMethodCall (MethodCallExpression node) {

            var result = base.VisitMethodCall (node);
            var exp = result as MethodCallExpression;
            if (exp != null)
                _member = exp.Method;
            return result;
        }

        protected override Expression VisitMember (MemberExpression m) {
            var result = base.VisitMember (m);
            var exp = result as MemberExpression;
            if (exp != null && exp.Member.MemberType == MemberTypes.Method) {
                _member = exp.Member as MethodInfo;
            }
            return result;
        }
    }
}