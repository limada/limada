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
	/// <exclude></exclude>
	public class BTreeCancelledRemoval : BTreeUpdate
	{
		private readonly object _newKey;

		public BTreeCancelledRemoval(Transaction transaction, object originalKey, object 
			newKey, BTreeUpdate existingPatches) : base(transaction, originalKey)
		{
			_newKey = newKey;
			if (null != existingPatches)
			{
				Append(existingPatches);
			}
		}

		protected override void Committed(BTree btree)
		{
		}

		// do nothing
		public override bool IsCancelledRemoval()
		{
			return true;
		}

		public override string ToString()
		{
			return "(u) " + base.ToString();
		}

		protected override object GetCommittedObject()
		{
			return _newKey;
		}

		protected override void AdjustSizeOnRemovalByOtherTransaction(BTree btree, BTreeNode
			 node)
		{
		}

		// The other transaction reduces the size, this entry ignores.
		protected override int SizeDiff()
		{
			return 1;
		}
	}
}
