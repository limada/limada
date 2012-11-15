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
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Linq.Expressions;
using System.Linq;

namespace Limaki.Common.Linqish {

    public static class CompareExtensions {
        public static Expression<Func<T, bool>> Compare<T, M>(this Expression<Func<T, bool>> self, Expression<Func<T, M>> e, Comparator c, M s)
            where M : IComparable<M> {
            return CompareExpressionBuilder<M>.Compare<T>(e, s, c);
        }

        public static IQueryable<T> Compare<T, M>(this IQueryable<T> self, Expression<Func<T, M>> e, Comparator c, M s)
            where M : IComparable<M> {
            var pred = CompareExpressionBuilder<M>.Compare<T>(e, s, c);
            return Queryable.Where(self, pred);
        }
    }

    public enum Comparator {
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual
    }

    public class CompareExpressionBuilder<M> where M : IComparable<M> {

        protected static Type Clazz = typeof(CompareExpressionBuilder<>).MakeGenericType(typeof(M));

        public static bool GreaterThan(M a, M b) {
            return a.CompareTo(b) > 0;
        }

        public static bool GreaterThanOrEqual(M a, M b) {
            return a.CompareTo(b) >= 0;
        }

        public static bool LessThan(M a, M b) {
            return a.CompareTo(b) < 0;
        }

        public static bool LessThanOrEqual(M a, M b) {
            return a.CompareTo(b) <= 0;
        }

        public static Expression<Func<T, bool>> Compare<T>(Expression<Func<T, M>> a, M b, Comparator type) {
            var method = Clazz
                .GetMethod(type.ToString(), new Type[] { typeof(M), typeof(M) });

            if (method == null)
                throw new NotImplementedException(type.ToString() + " is not supported in " + Clazz.Name);

            var exType = ExpressionType.Negate;
            if (type == Comparator.GreaterThan)
                exType = ExpressionType.GreaterThan;
            else if (type == Comparator.GreaterThanOrEqual)
                exType = ExpressionType.GreaterThanOrEqual;
            else if (type == Comparator.LessThan)
                exType = ExpressionType.LessThan;
            else if (type == Comparator.LessThanOrEqual)
                exType = ExpressionType.LessThanOrEqual;

            var gt = Expression.MakeBinary(exType, a.Body, Expression.Constant(b), false,
                                               method
                );

            return Expression.Lambda<Func<T, bool>>(gt, a.Parameters);
        }
    }
}