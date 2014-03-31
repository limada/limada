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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Limaki.Common.Linqish {
    /// <summary>
    /// Builds Expressions and extends IQuerables
    /// where the Member-Type of an entity is unknown 
    /// but the Member-Name is known
    /// </summary>
    public class MemberByNameExpressionBuilder {

        #region IQuerable-Builder
        /// <summary>
        /// calls keySelector and with this result
        /// calls OrderBySelectorAndType
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="memberName"></param>
        /// <param name="descending"></param>
        /// <returns></returns>
        public IQueryable<TSource> OrderByMemberName<TSource>(IQueryable<TSource> source, string memberName, bool descending) {
            var selector = KeySelector<TSource>(memberName);
            var querableMethodName = descending ? "OrderByDescending" : "OrderBy";
            return OrderBySelectorAndType(source, selector.Item1, selector.Item2, querableMethodName);
        }

        /// <summary>
        /// extends an IQuerable with an OrderBy-Clause
        /// calls System.Linq.Queryable.OrderBy<TSource, TKey>, which is the same as the extension OrderBy do
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector">the key expression</param>
        /// <param name="keyType">type of the member in the key expressoin</param>
        /// <param name="querableMethodName">OrderBy or OrderByDescending</param>
        /// <returns></returns>
        public IQueryable<TSource> OrderBySelectorAndType<TSource>(IQueryable<TSource> source, Expression keySelector, Type keyType, string querableMethodName) {
            var method = QuerableMethodFromCache(querableMethodName);

            var result = source.Provider.CreateQuery<TSource>(
                ExpressionCall(
                    method.MakeGenericMethod(typeof(TSource), keyType),
                    source.Expression,
                    Expression.Quote(keySelector)));
            return (IOrderedQueryable<TSource>)result;

        }

        protected static Expression ExpressionCall(MethodInfo method, params Expression[] arguments) {
            return Expression.Call(null, method, arguments);
        }

        #endregion

        #region Expression-Builder
        
        /// <summary>
        /// builds an Expression of Type: 
        /// Expression<Func<TSource,TKey>> keySelector = e => e.Member
        /// where e.Member: public property of TSource with Name == memberName
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public Tuple<Expression, Type> KeySelector<TSource>(string memberName) {
            if (!IsValidMemberName<TSource>(memberName))
                throw new ArgumentException(typeof(TSource).FullName+" has no member with Name "+memberName);

            var memberInfo = SourceMemberInfoFromCache<TSource>(memberName); // TSource.Member
            var keyType = memberInfo.PropertyType; // typeof(TKey)

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(TSource), keyType); // Func<TSource, TKey>

            var entity = Expression.Parameter(typeof(TSource), "e"); // e => e ...
            var body = Expression.MakeMemberAccess(entity, memberInfo); // ... e.Member
            var parameters = new ParameterExpression[] { entity }; // e => ...
            Expression keySelector = Expression.Lambda(delegateType, body, parameters); // Expression<Func<TSource, TKey>> keySelector = e => e.Member
            return Tuple.Create(keySelector, keyType);
        }

        /// <summary>
        /// can be used to build OrderBy
        /// calling OrderBySelectorAndType
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Tuple<Expression, Type> KeySelector<TSource, TKey>(IQueryable<TSource> source, Expression<Func<TSource, TKey>> expression) {
            return Tuple.Create((Expression)expression, typeof(TKey));
        }

        protected PropertyInfo GetMemberInfo<TSource>(string memberName) {
            return typeof (TSource).GetProperty(memberName);
        }

        public bool IsValidMemberName<TSource>(string memberName) {
            return SourceMemberInfoFromCache<TSource>(memberName) != null;
        }

        #endregion

        #region Caches

        protected static IDictionary<Tuple<Type, string>, PropertyInfo> SourceMemberInfos = new Dictionary<Tuple<Type, string>, PropertyInfo>();
        protected PropertyInfo SourceMemberInfoFromCache<TSource>(string memberName) {
            PropertyInfo memberInfo = null;
            var cacheKey = Tuple.Create(typeof (TSource), memberName);
            if (!SourceMemberInfos.TryGetValue(cacheKey, out memberInfo)) {
                memberInfo = GetMemberInfo<TSource>(memberName);
                SourceMemberInfos.Add(cacheKey, memberInfo);
            }
            return memberInfo;
        }

        protected static IDictionary<string, MethodInfo> QuerableMethodNames = new Dictionary<string, MethodInfo>();
        protected MethodInfo QuerableMethodFromCache(string methodName) {
            MethodInfo method = null;
            // cache the method:
            if (!QuerableMethodNames.TryGetValue(methodName, out method)) {
                method = typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static).Where(
                    t => t.Name == methodName && t.GetParameters().Count() == 2
                    ).FirstOrDefault();
                QuerableMethodNames.Add(methodName, method);
            }
            return method;
        }
        #endregion

        #region DO NOT USE THIS
        /// <summary>
        /// DO NOT USE THIS!
        /// this is just for exploration and testing of the IQuerable<T>.OrderBy - Extension
        /// its the same as OrderBy
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public IQueryable<TSource> OrderByKey<TSource, TKey>(IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector) {
            return OrderBySelectorAndType(source, keySelector, typeof(TKey), "OrderBy");
        }
        

        #endregion
    }
}
