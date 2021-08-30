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
using Sharpen;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public sealed class BitMap4
	{
		private readonly byte[] _bits;

		public BitMap4(int numBits)
		{
			_bits = new byte[ByteCount(numBits)];
		}

		/// <summary>"readFrom  buffer" constructor</summary>
		public BitMap4(byte[] buffer, int pos, int numBits) : this(numBits)
		{
			System.Array.Copy(buffer, pos, _bits, 0, _bits.Length);
		}

		public BitMap4(byte singleByte)
		{
			_bits = new byte[] { singleByte };
		}

		public bool IsTrue(int bit)
		{
			return (((_bits[ArrayOffset(bit)]) >> (ByteOffset(bit) & 0x1f)) & 1) != 0;
		}

		public bool IsFalse(int bit)
		{
			return !IsTrue(bit);
		}

		public int MarshalledLength()
		{
			return _bits.Length;
		}

		public void SetFalse(int bit)
		{
			_bits[ArrayOffset(bit)] &= (byte)~BitMask(bit);
		}

		public void Set(int bit, bool val)
		{
			if (val)
			{
				SetTrue(bit);
			}
			else
			{
				SetFalse(bit);
			}
		}

		public void SetTrue(int bit)
		{
			_bits[ArrayOffset(bit)] |= BitMask(bit);
		}

		public void WriteTo(byte[] bytes, int pos)
		{
			System.Array.Copy(_bits, 0, bytes, pos, _bits.Length);
		}

		private byte ByteOffset(int bit)
		{
			return (byte)(bit % 8);
		}

		private int ArrayOffset(int bit)
		{
			return bit / 8;
		}

		private byte BitMask(int bit)
		{
			return (byte)(1 << ByteOffset(bit));
		}

		private int ByteCount(int numBits)
		{
			return (numBits + 7) / 8;
		}

		public byte GetByte(int index)
		{
			return _bits[index];
		}

		public byte[] Bytes()
		{
			return _bits;
		}
	}
}
