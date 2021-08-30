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
using Db4objects.Db4o.NativeQueries.Expr;
using Db4objects.Db4o.NativeQueries.Expr.Cmp;
using Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand;

namespace Db4objects.Db4o.NativeQueries.Expr
{
	public class ComparisonExpression : IExpression
	{
		private FieldValue _left;

		private IComparisonOperand _right;

		private ComparisonOperator _op;

		public ComparisonExpression(FieldValue left, IComparisonOperand right, ComparisonOperator
			 op)
		{
			if (left == null || right == null || op == null)
			{
				throw new ArgumentNullException();
			}
			this._left = left;
			this._right = right;
			this._op = op;
		}

		public virtual FieldValue Left()
		{
			return _left;
		}

		public virtual IComparisonOperand Right()
		{
			return _right;
		}

		public virtual ComparisonOperator Op()
		{
			return _op;
		}

		public override string ToString()
		{
			return _left + " " + _op + " " + _right;
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
			Db4objects.Db4o.NativeQueries.Expr.ComparisonExpression casted = (Db4objects.Db4o.NativeQueries.Expr.ComparisonExpression
				)other;
			return _left.Equals(casted._left) && _right.Equals(casted._right) && _op.Equals(casted
				._op);
		}

		public override int GetHashCode()
		{
			return (_left.GetHashCode() * 29 + _right.GetHashCode()) * 29 + _op.GetHashCode();
		}

		public virtual void Accept(IExpressionVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
