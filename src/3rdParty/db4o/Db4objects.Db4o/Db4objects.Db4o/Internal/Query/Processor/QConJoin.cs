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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <summary>Join constraint on queries</summary>
	/// <exclude></exclude>
	public class QConJoin : QCon
	{
		private bool i_and;

		private QCon i_constraint1;

		private QCon i_constraint2;

		public QConJoin()
		{
		}

		internal QConJoin(Transaction a_trans, QCon a_c1, QCon a_c2, bool a_and) : base(a_trans
			)
		{
			// FIELDS MUST BE PUBLIC TO BE REFLECTED ON UNDER JDK <= 1.1
			// C/S
			i_constraint1 = a_c1;
			i_constraint2 = a_c2;
			i_and = a_and;
		}

		public virtual QCon Constraint2()
		{
			return i_constraint2;
		}

		public virtual QCon Constraint1()
		{
			return i_constraint1;
		}

		internal override void DoNotInclude(QCandidate a_root)
		{
			Constraint1().DoNotInclude(a_root);
			Constraint2().DoNotInclude(a_root);
		}

		internal override void ExchangeConstraint(QCon a_exchange, QCon a_with)
		{
			base.ExchangeConstraint(a_exchange, a_with);
			if (a_exchange == Constraint1())
			{
				i_constraint1 = a_with;
			}
			if (a_exchange == Constraint2())
			{
				i_constraint2 = a_with;
			}
		}

		internal virtual void EvaluatePending(QCandidate a_root, QPending a_pending, int 
			a_secondResult)
		{
			bool res = i_evaluator.Not(i_and ? ((a_pending._result + a_secondResult) > 0) : (
				a_pending._result + a_secondResult) > QPending.False);
			if (HasJoins())
			{
				IEnumerator i = IterateJoins();
				while (i.MoveNext())
				{
					Db4objects.Db4o.Internal.Query.Processor.QConJoin qcj = (Db4objects.Db4o.Internal.Query.Processor.QConJoin
						)i.Current;
					a_root.Evaluate(new QPending(qcj, this, res));
				}
			}
			else
			{
				if (!res)
				{
					Constraint1().DoNotInclude(a_root);
					Constraint2().DoNotInclude(a_root);
				}
			}
		}

		public virtual QCon GetOtherConstraint(QCon a_constraint)
		{
			if (a_constraint == Constraint1())
			{
				return Constraint2();
			}
			else
			{
				if (a_constraint == Constraint2())
				{
					return Constraint1();
				}
			}
			throw new ArgumentException();
		}

		internal override string LogObject()
		{
			return string.Empty;
		}

		internal virtual bool RemoveForParent(QCon a_constraint)
		{
			if (i_and)
			{
				QCon other = GetOtherConstraint(a_constraint);
				other.RemoveJoin(this);
				// prevents circular call
				other.Remove();
				return true;
			}
			return false;
		}

		public override string ToString()
		{
			string str = "QConJoin " + (i_and ? "AND " : "OR");
			if (Constraint1() != null)
			{
				str += "\n   " + Constraint1();
			}
			if (Constraint2() != null)
			{
				str += "\n   " + Constraint2();
			}
			return str;
		}

		public virtual bool IsOr()
		{
			return !i_and;
		}

		public override void SetProcessedByIndex()
		{
			if (ProcessedByIndex())
			{
				return;
			}
			base.SetProcessedByIndex();
			Constraint1().SetProcessedByIndex();
			Constraint2().SetProcessedByIndex();
		}
	}
}
