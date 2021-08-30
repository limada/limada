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
	public class NullFreespaceManager : IFreespaceManager
	{
		public static readonly IFreespaceManager Instance = new Db4objects.Db4o.Internal.Freespace.NullFreespaceManager
			();

		private NullFreespaceManager()
		{
		}

		public virtual Slot AllocateSlot(int length)
		{
			return null;
		}

		public virtual Slot AllocateSafeSlot(int length)
		{
			return null;
		}

		public virtual void BeginCommit()
		{
		}

		public virtual void Commit()
		{
		}

		public virtual void EndCommit()
		{
		}

		public virtual void Free(Slot slot)
		{
		}

		public virtual void FreeSelf()
		{
		}

		public virtual void FreeSafeSlot(Slot slot)
		{
		}

		public virtual void Listener(IFreespaceListener listener)
		{
		}

		public virtual void MigrateTo(IFreespaceManager fm)
		{
		}

		public virtual int SlotCount()
		{
			return 0;
		}

		public virtual void SlotFreed(Slot slot)
		{
		}

		public virtual void Start(int id)
		{
		}

		public virtual byte SystemType()
		{
			return 0;
		}

		public virtual int TotalFreespace()
		{
			return 0;
		}

		public virtual void Traverse(IVisitor4 visitor)
		{
		}

		public virtual void Write(LocalObjectContainer container)
		{
		}

		public virtual bool IsStarted()
		{
			return false;
		}

		public virtual Slot AllocateTransactionLogSlot(int length)
		{
			return null;
		}

		public virtual void Read(LocalObjectContainer container, Slot slot)
		{
		}
	}
}
