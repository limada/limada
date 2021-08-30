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
namespace Db4objects.Db4o.Internal.Btree
{
	/// <exclude></exclude>
	public sealed class SearchTarget
	{
		public static readonly Db4objects.Db4o.Internal.Btree.SearchTarget Lowest = new Db4objects.Db4o.Internal.Btree.SearchTarget
			("Lowest");

		public static readonly Db4objects.Db4o.Internal.Btree.SearchTarget Any = new Db4objects.Db4o.Internal.Btree.SearchTarget
			("Any");

		public static readonly Db4objects.Db4o.Internal.Btree.SearchTarget Highest = new 
			Db4objects.Db4o.Internal.Btree.SearchTarget("Highest");

		private readonly string _target;

		public SearchTarget(string target)
		{
			_target = target;
		}

		public override string ToString()
		{
			return _target;
		}
	}
}
