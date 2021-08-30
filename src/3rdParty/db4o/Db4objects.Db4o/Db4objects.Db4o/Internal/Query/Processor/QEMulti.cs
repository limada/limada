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
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <exclude></exclude>
	public class QEMulti : QE
	{
		private Collection4 i_evaluators = new Collection4();

		// used by .net LINQ tests
		public virtual IEnumerable Evaluators()
		{
			return i_evaluators;
		}

		internal override QE Add(QE evaluator)
		{
			i_evaluators.Ensure(evaluator);
			return this;
		}

		public override bool Identity()
		{
			bool ret = false;
			IEnumerator i = i_evaluators.GetEnumerator();
			while (i.MoveNext())
			{
				if (((QE)i.Current).Identity())
				{
					ret = true;
				}
				else
				{
					return false;
				}
			}
			return ret;
		}

		internal override bool IsDefault()
		{
			return false;
		}

		internal override bool Evaluate(QConObject a_constraint, QCandidate a_candidate, 
			object a_value)
		{
			IEnumerator i = i_evaluators.GetEnumerator();
			while (i.MoveNext())
			{
				if (((QE)i.Current).Evaluate(a_constraint, a_candidate, a_value))
				{
					return true;
				}
			}
			return false;
		}

		public override void IndexBitMap(bool[] bits)
		{
			IEnumerator i = i_evaluators.GetEnumerator();
			while (i.MoveNext())
			{
				((QE)i.Current).IndexBitMap(bits);
			}
		}

		public override bool SupportsIndex()
		{
			IEnumerator i = i_evaluators.GetEnumerator();
			while (i.MoveNext())
			{
				if (!((QE)i.Current).SupportsIndex())
				{
					return false;
				}
			}
			return true;
		}
	}
}
