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
	public class BoolConstExpression : IExpression
	{
		public static readonly Db4objects.Db4o.NativeQueries.Expr.BoolConstExpression True
			 = new Db4objects.Db4o.NativeQueries.Expr.BoolConstExpression(true);

		public static readonly Db4objects.Db4o.NativeQueries.Expr.BoolConstExpression False
			 = new Db4objects.Db4o.NativeQueries.Expr.BoolConstExpression(false);

		private bool _value;

		private BoolConstExpression(bool value)
		{
			this._value = value;
		}

		public virtual bool Value()
		{
			return _value;
		}

		public override string ToString()
		{
			return _value.ToString();
		}

		public static Db4objects.Db4o.NativeQueries.Expr.BoolConstExpression Expr(bool value
			)
		{
			return (value ? True : False);
		}

		public virtual void Accept(IExpressionVisitor visitor)
		{
			visitor.Visit(this);
		}

		public virtual IExpression Negate()
		{
			return (_value ? False : True);
		}
	}
}
