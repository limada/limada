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
 * http://limada.sourceforge.net
 * 
 */

using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Limaki.Common.Linqish {
    /// <summary>
    /// replaces local variables with constants
    /// </summary>
    public class ExpressionConstantBuilder : ExpressionVisitor {

        ICollection<ParameterExpression> sourceParam;
        ParameterExpression param;
        MemberExpression targetMember = null;

        public Expression<T> Replace<T>(Expression<T> source) {
            if (source == null)
                return null;
            this.sourceParam = source.Parameters;
            var result = base.Visit(source);
            return result as Expression<T>;
        }

        protected override Expression VisitMember(MemberExpression node) {
            //if (!sourceParam.Contains(node.Expression as ParameterExpression)) 
            if (IsConstant(node)) {
                var val = ConstantValue(node);
                return Expression.Constant(val, node.Type);
            }
            return base.VisitMember(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node) {
            if ((node.Arguments.Count == 0 && IsConstant(node.Object)) || 
                (node.Object == null && ! node.Method.Attributes.HasFlag(MethodAttributes.Static)))
                return Expression.Constant(ConstantValue(node), node.Type);
            var args = new List<Expression>();
            var hasConsts = false;
            foreach (var arg in node.Arguments) {
                if (IsConstant(arg)) {
                    var val = ConstantValue(arg);
                    args.Add(Expression.Constant(val, arg.Type));
                    hasConsts = true;
                } else {
                    args.Add(arg);
                   
                }
            }
            if (args.Count > 0 && hasConsts) {
                var result = Expression.Call(node.Object,node.Method, args);
                if (IsConstant(result.Object) || (result.Object==null && ! result.Method.Attributes.HasFlag(MethodAttributes.Static)))
                    return Expression.Constant(ConstantValue(result), result.Type);
                return result;
            }
            return base.VisitMethodCall(node);
        }

        class ConstantIs : ExpressionVisitor {
            int constFound = 0;
            public bool HasConstant(Expression ex) {
                constFound = 0;
                Visit(ex);
                return constFound != 0;
            }
            protected override Expression VisitConstant(ConstantExpression node) {
                constFound ++;
                return base.VisitConstant(node);
            }
            protected override Expression VisitMethodCall(MethodCallExpression node) {
                Visit(node.Arguments);
                return base.VisitMethodCall(node);
            }
        }

        bool IsConstant(Expression expression) {
            return new ConstantIs().HasConstant(expression);
        }

        private static object ConstantValue(Expression expression) {
            object value;
            if (!TryGetContantValue(expression, out value)) { // fallback:
                value = Expression.Lambda(expression).Compile().DynamicInvoke();
            }
            return value;
        }

        private static bool TryGetContantValue(Expression expression, out object value) {
            if (expression == null) {   // used for static fields, etc
                value = null;
                return true;
            }
            switch (expression.NodeType) {
                case ExpressionType.Constant:
                    value = ((ConstantExpression)expression).Value;
                    return true;
                case ExpressionType.MemberAccess:
                    MemberExpression memberExpression = (MemberExpression)expression;
                    object target;
                    if (TryGetContantValue(memberExpression.Expression, out target)) {
                        switch (memberExpression.Member.MemberType) {
                            case MemberTypes.Field:
                                value = ((FieldInfo)memberExpression.Member).GetValue(target);
                                return true;
                            case MemberTypes.Property:
                                value = ((PropertyInfo)memberExpression.Member).GetValue(target, null);
                                return true;
                        }
                    }
                    break;
            }
            value = null;
            return false;
        }

    }
}