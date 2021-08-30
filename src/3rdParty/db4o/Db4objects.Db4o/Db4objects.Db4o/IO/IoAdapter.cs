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
using Db4objects.Db4o;
using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.IO
{
	/// <summary>Base class for database file adapters, both for file and memory databases.
	/// 	</summary>
	/// <remarks>Base class for database file adapters, both for file and memory databases.
	/// 	</remarks>
	[System.ObsoleteAttribute(@"Use classes that implement  instead. The new functionality has been split:  is the factory class to open  adapters.   is the actual implementation of IO access."
		)]
	public abstract class IoAdapter
	{
		private const int CopySize = 4096;

		private int _blockSize;

		/// <summary>converts address and address offset to an absolute address</summary>
		protected long RegularAddress(int blockAddress, int blockAddressOffset)
		{
			if (0 == _blockSize)
			{
				throw new InvalidOperationException();
			}
			return (long)blockAddress * _blockSize + blockAddressOffset;
		}

		/// <summary>copies a block within a file in block mode</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void BlockCopy(int oldAddress, int oldAddressOffset, int newAddress
			, int newAddressOffset, int length)
		{
			Copy(RegularAddress(oldAddress, oldAddressOffset), RegularAddress(newAddress, newAddressOffset
				), length);
		}

		/// <summary>sets the read/write pointer in the file using block mode</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void BlockSeek(int address)
		{
			BlockSeek(address, 0);
		}

		/// <summary>sets the read/write pointer in the file using block mode</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void BlockSeek(int address, int offset)
		{
			Seek(RegularAddress(address, offset));
		}

		/// <summary>outside call to set the block size of this adapter</summary>
		public virtual void BlockSize(int blockSize)
		{
			if (blockSize < 1)
			{
				throw new ArgumentException();
			}
			_blockSize = blockSize;
		}

		/// <summary>implement to close the adapter</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public abstract void Close();

		/// <summary>copies a block within a file in absolute mode</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Copy(long oldAddress, long newAddress, int length)
		{
			if (DTrace.enabled)
			{
				DTrace.IoCopy.LogLength(newAddress, length);
			}
			if (length > CopySize)
			{
				byte[] buffer = new byte[CopySize];
				int pos = 0;
				while (pos + CopySize < length)
				{
					Copy(buffer, oldAddress + pos, newAddress + pos);
					pos += CopySize;
				}
				oldAddress += pos;
				newAddress += pos;
				length -= pos;
			}
			Copy(new byte[length], oldAddress, newAddress);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		private void Copy(byte[] buffer, long oldAddress, long newAddress)
		{
			Seek(oldAddress);
			Read(buffer);
			Seek(newAddress);
			Write(buffer);
		}

		/// <summary>deletes the given path from whatever 'file system' is addressed</summary>
		public abstract void Delete(string path);

		/// <summary>checks whether a file exists</summary>
		public abstract bool Exists(string path);

		/// <summary>implement to return the absolute length of the file</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public abstract long GetLength();

		/// <summary>implement to open the file</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public abstract IoAdapter Open(string path, bool lockFile, long initialLength, bool
			 readOnly);

		/// <summary>reads a buffer at the seeked address</summary>
		/// <returns>the number of bytes read and returned</returns>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual int Read(byte[] buffer)
		{
			return Read(buffer, buffer.Length);
		}

		/// <summary>implement to read a buffer at the seeked address</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public abstract int Read(byte[] bytes, int length);

		/// <summary>implement to set the read/write pointer in the file, absolute mode</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public abstract void Seek(long pos);

		/// <summary>implement to flush the file contents to storage</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public abstract void Sync();

		/// <summary>writes a buffer to the seeked address</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Write(byte[] bytes)
		{
			Write(bytes, bytes.Length);
		}

		/// <summary>implement to write a buffer at the seeked address</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public abstract void Write(byte[] buffer, int length);

		/// <summary>returns the block size currently used</summary>
		public virtual int BlockSize()
		{
			return _blockSize;
		}

		/// <summary>Delegated IO Adapter</summary>
		/// <returns>reference to itself</returns>
		public virtual IoAdapter DelegatedIoAdapter()
		{
			return this;
		}
	}
}
