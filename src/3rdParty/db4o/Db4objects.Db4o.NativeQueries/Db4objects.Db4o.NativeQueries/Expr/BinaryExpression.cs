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
using Db4objects.Db4o.NativeQueries.Expr;

namespace Db4objects.Db4o.NativeQueries.Expr
{
	public abstract class BinaryExpression : IExpression
	{
		protected IExpression _left;

		protected IExpression _right;

		public BinaryExpression(IExpression left, IExpression right)
		{
			this._left = left;
			this._right = right;
		}

		public virtual IExpression Left()
		{
			return _left;
		}

		public virtual IExpression Right()
		{
			return _right;
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
			Db4objects.Db4o.NativeQueries.Expr.BinaryExpression casted = (Db4objects.Db4o.NativeQueries.Expr.BinaryExpression
				)other;
			return _left.Equals(casted._left) && (_right.Equals(casted._right)) || _left.Equals
				(casted._right) && (_right.Equals(casted._left));
		}

		public override int GetHashCode()
		{
			return _left.GetHashCode() + _right.GetHashCode();
		}

		public abstract void Accept(IExpressionVisitor arg1);
	}
}
