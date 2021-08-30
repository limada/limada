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
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Qlin;
using Db4objects.Db4o.Internal.Query;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Internal.Query.Result;
using Db4objects.Db4o.Qlin;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Internal.Qlin
{
	/// <exclude></exclude>
	public class QLinRoot : QLinSodaNode
	{
		private readonly QQuery _query;

		private int _limit = -1;

		public QLinRoot(IQuery query, Type clazz)
		{
			_query = (QQuery)query;
			query.Constrain(clazz);
			QLinSupport.Context(clazz);
		}

		public virtual IQuery Query()
		{
			return _query;
		}

		public override IObjectSet Select()
		{
			if (_limit == -1)
			{
				return _query.Execute();
			}
			IQueryResult queryResult = _query.GetQueryResult();
			IdListQueryResult limitedResult = new IdListQueryResult(_query.Transaction(), _limit
				);
			int counter = 0;
			IIntIterator4 i = queryResult.IterateIDs();
			while (i.MoveNext())
			{
				if (counter++ >= _limit)
				{
					break;
				}
				limitedResult.Add(i.CurrentInt());
			}
			return new ObjectSetFacade(limitedResult);
		}

		public override IQLin Limit(int size)
		{
			if (size < 1)
			{
				throw new QLinException("Limit must be greater that 0");
			}
			_limit = size;
			return this;
		}

		protected override Db4objects.Db4o.Internal.Qlin.QLinRoot Root()
		{
			return this;
		}

		internal virtual IQuery Descend(object expression)
		{
			// TODO: Implement deep descend
			return Query().Descend(QLinSupport.Field(expression).GetName());
		}
	}
}
