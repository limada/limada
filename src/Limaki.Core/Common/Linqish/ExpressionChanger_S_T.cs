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
using System.Linq.Expressions;

namespace Limaki.Common.Linqish {

    public class ExpressionChanger<Source, Target> : ExpressionChangerBase
        where Target : Source {

        public ExpressionChanger() {
            _memberReflectionCache.AddType(typeof(Source));
            _memberReflectionCache.AddType(typeof(Target));
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
            var target = ChangeGenericArguments(source);
            return VisitParameter(node, source, target);
        }
    }

}