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
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Internal.Btree
{
	/// <exclude></exclude>
	public class BTreeRemove : BTreeUpdate
	{
		public BTreeRemove(Transaction transaction, object obj) : base(transaction, obj)
		{
		}

		protected override void Committed(BTree btree)
		{
			btree.NotifyRemoveListener(new TransactionContext(_transaction, GetObject()));
		}

		public override string ToString()
		{
			return "(-) " + base.ToString();
		}

		public override bool IsRemove()
		{
			return true;
		}

		protected override object GetCommittedObject()
		{
			return No4.Instance;
		}

		protected override void AdjustSizeOnRemovalByOtherTransaction(BTree btree, BTreeNode
			 node)
		{
			// The size was reduced for this entry, let's change back.
			btree.SizeChanged(_transaction, node, +1);
		}

		protected override int SizeDiff()
		{
			return 0;
		}
	}
}
