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
	public sealed class TransportIdSystem : ITransactionalIdSystem
	{
		private readonly LocalObjectContainer _container;

		public TransportIdSystem(LocalObjectContainer localObjectContainer)
		{
			_container = localObjectContainer;
		}

		public int NewId(SlotChangeFactory slotChangeFactory)
		{
			return _container.AllocatePointerSlot();
		}

		public void NotifySlotCreated(int id, Slot slot, SlotChangeFactory slotChangeFactory
			)
		{
			WritePointer(id, slot);
		}

		private void WritePointer(int id, Slot slot)
		{
			_container.WritePointer(id, slot);
		}

		public void NotifySlotUpdated(int id, Slot slot, SlotChangeFactory slotChangeFactory
			)
		{
			WritePointer(id, slot);
		}

		public void NotifySlotDeleted(int id, SlotChangeFactory slotChangeFactory)
		{
			WritePointer(id, Slot.Zero);
		}

		public void Commit(FreespaceCommitter accumulator)
		{
		}

		// don't do anything
		public Slot CurrentSlot(int id)
		{
			return CommittedSlot(id);
		}

		public void CollectCallBackInfo(ICallbackInfoCollector collector)
		{
		}

		// do nothing
		public void Clear()
		{
		}

		// TODO Auto-generated method stub
		public Slot CommittedSlot(int id)
		{
			return _container.ReadPointerSlot(id);
		}

		public bool IsDeleted(int id)
		{
			return false;
		}

		public bool IsDirty()
		{
			return false;
		}

		public int PrefetchID()
		{
			return 0;
		}

		public void PrefetchedIDConsumed(int id)
		{
		}

		public void Rollback()
		{
		}

		public void Close()
		{
		}

		public void AccumulateFreeSlots(FreespaceCommitter freespaceCommitter, bool forFreespace
			)
		{
		}
	}
}
