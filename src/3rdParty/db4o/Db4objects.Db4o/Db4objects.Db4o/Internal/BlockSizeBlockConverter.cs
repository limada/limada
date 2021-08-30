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
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public sealed class BlockSizeBlockConverter : IBlockConverter
	{
		private readonly int _blockSize;

		public BlockSizeBlockConverter(int blockSize)
		{
			_blockSize = blockSize;
		}

		public int BytesToBlocks(long bytes)
		{
			return (int)((bytes + _blockSize - 1) / _blockSize);
		}

		public int BlockAlignedBytes(int bytes)
		{
			return BytesToBlocks(bytes) * _blockSize;
		}

		public int BlocksToBytes(int blocks)
		{
			return blocks * _blockSize;
		}

		public Slot ToBlockedLength(Slot slot)
		{
			return new Slot(slot.Address(), BytesToBlocks(slot.Length()));
		}

		public Slot ToNonBlockedLength(Slot slot)
		{
			return new Slot(slot.Address(), BlocksToBytes(slot.Length()));
		}
	}
}
