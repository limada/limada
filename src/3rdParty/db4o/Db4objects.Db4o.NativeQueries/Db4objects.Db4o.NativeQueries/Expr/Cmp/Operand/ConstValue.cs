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
using Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand;

namespace Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand
{
	public class ConstValue : IComparisonOperand
	{
		private object _value;

		public ConstValue(object value)
		{
			this._value = value;
		}

		public virtual object Value()
		{
			return _value;
		}

		public virtual void Value(object value)
		{
			_value = value;
		}

		public override string ToString()
		{
			if (_value == null)
			{
				return "null";
			}
			if (_value is string)
			{
				return "\"" + _value + "\"";
			}
			return _value.ToString();
		}

		public override bool Equals(object other)
		{
			if (this == other)
			{
				return true;
			}
			if (other == null || GetType() != other.GetType())
			{
				return false;
			}
			object otherValue = ((Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand.ConstValue)other
				)._value;
			if (otherValue == _value)
			{
				return true;
			}
			if (otherValue == null || _value == null)
			{
				return false;
			}
			return _value.Equals(otherValue);
		}

		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}

		public virtual void Accept(IComparisonOperandVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
