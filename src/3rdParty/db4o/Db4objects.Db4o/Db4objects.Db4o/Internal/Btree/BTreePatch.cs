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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Internal.Btree
{
	public abstract class BTreePatch
	{
		protected readonly Transaction _transaction;

		protected object _object;

		public BTreePatch(Transaction transaction, object obj)
		{
			_transaction = transaction;
			_object = obj;
		}

		public abstract object Commit(Transaction trans, BTree btree, BTreeNode node);

		public abstract Db4objects.Db4o.Internal.Btree.BTreePatch ForTransaction(Transaction
			 trans);

		public virtual object GetObject()
		{
			return _object;
		}

		public virtual bool IsAdd()
		{
			return false;
		}

		public virtual bool IsCancelledRemoval()
		{
			return false;
		}

		public virtual bool IsRemove()
		{
			return false;
		}

		public abstract object Key(Transaction trans);

		public abstract object Rollback(Transaction trans, BTree btree);

		public override string ToString()
		{
			if (_object == null)
			{
				return "[NULL]";
			}
			return _object.ToString();
		}

		public abstract int SizeDiff(Transaction trans);
	}
}
