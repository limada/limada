/* This file is part of the db4o object database http://www.db4o.com

Copyright (C) 2004 - 2010  Versant Corporation http://www.versant.com

db4o is free software; you can redistribute it and/or modify it under
the terms of version 3 of the GNU General Public License as published
by the Free Software Foundation.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program.  If not, see http://www.gnu.org/licenses/. */
using Db4objects.Db4o.Linq.Internals;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Linq
{
	public static class ObjectContainerExtensions
	{
		/// <summary>
		/// This is the entry point of Linq to db4o.
		/// It allows the compiler to call the standard query operators
		/// in <see cref="Db4objects.Db4o.Linq.IDb4oLinqQuery">IDb4oLinqQuery</see>. The optimized methods are defined as extension methods
		/// on the <see cref="Db4objects.Db4o.Linq.IDb4oLinqQuery">IDb4oLinqQuery</see> marker interface.
		/// </summary>
		/// <typeparam name="T">The type to query the ObjectContainer</typeparam>
		/// <param name="self">A query factory (any IObjectContainer implementation)</param>
		/// <returns>A <see cref="Db4objects.Db4o.Linq.IDb4oLinqQuery">IDb4oLinqQuery</see> marker interface</returns>
		public static IDb4oLinqQuery<T> Cast<T>(this ISodaQueryFactory queryFactory)
		{
			if (typeof(T) == typeof(object)) return new PlaceHolderQuery<T>(queryFactory);
			return new Db4oQuery<T>(queryFactory);
		}

#if !CF_3_5
		public static IDb4oLinqQueryable<T> AsQueryable<T>(this ISodaQueryFactory queryFactory)
		{
			return queryFactory.Cast<T>().AsQueryable();
		}
#endif
	}
}
