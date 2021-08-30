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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Linq.Internals
{
	internal class Db4oQuery<T> : IDb4oLinqQueryInternal<T>
	{
		private readonly ISodaQueryFactory _queryFactory;
		private readonly IQueryBuilderRecord _record;

		public Db4oQuery(ISodaQueryFactory queryFactory)
		{
			if (queryFactory == null) throw new ArgumentNullException("queryFactory");
			_queryFactory = queryFactory;
			_record = NullQueryBuilderRecord.Instance;
		}

		public Db4oQuery(Db4oQuery<T> parent, IQueryBuilderRecord record)
		{
			_queryFactory = parent.QueryFactory;
			_record = new CompositeQueryBuilderRecord(parent.Record, record);
		}

		public ISodaQueryFactory QueryFactory
		{
			get { return _queryFactory; }
		}

		public IQueryBuilderRecord Record
		{
			get { return _record; }
		}

		public int Count
		{
			get { return Execute().Count; }
		}

		public ObjectSetWrapper<T> GetExtentResult()
		{
			var query = NewQuery();
			return Wrap(ExecuteQuery(query, MonitorUnoptimizedQuery));
		}

		private IObjectSet Execute()
		{
			var query = NewQuery();
			_record.Playback(query);
			return ExecuteQuery(query, MonitorOptimizedQuery);
		}

		private static IObjectSet ExecuteQuery(IQuery query, Action4 monitorAction)
		{
			var result = query.Execute();
			((IInternalQuery)query).Container.WithEnvironment(monitorAction);
			return result;
		}

		private void MonitorOptimizedQuery()
		{
			My<ILinqQueryMonitor>.Instance.OnOptimizedQuery();
		}

		private void MonitorUnoptimizedQuery()
		{
			My<ILinqQueryMonitor>.Instance.OnUnoptimizedQuery();
		}

		private IQuery NewQuery()
		{
			var query = _queryFactory.Query();
			query.Constrain(typeof(T));
			return query;
		}

		static ObjectSetWrapper<T> Wrap(IObjectSet set)
		{
			return new ObjectSetWrapper<T>(set);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return Wrap(Execute()).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#region IDb4oLinqQueryInternal<T> Members

		public IEnumerable<T> UnoptimizedThenBy<TKey>(Func<T, TKey> function)
		{
			throw new NotImplementedException("cannot fallback on UnoptimizedThenBy");
		}

		public IEnumerable<T> UnoptimizedThenByDescending<TKey>(Func<T, TKey> function)
		{
			throw new NotImplementedException("cannot fallback on UnoptimizedThenBy");
			/*
			IOrderByRecord record = _orderByRecord;
			IOrderedEnumerable<T> ordered = record.OrderBy(this);

			record = record.Next;
			while (record != null)
			{
				ordered = record.ThenBy(record);
			}
			return ordered.ThenByDescending(function);
			 * */
		}

		public IEnumerable<T> UnoptimizedWhere(Func<T, bool> func)
		{
			return GetExtentResult().Where(func);
		}

		#endregion
	}
}
