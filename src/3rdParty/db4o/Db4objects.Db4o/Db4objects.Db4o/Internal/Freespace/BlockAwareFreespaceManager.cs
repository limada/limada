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
	/// <exclude></exclude>
	public class BlockAwareFreespaceManager : IFreespaceManager
	{
		private readonly IFreespaceManager _delegate;

		private readonly IBlockConverter _blockConverter;

		public BlockAwareFreespaceManager(IFreespaceManager delegate_, IBlockConverter blockConverter
			)
		{
			_delegate = delegate_;
			_blockConverter = blockConverter;
		}

		public virtual Slot AllocateSlot(int length)
		{
			Slot slot = _delegate.AllocateSlot(_blockConverter.BytesToBlocks(length));
			if (slot == null)
			{
				return null;
			}
			return _blockConverter.ToNonBlockedLength(slot);
		}

		public virtual Slot AllocateSafeSlot(int length)
		{
			Slot slot = _delegate.AllocateSafeSlot(_blockConverter.BytesToBlocks(length));
			if (slot == null)
			{
				return null;
			}
			return _blockConverter.ToNonBlockedLength(slot);
		}

		public virtual void BeginCommit()
		{
			_delegate.BeginCommit();
		}

		public virtual void Commit()
		{
			_delegate.Commit();
		}

		public virtual void EndCommit()
		{
			_delegate.EndCommit();
		}

		public virtual void Free(Slot slot)
		{
			_delegate.Free(_blockConverter.ToBlockedLength(slot));
		}

		public virtual void FreeSelf()
		{
			_delegate.FreeSelf();
		}

		public virtual void FreeSafeSlot(Slot slot)
		{
			_delegate.FreeSafeSlot(_blockConverter.ToBlockedLength(slot));
		}

		public virtual void Listener(IFreespaceListener listener)
		{
			_delegate.Listener(listener);
		}

		public virtual void MigrateTo(IFreespaceManager fm)
		{
			throw new InvalidOperationException();
		}

		public virtual int SlotCount()
		{
			return _delegate.SlotCount();
		}

		public virtual void Start(int id)
		{
			throw new InvalidOperationException();
		}

		public virtual byte SystemType()
		{
			return _delegate.SystemType();
		}

		public virtual int TotalFreespace()
		{
			return _blockConverter.BlocksToBytes(_delegate.TotalFreespace());
		}

		public virtual void Traverse(IVisitor4 visitor)
		{
			_delegate.Traverse(new _IVisitor4_89(this, visitor));
		}

		private sealed class _IVisitor4_89 : IVisitor4
		{
			public _IVisitor4_89(BlockAwareFreespaceManager _enclosing, IVisitor4 visitor)
			{
				this._enclosing = _enclosing;
				this.visitor = visitor;
			}

			public void Visit(object slot)
			{
				visitor.Visit(this._enclosing._blockConverter.ToNonBlockedLength(((Slot)slot)));
			}

			private readonly BlockAwareFreespaceManager _enclosing;

			private readonly IVisitor4 visitor;
		}

		public virtual void Write(LocalObjectContainer container)
		{
			_delegate.Write(container);
		}

		public virtual void SlotFreed(Slot slot)
		{
			_delegate.SlotFreed(slot);
		}

		public virtual bool IsStarted()
		{
			return _delegate.IsStarted();
		}

		public virtual Slot AllocateTransactionLogSlot(int length)
		{
			Slot slot = _delegate.AllocateTransactionLogSlot(_blockConverter.BytesToBlocks(length
				));
			if (slot == null)
			{
				return null;
			}
			return _blockConverter.ToNonBlockedLength(slot);
		}

		public virtual void Read(LocalObjectContainer container, Slot slot)
		{
			throw new InvalidOperationException();
		}
	}
}
