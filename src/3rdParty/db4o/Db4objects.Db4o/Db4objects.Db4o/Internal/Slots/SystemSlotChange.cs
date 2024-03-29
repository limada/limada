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
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Slots
{
	/// <exclude></exclude>
	public class SystemSlotChange : SlotChange
	{
		public SystemSlotChange(int id) : base(id)
		{
		}

		public override void AccumulateFreeSlot(TransactionalIdSystemImpl idSystem, FreespaceCommitter
			 freespaceCommitter, bool forFreespace)
		{
			base.AccumulateFreeSlot(idSystem, freespaceCommitter, forFreespace);
		}

		// FIXME: If we are doing a delete, we should also free our pointer here.
		protected override Slot ModifiedSlotInParentIdSystem(TransactionalIdSystemImpl idSystem
			)
		{
			return null;
		}

		public override bool RemoveId()
		{
			return _newSlot == Slot.Zero;
		}
	}
}
