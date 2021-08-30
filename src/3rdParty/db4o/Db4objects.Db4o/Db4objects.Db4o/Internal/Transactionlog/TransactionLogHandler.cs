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
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Transactionlog
{
	/// <exclude></exclude>
	public abstract class TransactionLogHandler
	{
		protected readonly LocalObjectContainer _container;

		protected TransactionLogHandler(LocalObjectContainer container)
		{
			_container = container;
		}

		protected virtual LocalObjectContainer LocalContainer()
		{
			return _container;
		}

		protected void FlushDatabaseFile()
		{
			_container.SyncFiles();
		}

		protected void AppendSlotChanges(ByteArrayBuffer writer, IVisitable slotChangeVisitable
			)
		{
			slotChangeVisitable.Accept(new _IVisitor4_30(writer));
		}

		private sealed class _IVisitor4_30 : IVisitor4
		{
			public _IVisitor4_30(ByteArrayBuffer writer)
			{
				this.writer = writer;
			}

			public void Visit(object obj)
			{
				((SlotChange)obj).Write(writer);
			}

			private readonly ByteArrayBuffer writer;
		}

		protected virtual bool WriteSlots(IVisitable slotChangeTree)
		{
			BooleanByRef ret = new BooleanByRef();
			slotChangeTree.Accept(new _IVisitor4_39(this, ret));
			return ret.value;
		}

		private sealed class _IVisitor4_39 : IVisitor4
		{
			public _IVisitor4_39(TransactionLogHandler _enclosing, BooleanByRef ret)
			{
				this._enclosing = _enclosing;
				this.ret = ret;
			}

			public void Visit(object obj)
			{
				((SlotChange)obj).WritePointer(this._enclosing._container);
				ret.value = true;
			}

			private readonly TransactionLogHandler _enclosing;

			private readonly BooleanByRef ret;
		}

		protected int TransactionLogSlotLength(int slotChangeCount)
		{
			// slotchanges * 3 for ID, address, length
			// 2 ints for slotlength and count
			return ((slotChangeCount * 3) + 2) * Const4.IntLength;
		}

		public abstract Slot AllocateSlot(bool append, int slotChangeCount);

		public abstract void ApplySlotChanges(IVisitable slotChangeTree, int slotChangeCount
			, Slot reservedSlot);

		public abstract void CompleteInterruptedTransaction(int transactionId1, int transactionId2
			);

		public abstract void Close();

		protected virtual void ReadWriteSlotChanges(ByteArrayBuffer buffer)
		{
			LockedTree slotChanges = new LockedTree();
			slotChanges.Read(buffer, new SlotChange(0));
			if (WriteSlots(new _IVisitable_65(slotChanges)))
			{
				FlushDatabaseFile();
			}
		}

		private sealed class _IVisitable_65 : IVisitable
		{
			public _IVisitable_65(LockedTree slotChanges)
			{
				this.slotChanges = slotChanges;
			}

			public void Accept(IVisitor4 visitor)
			{
				slotChanges.TraverseMutable(visitor);
			}

			private readonly LockedTree slotChanges;
		}
	}
}
