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
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.CS.Internal.Objectexchange;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Internal.Query.Result;
using Sharpen.Lang;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public sealed class MQueryExecute : MsgQuery, IMessageWithResponse
	{
		public Msg ReplyFromServer()
		{
			Unmarshall(_payLoad._offset);
			ObjectByRef result = new ObjectByRef();
			Container().WithTransaction(Transaction(), new _IRunnable_15(this, result));
			return ((Msg)result.value);
		}

		private sealed class _IRunnable_15 : IRunnable
		{
			public _IRunnable_15(MQueryExecute _enclosing, ObjectByRef result)
			{
				this._enclosing = _enclosing;
				this.result = result;
			}

			public void Run()
			{
				QQuery query = this._enclosing.UnmarshallQuery();
				result.value = this._enclosing.WriteQueryResult(this._enclosing.ExecuteFully(query
					), query.EvaluationMode(), new ObjectExchangeConfiguration(query.PrefetchDepth()
					, query.PrefetchCount()));
			}

			private readonly MQueryExecute _enclosing;

			private readonly ObjectByRef result;
		}

		private QQuery UnmarshallQuery()
		{
			// TODO: The following used to run outside of the
			// Synchronization block for better performance but
			// produced inconsistent results, cause unknown.
			QQuery query = (QQuery)ReadObjectFromPayLoad();
			query.Unmarshall(Transaction());
			return query;
		}

		private AbstractQueryResult ExecuteFully(QQuery query)
		{
			return ((AbstractQueryResult)query.TriggeringQueryEvents(new _IClosure4_35(this, 
				query)));
		}

		private sealed class _IClosure4_35 : IClosure4
		{
			public _IClosure4_35(MQueryExecute _enclosing, QQuery query)
			{
				this._enclosing = _enclosing;
				this.query = query;
			}

			public object Run()
			{
				AbstractQueryResult qr = this._enclosing.NewQueryResult(query.EvaluationMode());
				qr.LoadFromQuery(query);
				return qr;
			}

			private readonly MQueryExecute _enclosing;

			private readonly QQuery query;
		}
	}
}
