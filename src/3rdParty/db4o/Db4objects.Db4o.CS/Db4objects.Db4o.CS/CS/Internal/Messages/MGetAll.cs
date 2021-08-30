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
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.CS.Internal.Objectexchange;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Internal.Query.Result;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public sealed class MGetAll : MsgQuery, IMessageWithResponse
	{
		public Msg ReplyFromServer()
		{
			QueryEvaluationMode evaluationMode = QueryEvaluationMode.FromInt(ReadInt());
			int prefetchDepth = ReadInt();
			int prefetchCount = ReadInt();
			lock (ContainerLock())
			{
				return WriteQueryResult(GetAll(evaluationMode), evaluationMode, new ObjectExchangeConfiguration
					(prefetchDepth, prefetchCount));
			}
		}

		private AbstractQueryResult GetAll(QueryEvaluationMode mode)
		{
			return ((AbstractQueryResult)NewQuery(mode).TriggeringQueryEvents(new _IClosure4_24
				(this, mode)));
		}

		private sealed class _IClosure4_24 : IClosure4
		{
			public _IClosure4_24(MGetAll _enclosing, QueryEvaluationMode mode)
			{
				this._enclosing = _enclosing;
				this.mode = mode;
			}

			public object Run()
			{
				try
				{
					return this._enclosing.LocalContainer().GetAll(this._enclosing.Transaction(), mode
						);
				}
				catch (Exception e)
				{
				}
				return this._enclosing.NewQueryResult(mode);
			}

			private readonly MGetAll _enclosing;

			private readonly QueryEvaluationMode mode;
		}

		private QQuery NewQuery(QueryEvaluationMode mode)
		{
			QQuery query = (QQuery)LocalContainer().Query();
			query.EvaluationMode(mode);
			return query;
		}
	}
}
