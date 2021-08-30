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
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Ids
{
	/// <exclude></exclude>
	public interface ITransactionalIdSystem
	{
		void CollectCallBackInfo(ICallbackInfoCollector collector);

		bool IsDirty();

		void Commit(FreespaceCommitter freespaceCommitter);

		Slot CommittedSlot(int id);

		Slot CurrentSlot(int id);

		void AccumulateFreeSlots(FreespaceCommitter freespaceCommitter, bool forFreespace
			);

		void Rollback();

		void Clear();

		bool IsDeleted(int id);

		void NotifySlotUpdated(int id, Slot slot, SlotChangeFactory slotChangeFactory);

		void NotifySlotCreated(int id, Slot slot, SlotChangeFactory slotChangeFactory);

		void NotifySlotDeleted(int id, SlotChangeFactory slotChangeFactory);

		int NewId(SlotChangeFactory slotChangeFactory);

		int PrefetchID();

		void PrefetchedIDConsumed(int id);

		void Close();
	}
}
