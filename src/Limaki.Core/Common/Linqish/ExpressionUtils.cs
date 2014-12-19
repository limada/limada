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
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;

namespace Limaki.Common.Linqish {

    public static class ExpressionUtils {

        public static MemberInfo MemberInfo<T, TMember>(Expression<Func<T, TMember>> exp) {
            var member = exp.Body as MemberExpression;
            if (member != null)
                return member.Member;
            throw new ArgumentException(string.Format("{0} is not a MemberExpression", exp.ToString()));
        }

        public static string MemberName<T, TMember>(Expression<Func<T, TMember>> exp) {
            return MemberInfo(exp).Name;
        }

        public static Expression<Func<T, TMember>> ReplaceBody<T, TMember> (Expression<Func<T, TMember>> exp, Expression replace, bool right) {
            var body = exp.Body as BinaryExpression;
            if (body == null)
                throw new NotSupportedException ("Only BinaryExpressions are supported ");
            if (right)
                body = Expression.MakeBinary (body.NodeType, body.Left, replace);
            else
                body = Expression.MakeBinary (body.NodeType, replace, body.Right);
            return Expression.Lambda<Func<T, TMember>> (body, exp.Parameters);
        }

        /// <summary>
        /// Expression<T> expr = (...) => ...;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static Expression<T> Lambda<T>(Expression<T> expr) {
            return expr;
        }

      
    }
}