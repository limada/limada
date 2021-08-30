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
	public sealed class ComparisonOperator
	{
		public const int EqualsId = 0;

		public const int SmallerId = 1;

		public const int GreaterId = 2;

		public const int ContainsId = 3;

		public const int StartswithId = 4;

		public const int EndswithId = 5;

		public const int IdentityId = 6;

		public static readonly Db4objects.Db4o.NativeQueries.Expr.Cmp.ComparisonOperator 
			ValueEquality = new Db4objects.Db4o.NativeQueries.Expr.Cmp.ComparisonOperator(EqualsId
			, "==", true);

		public static readonly Db4objects.Db4o.NativeQueries.Expr.Cmp.ComparisonOperator 
			Smaller = new Db4objects.Db4o.NativeQueries.Expr.Cmp.ComparisonOperator(SmallerId
			, "<", false);

		public static readonly Db4objects.Db4o.NativeQueries.Expr.Cmp.ComparisonOperator 
			Greater = new Db4objects.Db4o.NativeQueries.Expr.Cmp.ComparisonOperator(GreaterId
			, ">", false);

		public static readonly Db4objects.Db4o.NativeQueries.Expr.Cmp.ComparisonOperator 
			Contains = new Db4objects.Db4o.NativeQueries.Expr.Cmp.ComparisonOperator(ContainsId
			, "<CONTAINS>", false);

		public static readonly Db4objects.Db4o.NativeQueries.Expr.Cmp.ComparisonOperator 
			StartsWith = new Db4objects.Db4o.NativeQueries.Expr.Cmp.ComparisonOperator(StartswithId
			, "<STARTSWITH>", false);

		public static readonly Db4objects.Db4o.NativeQueries.Expr.Cmp.ComparisonOperator 
			EndsWith = new Db4objects.Db4o.NativeQueries.Expr.Cmp.ComparisonOperator(EndswithId
			, "<ENDSWITH>", false);

		public static readonly Db4objects.Db4o.NativeQueries.Expr.Cmp.ComparisonOperator 
			ReferenceEquality = new Db4objects.Db4o.NativeQueries.Expr.Cmp.ComparisonOperator
			(IdentityId, "===", true);

		private int _id;

		private string _op;

		private bool _symmetric;

		private ComparisonOperator(int id, string op, bool symmetric)
		{
			// TODO: switch to individual classes and visitor dispatch?
			_id = id;
			_op = op;
			_symmetric = symmetric;
		}

		public int Id()
		{
			return _id;
		}

		public override string ToString()
		{
			return _op;
		}

		public bool IsSymmetric()
		{
			return _symmetric;
		}
	}
}
