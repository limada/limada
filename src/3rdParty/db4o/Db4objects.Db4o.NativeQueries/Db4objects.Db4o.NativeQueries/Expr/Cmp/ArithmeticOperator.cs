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
namespace Db4objects.Db4o.NativeQueries.Expr.Cmp
{
	public sealed class ArithmeticOperator
	{
		public const int AddId = 0;

		public const int SubtractId = 1;

		public const int MultiplyId = 2;

		public const int DivideId = 3;

		public static readonly Db4objects.Db4o.NativeQueries.Expr.Cmp.ArithmeticOperator 
			Add = new Db4objects.Db4o.NativeQueries.Expr.Cmp.ArithmeticOperator(AddId, "+");

		public static readonly Db4objects.Db4o.NativeQueries.Expr.Cmp.ArithmeticOperator 
			Subtract = new Db4objects.Db4o.NativeQueries.Expr.Cmp.ArithmeticOperator(SubtractId
			, "-");

		public static readonly Db4objects.Db4o.NativeQueries.Expr.Cmp.ArithmeticOperator 
			Multiply = new Db4objects.Db4o.NativeQueries.Expr.Cmp.ArithmeticOperator(MultiplyId
			, "*");

		public static readonly Db4objects.Db4o.NativeQueries.Expr.Cmp.ArithmeticOperator 
			Divide = new Db4objects.Db4o.NativeQueries.Expr.Cmp.ArithmeticOperator(DivideId, 
			"/");

		private string _op;

		private int _id;

		private ArithmeticOperator(int id, string op)
		{
			_id = id;
			_op = op;
		}

		public int Id()
		{
			return _id;
		}

		public override string ToString()
		{
			return _op;
		}
	}
}
