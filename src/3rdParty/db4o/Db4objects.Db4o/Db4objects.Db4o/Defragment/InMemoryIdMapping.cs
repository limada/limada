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
using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Defragment
{
	/// <summary>In-memory mapping for IDs during a defragmentation run.</summary>
	/// <remarks>
	/// In-memory mapping for IDs during a defragmentation run.
	/// This is faster than the
	/// <see cref="DatabaseIdMapping">DatabaseIdMapping</see>
	/// but
	/// it uses more memory. If you have OutOfMemory conditions
	/// with this id mapping, use the
	/// <see cref="DatabaseIdMapping">DatabaseIdMapping</see>
	/// instead.
	/// </remarks>
	/// <seealso cref="Defragment">Defragment</seealso>
	public class InMemoryIdMapping : AbstractIdMapping
	{
		private IdSlotTree _idsToSlots;

		private Tree _tree;

		public override int MappedId(int oldID)
		{
			int classID = MappedClassID(oldID);
			if (classID != 0)
			{
				return classID;
			}
			TreeIntObject res = (TreeIntObject)TreeInt.Find(_tree, oldID);
			if (res != null)
			{
				return ((int)res._object);
			}
			return 0;
		}

		public override void Open()
		{
		}

		public override void Close()
		{
		}

		protected override void MapNonClassIDs(int origID, int mappedID)
		{
			_tree = Tree.Add(_tree, new TreeIntObject(origID, mappedID));
		}

		public override int AddressForId(int id)
		{
			IdSlotTree node = (IdSlotTree)_idsToSlots.Find(id);
			if (node == null)
			{
				throw new InvalidOperationException();
			}
			return node.Slot().Address();
		}

		public override void MapId(int id, Slot slot)
		{
			IdSlotTree idSlotMapping = new IdSlotTree(id, slot);
			_idsToSlots = ((IdSlotTree)Tree.Add(_idsToSlots, idSlotMapping));
		}

		public override IVisitable SlotChanges()
		{
			return new _IVisitable_62(this);
		}

		private sealed class _IVisitable_62 : IVisitable
		{
			public _IVisitable_62(InMemoryIdMapping _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Accept(IVisitor4 outSideVisitor)
			{
				Tree.Traverse(this._enclosing._idsToSlots, new _IVisitor4_64(outSideVisitor));
			}

			private sealed class _IVisitor4_64 : IVisitor4
			{
				public _IVisitor4_64(IVisitor4 outSideVisitor)
				{
					this.outSideVisitor = outSideVisitor;
				}

				public void Visit(object idSlotMapping)
				{
					SlotChange slotChange = new SlotChange(((TreeInt)idSlotMapping)._key);
					slotChange.NotifySlotCreated(((IdSlotTree)idSlotMapping).Slot());
					outSideVisitor.Visit(slotChange);
				}

				private readonly IVisitor4 outSideVisitor;
			}

			private readonly InMemoryIdMapping _enclosing;
		}

		public override void Commit()
		{
		}
	}
}
