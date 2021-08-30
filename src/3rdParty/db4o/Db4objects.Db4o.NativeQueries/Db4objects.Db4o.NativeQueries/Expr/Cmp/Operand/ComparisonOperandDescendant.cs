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
using Db4objects.Db4o.Instrumentation.Api;
using Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand;

namespace Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand
{
	public abstract class ComparisonOperandDescendant : IComparisonOperandAnchor
	{
		private IComparisonOperandAnchor _parent;

		protected ComparisonOperandDescendant(IComparisonOperandAnchor _parent)
		{
			this._parent = _parent;
		}

		public IComparisonOperandAnchor Parent()
		{
			return _parent;
		}

		public IComparisonOperandAnchor Root()
		{
			return _parent.Root();
		}

		public abstract ITypeRef Type
		{
			get;
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand.ComparisonOperandDescendant casted
				 = (Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand.ComparisonOperandDescendant)obj;
			return _parent.Equals(casted._parent);
		}

		public override int GetHashCode()
		{
			return _parent.GetHashCode();
		}

		public override string ToString()
		{
			return _parent.ToString();
		}

		public abstract void Accept(IComparisonOperandVisitor arg1);
	}
}
