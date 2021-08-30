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
using System.Linq;
using System.Linq.Expressions;

namespace Limaki.Common.Linqish {

    /// <summary>
    /// projects an  Expression<Func<S, R>>
    /// into an Expression<Func<T, R>>
    /// </summary>
    /// <typeparam name="T">Target-Type</typeparam>
    /// <typeparam name="S">Source-Type</typeparam>
    /// <typeparam name="R">Result-Type</typeparam>
    public class ExpressionProjector<T, S, R> : ExpressionVisitor {

        Expression sourceMember;
        Expression sourceParam;
        ParameterExpression targetParam;
        MemberExpression targetMember = null;
        Type sourceType = typeof(S);
        public Expression<Func<T, R>> Replace(Expression<Func<T, S>> targetAccess, Expression<Func<S, R>> source) {
            this.sourceMember = source.Parameters.First();
            this.sourceParam = source.Parameters.First();
            targetMember = (targetAccess.Body as MemberExpression);
            Visit(targetAccess);
            targetParam = targetAccess.Parameters.First();
            var result = Expression.Lambda<Func<T, R>>(
                Visit(source.Body),
                targetParam);
            return result;
        }

        public Expression<Func<T, R>> Replace<TMember>(Expression<Func<T, TMember>> targetAccess, Expression<Func<S, TMember>> sourceAccess, Expression<Func<S, R>> source) {
            this.sourceParam = source.Parameters.First();
            this.sourceMember = sourceAccess.Body;
            sourceType = typeof(TMember);
            targetMember = (targetAccess.Body as MemberExpression);
            var t1 = Visit(targetAccess);
            targetParam = targetAccess.Parameters.First();
            var result = Expression.Lambda<Func<T, R>>(
                Visit(source.Body),
                targetParam);
            return result;
        }

        class MemberOfParam : ExpressionVisitor {
            Expression sourceParam;
            bool result = false;
            public virtual bool IsMemberOf(Expression sourceParam, Expression source) {

                result = sourceParam.Type == source.Type && sourceParam.NodeType==source.NodeType;
                this.sourceParam = sourceParam;
                if (!result)
                    base.Visit(source);
                return result;
            }
            protected override Expression VisitMember(MemberExpression node) {
                if (node.Expression.Type == sourceParam.Type && node.NodeType == sourceParam.NodeType) {
                    result = true;
                }
                return base.VisitMember(node);
            }
        }

        protected override Expression VisitMember(MemberExpression node) {
            if (node.Type == sourceMember.Type && targetMember == null)
                targetMember = node;
            
            bool isMember = node.Member.ReflectedType == sourceType || sourceType.GetInterfaces().Contains(node.Member.DeclaringType);
            if (isMember && new MemberOfParam().IsMemberOf(sourceMember, node.Expression))
                return Expression.MakeMemberAccess(
                    targetMember,
                    node.Member);
            else if (node.Expression.NodeType == ExpressionType.Parameter && node.Expression.Type == sourceParam.Type && targetParam != null)
                return Expression.MakeMemberAccess(
                    targetParam,
                    targetParam.Type.GetMember(node.Member.Name).First());
            return base.VisitMember(node);
        }


    }

}