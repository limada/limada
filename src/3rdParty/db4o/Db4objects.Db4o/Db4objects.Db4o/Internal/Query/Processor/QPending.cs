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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <exclude></exclude>
	internal class QPending : Tree
	{
		internal readonly QConJoin _join;

		internal QCon _constraint;

		internal int _result;

		internal const int False = -4;

		internal const int Both = 1;

		internal const int True = 2;

		internal QPending(QConJoin a_join, QCon a_constraint, bool a_firstResult)
		{
			// Constants, so QConJoin.evaluatePending is made easy:
			_join = a_join;
			_constraint = a_constraint;
			_result = a_firstResult ? True : False;
		}

		public override int Compare(Tree a_to)
		{
			return _constraint.Id() - ((Db4objects.Db4o.Internal.Query.Processor.QPending)a_to
				)._constraint.Id();
		}

		internal virtual void ChangeConstraint()
		{
			_constraint = _join.GetOtherConstraint(_constraint);
		}

		public override object ShallowClone()
		{
			Db4objects.Db4o.Internal.Query.Processor.QPending pending = InternalClonePayload(
				);
			base.ShallowCloneInternal(pending);
			return pending;
		}

		internal virtual Db4objects.Db4o.Internal.Query.Processor.QPending InternalClonePayload
			()
		{
			Db4objects.Db4o.Internal.Query.Processor.QPending pending = new Db4objects.Db4o.Internal.Query.Processor.QPending
				(_join, _constraint, false);
			pending._result = _result;
			return pending;
		}

		public override object Key()
		{
			throw new NotImplementedException();
		}
	}
}
