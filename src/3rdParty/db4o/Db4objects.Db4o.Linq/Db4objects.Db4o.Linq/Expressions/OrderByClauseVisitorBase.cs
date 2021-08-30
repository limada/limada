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
using System.Linq.Expressions;
using Db4objects.Db4o.Linq.Internals;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Linq.Expressions
{
	internal abstract class OrderByClauseVisitorBase : ExpressionQueryBuilder
	{
		protected abstract void ApplyDirection(IQuery query);

		protected override void VisitMethodCall(MethodCallExpression methodCall)
		{
			Visit(methodCall.Object);
			AnalyseMethod(Recorder, methodCall.Method);
		}

		protected override void VisitMemberAccess(MemberExpression m)
		{
			ProcessMemberAccess(m);
		}

		public override IQueryBuilderRecord Process(LambdaExpression expression)
		{
			if (!StartsWithParameterReference(expression.Body))
				CannotOptimize(expression.Body);

			return ApplyDirection(base.Process(expression));
		}

		private IQueryBuilderRecord ApplyDirection(IQueryBuilderRecord record)
		{
			QueryBuilderRecorder recorder = new QueryBuilderRecorder(record);
			recorder.Add(ctx => ApplyDirection(ctx.CurrentQuery));
			return recorder.Record;
		}
	}
}
