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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Db4objects.Db4o.Linq.Expressions;
using Db4objects.Db4o.Linq.Internals;

namespace Db4objects.Db4o.Linq
{
	/// <summary>
	/// A class that defines some standard linq query operations
	/// that can be optimized.
	/// </summary>
	public static class Db4oLinqQueryExtensions
	{
		public static IDb4oLinqQuery<TSource> Where<TSource>(this IDb4oLinqQuery<TSource> self, Expression<Func<TSource, bool>> expression)
		{
			return Process(self,
				query => new WhereClauseVisitor().Process(expression),
				data => data.UnoptimizedWhere(expression.Compile())
			);
		}

		public static int Count<TSource>(this IDb4oLinqQuery<TSource> self)
		{
			if (self == null)
				throw new ArgumentNullException("self");

			var query = self as Db4oQuery<TSource>;
			if (query != null)
				return query.Count;

			return Enumerable.Count(self);
		}

		private static IDb4oLinqQuery<TSource> Process<TSource>(
			IDb4oLinqQuery<TSource> query,
			Func<Db4oQuery<TSource>, IQueryBuilderRecord> queryProcessor,
			Func<IDb4oLinqQueryInternal<TSource>, IEnumerable<TSource>> fallbackProcessor)
		{
			if (query == null)
			{
				throw new ArgumentNullException("query");
			}

			var candidate = query as Db4oQuery<TSource>;

			if (candidate == null)
			{
				return new UnoptimizedQuery<TSource>(fallbackProcessor((IDb4oLinqQueryInternal<TSource>) EnsureDb4oQuery(query)));
			}
			try
			{
				IQueryBuilderRecord record = queryProcessor(candidate);
				return new Db4oQuery<TSource>(candidate, record);
			}
			catch (QueryOptimizationException)
			{
				return new UnoptimizedQuery<TSource>(fallbackProcessor(candidate));
			}
		}

		private static IDb4oLinqQuery<TSource> EnsureDb4oQuery<TSource>(IDb4oLinqQuery<TSource> query)
		{
			var placeHolderQuery = query as PlaceHolderQuery<TSource>;
			if (placeHolderQuery == null)
			{
				return query;
			}

			return new Db4oQuery<TSource>(placeHolderQuery.QueryFactory);
		}

		private static IDb4oLinqQuery<TSource> ProcessOrderBy<TSource, TKey>(
			IDb4oLinqQuery<TSource> query,
			OrderByClauseVisitorBase visitor,
			Expression<Func<TSource, TKey>> expression,
			Func<IDb4oLinqQueryInternal<TSource>, IEnumerable<TSource>> fallbackProcessor)
		{
			return Process(query, q => visitor.Process(expression), fallbackProcessor);
		}

		public static IDb4oLinqQuery<TSource> OrderBy<TSource, TKey>(this IDb4oLinqQuery<TSource> self, Expression<Func<TSource, TKey>> expression)
		{
			return ProcessOrderBy(self, new OrderByAscendingClauseVisitor(), expression,
				data => data.OrderBy(expression.Compile())
			);
		}

		public static IDb4oLinqQuery<TSource> OrderByDescending<TSource, TKey>(this IDb4oLinqQuery<TSource> self, Expression<Func<TSource, TKey>> expression)
		{
			return ProcessOrderBy(self, new OrderByDescendingClauseVisitor(), expression,
				data => data.OrderByDescending(expression.Compile())
			);
		}

		public static IDb4oLinqQuery<TSource> ThenBy<TSource, TKey>(this IDb4oLinqQuery<TSource> self, Expression<Func<TSource, TKey>> expression)
		{
			return ProcessOrderBy(self, new OrderByAscendingClauseVisitor(), expression,
				data => data.UnoptimizedThenBy(expression.Compile())
			);
		}

		public static IDb4oLinqQuery<TSource> ThenByDescending<TSource, TKey>(this IDb4oLinqQuery<TSource> self, Expression<Func<TSource, TKey>> expression)
		{
			return ProcessOrderBy(self, new OrderByDescendingClauseVisitor(), expression,
				data => data.UnoptimizedThenByDescending(expression.Compile())
			);
		}

		public static IDb4oLinqQuery<TRet> Select<TSource, TRet>(this IDb4oLinqQuery<TSource> self, Func<TSource, TRet> selector)
		{
			var placeHolderQuery = self as PlaceHolderQuery<TSource>;
			if (placeHolderQuery != null) return new Db4oQuery<TRet>(placeHolderQuery.QueryFactory);

			return new UnoptimizedQuery<TRet>(Enumerable.Select(self, selector));
		}
#if !CF_3_5
		public static IDb4oLinqQueryable<TSource> AsQueryable<TSource>(this IDb4oLinqQuery<TSource> self)
		{
			return new Db4oQueryable<TSource>(self);
		}
#endif
	}
}
