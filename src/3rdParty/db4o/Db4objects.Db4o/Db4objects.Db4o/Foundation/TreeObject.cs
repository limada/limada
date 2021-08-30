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
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class TreeObject : Tree
	{
		private readonly object _object;

		private readonly IComparison4 _function;

		public TreeObject(object @object, IComparison4 function)
		{
			_object = @object;
			_function = function;
		}

		public override int Compare(Tree tree)
		{
			return _function.Compare(_object, tree.Key());
		}

		public override object Key()
		{
			return _object;
		}
	}
}
