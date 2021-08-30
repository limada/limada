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
	public class ArrayAccessValue : ComparisonOperandDescendant
	{
		private IComparisonOperand _index;

		public ArrayAccessValue(ComparisonOperandDescendant parent, IComparisonOperand index
			) : base(parent)
		{
			_index = index;
		}

		public override void Accept(IComparisonOperandVisitor visitor)
		{
			visitor.Visit(this);
		}

		public virtual IComparisonOperand Index()
		{
			return _index;
		}

		public override bool Equals(object obj)
		{
			if (!base.Equals(obj))
			{
				return false;
			}
			Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand.ArrayAccessValue casted = (Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand.ArrayAccessValue
				)obj;
			return _index.Equals(casted._index);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode() * 29 + _index.GetHashCode();
		}

		public override string ToString()
		{
			return base.ToString() + "[" + _index + "]";
		}

		public override ITypeRef Type
		{
			get
			{
				return ((ComparisonOperandDescendant)Parent()).Type.ElementType;
			}
		}
	}
}
