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
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Linq.Internals
{
	internal interface IQueryBuilderRecord
	{
		void Playback(IQuery query);
		void Playback(QueryBuilderContext context);
	}

	internal class NullQueryBuilderRecord : IQueryBuilderRecord
	{
		public static readonly NullQueryBuilderRecord Instance = new NullQueryBuilderRecord();

		private NullQueryBuilderRecord()
		{
		}

		public void Playback(IQuery query)
		{
		}

		public void Playback(QueryBuilderContext context)
		{
		}
	}

	internal abstract class QueryBuilderRecordImpl : IQueryBuilderRecord
	{
		public void Playback(IQuery query)
		{
			Playback(new QueryBuilderContext(query));
		}

		public abstract void Playback(QueryBuilderContext context);
	}

	internal class CompositeQueryBuilderRecord : QueryBuilderRecordImpl
	{
		private readonly IQueryBuilderRecord _first;
		private readonly IQueryBuilderRecord _second;

		public CompositeQueryBuilderRecord(IQueryBuilderRecord first, IQueryBuilderRecord second)
		{
			_first = first;
			_second = second;
		}

		override public void Playback(QueryBuilderContext context)
		{
			context.SaveQuery();
			_first.Playback(context);
			context.RestoreQuery();

			_second.Playback(context);
		}
	}

	internal class ChainedQueryBuilderRecord : QueryBuilderRecordImpl
	{
		private readonly Action<QueryBuilderContext> _action;
		private readonly IQueryBuilderRecord _next;

		public ChainedQueryBuilderRecord(IQueryBuilderRecord next, Action<QueryBuilderContext> action)
		{
			_next = next;
			_action = action;
		}

		override public void Playback(QueryBuilderContext context)
		{
			_next.Playback(context);
			_action(context);
		}
	}

	internal class QueryBuilderRecorder
	{
		private IQueryBuilderRecord _last = NullQueryBuilderRecord.Instance;

		public QueryBuilderRecorder()
		{
		}

		public QueryBuilderRecorder(IQueryBuilderRecord record)
		{
			_last = record;
		}

		public IQueryBuilderRecord Record
		{
			get { return _last; }
		}

		public void Add(Action<QueryBuilderContext> action)
		{
			_last = new ChainedQueryBuilderRecord(_last, action);
		}
	}
}
