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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers;

namespace Db4objects.Db4o.Foundation
{
	public class TreeString : Tree
	{
		public string _key;

		public TreeString(string key)
		{
			this._key = key;
		}

		protected override Tree ShallowCloneInternal(Tree tree)
		{
			Db4objects.Db4o.Foundation.TreeString ts = (Db4objects.Db4o.Foundation.TreeString
				)base.ShallowCloneInternal(tree);
			ts._key = _key;
			return ts;
		}

		public override object ShallowClone()
		{
			return ShallowCloneInternal(new Db4objects.Db4o.Foundation.TreeString(_key));
		}

		public override int Compare(Tree to)
		{
			return StringHandler.Compare(Const4.stringIO.Write(_key), Const4.stringIO.Write((
				(Db4objects.Db4o.Foundation.TreeString)to)._key));
		}

		public override object Key()
		{
			return _key;
		}
	}
}
