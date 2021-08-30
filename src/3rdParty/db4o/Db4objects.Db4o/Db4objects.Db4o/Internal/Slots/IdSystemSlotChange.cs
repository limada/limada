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
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Slots
{
	/// <exclude></exclude>
	public class IdSystemSlotChange : SystemSlotChange
	{
		private Collection4 _freed;

		public IdSystemSlotChange(int id) : base(id)
		{
		}

		protected override void Free(IFreespaceManager freespaceManager, Slot slot)
		{
			if (slot.IsNull())
			{
				return;
			}
			if (_freed == null)
			{
				_freed = new Collection4();
			}
			_freed.Add(slot);
		}

		public override void AccumulateFreeSlot(TransactionalIdSystemImpl idSystem, FreespaceCommitter
			 freespaceCommitter, bool forFreespace)
		{
			if (ForFreespace() != forFreespace)
			{
				return;
			}
			base.AccumulateFreeSlot(idSystem, freespaceCommitter, forFreespace);
			if (_freed == null)
			{
				return;
			}
			IEnumerator iterator = _freed.GetEnumerator();
			while (iterator.MoveNext())
			{
				freespaceCommitter.DelayedFree((Slot)iterator.Current, FreeToSystemFreespaceSystem
					());
			}
		}

		protected override bool FreeToSystemFreespaceSystem()
		{
			return true;
		}
	}
}
