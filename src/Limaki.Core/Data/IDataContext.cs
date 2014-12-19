/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010 - 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Limaki.Data {

    public interface IDataContext : IDisposable {

        IGateway Gateway { get; set; }

        TextWriter Log { get; set; }

        IQueryable<T> GetQuery<T> ();

        void SetQuery<T> (IQueryable<T> querable);

        void Upsert<T> (IEnumerable<T> entities);

        void Remove<T> (IEnumerable<T> entities);

        void Remove<T> (Expression<Func<T,bool>> where);

    }

    public static class ContextExtensions {

        #region FuncHandling

        public static Func<TContext, IQueryable<TEntity>> QueryableFunc<TContext, TEntity> () {
            Func<TContext, IQueryable<TEntity>> result = null;
            var type = typeof (TContext);

            var dataMembers = type.GetProperties ()
                .Where (p =>
                       p.PropertyType.IsInstanceOfType (typeof (IQueryable<TEntity>)) ||
                       p.PropertyType == typeof (IQueryable<TEntity>)
                )
                .FirstOrDefault ();

            if (dataMembers != null) {
                result = c => dataMembers.GetValue (c, null) as IQueryable<TEntity>;
            }
            return result;
        }

        public static IEnumerable<Type> QueryableTypes<TEntity> () {
            return QueryableProperties<TEntity> ().Select (p => p.PropertyType);
        }

        public static IEnumerable<PropertyInfo> QueryableProperties<TEntity> () {
            var genType = typeof (IQueryable<>).GetGenericTypeDefinition ();
            foreach (var prop in typeof (TEntity).GetProperties ()) {
                if (prop.PropertyType.IsGenericType) {
                    var gettype = prop.PropertyType.GetGenericTypeDefinition ();
                    if (gettype.Equals (genType)) {
                        yield return prop;
                    }
                }
            }
        }

        #endregion
    }
}