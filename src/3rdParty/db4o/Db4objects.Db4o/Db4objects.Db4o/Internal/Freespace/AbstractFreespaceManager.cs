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
	public abstract class AbstractFreespaceManager : IFreespaceManager
	{
		public const byte FmDebug = 127;

		public const byte FmDefault = 0;

		public const byte FmLegacyRam = 1;

		public const byte FmRam = 2;

		public const byte FmIx = 3;

		public const byte FmBtree = 4;

		private const int IntsInSlot = 12;

		public const int RemainderSizeLimit = 20;

		public static byte CheckType(byte systemType)
		{
			if (systemType == FmDefault)
			{
				return FmRam;
			}
			return systemType;
		}

		protected IProcedure4 _slotFreedCallback;

		private readonly int _discardLimit;

		public AbstractFreespaceManager(IProcedure4 slotFreedCallback, int discardLimit)
		{
			_slotFreedCallback = slotFreedCallback;
			_discardLimit = discardLimit;
		}

		public static Db4objects.Db4o.Internal.Freespace.AbstractFreespaceManager CreateNew
			(LocalObjectContainer file)
		{
			return CreateNew(file, file.SystemData().FreespaceSystem());
		}

		public static Db4objects.Db4o.Internal.Freespace.AbstractFreespaceManager CreateNew
			(LocalObjectContainer file, byte systemType)
		{
			systemType = CheckType(systemType);
			int unblockedDiscardLimit = file.ConfigImpl.DiscardFreeSpace();
			int blockedDiscardLimit = unblockedDiscardLimit == int.MaxValue ? unblockedDiscardLimit
				 : file.BlockConverter().BytesToBlocks(unblockedDiscardLimit);
			IProcedure4 slotFreedCallback = new _IProcedure4_50(file);
			switch (systemType)
			{
				case FmIx:
				{
					return new FreespaceManagerIx(blockedDiscardLimit);
				}

				case FmBtree:
				{
					return new BTreeFreespaceManager(file, slotFreedCallback, blockedDiscardLimit);
				}

				default:
				{
					return new InMemoryFreespaceManager(slotFreedCallback, blockedDiscardLimit);
					break;
				}
			}
		}

		private sealed class _IProcedure4_50 : IProcedure4
		{
			public _IProcedure4_50(LocalObjectContainer file)
			{
				this.file = file;
			}

			public void Apply(object slot)
			{
				file.OverwriteDeletedBlockedSlot(((Slot)slot));
			}

			private readonly LocalObjectContainer file;
		}

		public static int InitSlot(LocalObjectContainer file)
		{
			int address = file.AllocateSlot(SlotLength()).Address();
			SlotEntryToZeroes(file, address);
			return address;
		}

		public virtual void MigrateTo(IFreespaceManager fm)
		{
			Traverse(new _IVisitor4_72(fm));
		}

		private sealed class _IVisitor4_72 : IVisitor4
		{
			public _IVisitor4_72(IFreespaceManager fm)
			{
				this.fm = fm;
			}

			public void Visit(object obj)
			{
				fm.Free((Slot)obj);
			}

			private readonly IFreespaceManager fm;
		}

		internal static void SlotEntryToZeroes(LocalObjectContainer file, int address)
		{
			StatefulBuffer writer = new StatefulBuffer(file.SystemTransaction(), address, SlotLength
				());
			for (int i = 0; i < IntsInSlot; i++)
			{
				writer.WriteInt(0);
			}
			writer.WriteEncrypt();
		}

		internal static int SlotLength()
		{
			return Const4.IntLength * IntsInSlot;
		}

		public virtual int TotalFreespace()
		{
			IntByRef mint = new IntByRef();
			Traverse(new _IVisitor4_97(mint));
			return mint.value;
		}

		private sealed class _IVisitor4_97 : IVisitor4
		{
			public _IVisitor4_97(IntByRef mint)
			{
				this.mint = mint;
			}

			public void Visit(object obj)
			{
				Slot slot = (Slot)obj;
				mint.value += slot.Length();
			}

			private readonly IntByRef mint;
		}

		protected virtual int DiscardLimit()
		{
			return _discardLimit;
		}

		protected bool SplitRemainder(int length)
		{
			if (CanDiscard(length))
			{
				return false;
			}
			return length > RemainderSizeLimit;
		}

		internal bool CanDiscard(int length)
		{
			return length == 0 || length < DiscardLimit();
		}

		public static void Migrate(IFreespaceManager oldFM, IFreespaceManager newFM)
		{
			oldFM.MigrateTo(newFM);
			oldFM.FreeSelf();
		}

		public virtual void DebugCheckIntegrity()
		{
			IntByRef lastStart = new IntByRef();
			IntByRef lastEnd = new IntByRef();
			Traverse(new _IVisitor4_129(lastEnd, lastStart));
		}

		private sealed class _IVisitor4_129 : IVisitor4
		{
			public _IVisitor4_129(IntByRef lastEnd, IntByRef lastStart)
			{
				this.lastEnd = lastEnd;
				this.lastStart = lastStart;
			}

			public void Visit(object obj)
			{
				Slot slot = (Slot)obj;
				if (slot.Address() <= lastEnd.value)
				{
					throw new InvalidOperationException();
				}
				lastStart.value = slot.Address();
				lastEnd.value = slot.Address() + slot.Length();
			}

			private readonly IntByRef lastEnd;

			private readonly IntByRef lastStart;
		}

		public static bool MigrationRequired(byte systemType)
		{
			return systemType == FmLegacyRam || systemType == FmIx;
		}

		public virtual void SlotFreed(Slot slot)
		{
			if (_slotFreedCallback == null)
			{
				return;
			}
			_slotFreedCallback.Apply(slot);
		}

		public abstract Slot AllocateSafeSlot(int arg1);

		public abstract Slot AllocateSlot(int arg1);

		public abstract Slot AllocateTransactionLogSlot(int arg1);

		public abstract void BeginCommit();

		public abstract void Commit();

		public abstract void EndCommit();

		public abstract void Free(Slot arg1);

		public abstract void FreeSafeSlot(Slot arg1);

		public abstract void FreeSelf();

		public abstract bool IsStarted();

		public abstract void Listener(IFreespaceListener arg1);

		public abstract void Read(LocalObjectContainer arg1, Slot arg2);

		public abstract int SlotCount();

		public abstract void Start(int arg1);

		public abstract byte SystemType();

		public abstract void Traverse(IVisitor4 arg1);

		public abstract void Write(LocalObjectContainer arg1);
	}
}
