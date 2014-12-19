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

namespace Limaki.Common.Linqish {

    public class ExpressionChanger<T> : ExpressionVisitor {
        public virtual Expression<E> Replace<E>(Expression<E> node) {
            var result = Visit(node);
            return result as Expression<E>;
        }

        public override Expression Visit(Expression node) {
            return base.Visit(node);
        }

        protected override Expression VisitMember(MemberExpression node) {
            var map = FindSource(node);
            if (map != null) {
                return map.Item2.Body;
            }
            return base.VisitMember(node);
        }

        protected override Expression VisitBinary(BinaryExpression node) {
            // todo: convert-target could be right too!! 
            var left = Visit(node.Left);
            var right = Visit(node.Right);
            if (left != node.Left && left is MemberExpression) {
                var map = FindTarget(left);
                if (map != null) {
                    right = node.Right;
                    if (((MemberExpression)map.Item1.Body).Type.IsEnum) {
                        right = Expression.Convert(right, (map.Item1.Body as MemberExpression).Type);
                    }
                    right = Expression.Call((map.Item3.Body as MethodCallExpression).Method, right);

                    return Expression.MakeBinary(node.NodeType, left, right);
                }
            }
            return Expression.MakeBinary(node.NodeType, left, right);

        }

        protected override Expression VisitUnary(UnaryExpression node) {
            if (node.NodeType == ExpressionType.Convert) {
                var map = FindSource(node.Operand);
                if (map != null) {
                    return map.Item2.Body;
                }
            }
            return base.VisitUnary(node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node) {
            var r = Visit(node.Body);
            if (r != node.Body) {
                return Expression.Lambda(r, node.Parameters);
            }
            return base.VisitLambda<T>(node);
        }


        protected ICollection<Tuple<LambdaExpression, LambdaExpression, LambdaExpression>> _mappings =
            new List<Tuple<LambdaExpression, LambdaExpression, LambdaExpression>>();

        public Tuple<LambdaExpression, LambdaExpression, LambdaExpression> Add<R1, R2>(
            Expression<Func<T, R1>> source, Expression<Func<T, R2>> target, Expression<Func<R1, R2>> convert) {
            
            var t = Tuple.Create((LambdaExpression)source, (LambdaExpression)target, (LambdaExpression)convert);
            _mappings.Add(t);
            return t;
        }

        protected bool CompareMemberExpression(MemberExpression a, MemberExpression b) {
            return a.Type == b.Type && a.Member.Name == b.Member.Name ;
                //&& a.Member.DeclaringType == b.Member.DeclaringType;
        }

        public virtual Tuple<LambdaExpression, LambdaExpression, LambdaExpression> FindSource(Expression value) {
            var member = value as MemberExpression;
            if (member != null) {
                return _mappings.FirstOrDefault(l => CompareMemberExpression((MemberExpression)l.Item1.Body, member));
            }
            return null;
        }

       
        public virtual Tuple<LambdaExpression, LambdaExpression, LambdaExpression> FindTarget(Expression value) {
            var member = value as MemberExpression;
            if (member != null) {
                return _mappings.FirstOrDefault(l => CompareMemberExpression((MemberExpression)l.Item2.Body, member));
            }
            return null;
        }
    }
}