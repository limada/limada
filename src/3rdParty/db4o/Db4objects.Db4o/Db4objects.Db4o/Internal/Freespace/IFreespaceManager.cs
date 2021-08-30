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
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Freespace
{
	/// <exclude></exclude>
	public interface IFreespaceManager
	{
		void BeginCommit();

		void EndCommit();

		int SlotCount();

		void Free(Slot slot);

		void FreeSelf();

		int TotalFreespace();

		Slot AllocateTransactionLogSlot(int length);

		Slot AllocateSlot(int length);

		void MigrateTo(IFreespaceManager fm);

		void Read(LocalObjectContainer container, Slot slot);

		void Start(int id);

		byte SystemType();

		void Traverse(IVisitor4 visitor);

		void Write(LocalObjectContainer container);

		void Commit();

		Slot AllocateSafeSlot(int length);

		void FreeSafeSlot(Slot slot);

		void Listener(IFreespaceListener listener);

		void SlotFreed(Slot slot);

		bool IsStarted();
	}
}
