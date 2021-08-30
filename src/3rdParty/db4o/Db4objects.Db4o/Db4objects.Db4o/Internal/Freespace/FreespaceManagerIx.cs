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
using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Freespace
{
	/// <summary>Old freespacemanager, before version 7.0.</summary>
	/// <remarks>
	/// Old freespacemanager, before version 7.0.
	/// If it is still in use freespace is dropped.
	/// <see cref="BTreeFreespaceManager">BTreeFreespaceManager</see>
	/// should be used instead.
	/// </remarks>
	public class FreespaceManagerIx : AbstractFreespaceManager
	{
		public FreespaceManagerIx(int discardLimit) : base(null, discardLimit)
		{
		}

		public override Slot AllocateSafeSlot(int length)
		{
			return null;
		}

		public override void FreeSafeSlot(Slot slot)
		{
		}

		// do nothing
		public override void BeginCommit()
		{
		}

		public override void EndCommit()
		{
		}

		public override int SlotCount()
		{
			throw new InvalidOperationException();
		}

		public override void Free(Slot slot)
		{
		}

		// Should no longer be used: Should not happen.
		public override void FreeSelf()
		{
		}

		// do nothing, freespace is dropped.
		public override Slot AllocateSlot(int length)
		{
			// implementation is no longer present, no freespace returned.
			return null;
		}

		public override void MigrateTo(IFreespaceManager fm)
		{
		}

		// do nothing, freespace is dropped.
		public override void Traverse(IVisitor4 visitor)
		{
			throw new InvalidOperationException();
		}

		public override void Start(int id)
		{
		}

		public override byte SystemType()
		{
			return FmIx;
		}

		public override void Write(LocalObjectContainer container)
		{
		}

		public override void Commit()
		{
		}

		public override void Listener(IFreespaceListener listener)
		{
		}

		public override bool IsStarted()
		{
			return false;
		}

		public override Slot AllocateTransactionLogSlot(int length)
		{
			return null;
		}

		public override void Read(LocalObjectContainer container, Slot slot)
		{
		}
	}
}
