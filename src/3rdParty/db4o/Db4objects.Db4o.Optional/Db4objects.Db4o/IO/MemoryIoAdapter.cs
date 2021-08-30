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
using Db4objects.Db4o.IO;
using Sharpen;

namespace Db4objects.Db4o.IO
{
	/// <summary>IoAdapter for in-memory operation.</summary>
	/// <remarks>
	/// IoAdapter for in-memory operation. <br/>
	/// <br/>
	/// Configure db4o to operate with this in-memory IoAdapter with
	/// <code>
	/// MemoryIoAdapter memoryIoAdapter = new MemoryIoAdapter();<br/>
	/// Db4oFactory.Configure().Io(memoryIoAdapter);
	/// </code><br/>
	/// <br/>
	/// <br/>
	/// Use the normal #openFile() and #openServer() commands to open
	/// ObjectContainers and ObjectServers. The names specified as file names will be
	/// used to identify the <code>byte[]</code> content of the in-memory files in
	/// the _memoryFiles Hashtable in the adapter. After working with an in-memory
	/// ObjectContainer/ObjectServer the <code>byte[]</code> content is available
	/// in the MemoryIoAdapter by using
	/// <see cref="Db4objects.Db4o.IO.MemoryIoAdapter.Get">
	/// Db4objects.Db4o.IO.MemoryIoAdapter.Get
	/// </see>
	/// . To add old existing
	/// database <code>byte[]</code> content to a MemoryIoAdapter use
	/// <see cref="Db4objects.Db4o.IO.MemoryIoAdapter.Put">
	/// Db4objects.Db4o.IO.MemoryIoAdapter.Put
	/// 
	/// </see>
	/// . To reduce memory consumption of memory file
	/// names that will no longer be used call
	/// <see cref="Db4objects.Db4o.IO.MemoryIoAdapter.Put">
	/// Db4objects.Db4o.IO.MemoryIoAdapter.Put
	/// 
	/// </see>
	/// and pass
	/// an empty byte array.
	/// 
	/// </remarks>
	public class MemoryIoAdapter : IoAdapter
	{
		private byte[] _bytes;

		private int _length;

		private int _seekPos;

		private Hashtable4 _memoryFiles;

		private int _growBy;

		public MemoryIoAdapter()
		{
			_memoryFiles = new Hashtable4();
			_growBy = 10000;
		}

		public MemoryIoAdapter(int initialLength) : this()
		{
			_bytes = new byte[initialLength];
		}

		private MemoryIoAdapter(Db4objects.Db4o.IO.MemoryIoAdapter adapter, byte[] bytes)
		{
			_bytes = bytes;
			_length = bytes.Length;
			_growBy = adapter._growBy;
		}

		private MemoryIoAdapter(Db4objects.Db4o.IO.MemoryIoAdapter adapter, int initialLength
			) : this(adapter, new byte[initialLength])
		{
		}

		/// <summary>
		/// creates an in-memory database with the passed content bytes and adds it
		/// to the adapter for the specified name.
		/// </summary>
		/// <remarks>
		/// creates an in-memory database with the passed content bytes and adds it
		/// to the adapter for the specified name.
		/// </remarks>
		/// <param name="name">the name to be use for #openFile() or #openServer() calls</param>
		/// <param name="bytes">the database content</param>
		public virtual void Put(string name, byte[] bytes)
		{
			if (bytes == null)
			{
				bytes = new byte[0];
			}
			_memoryFiles.Put(name, new Db4objects.Db4o.IO.MemoryIoAdapter(this, bytes));
		}

		/// <summary>returns the content bytes for a database with the given name.</summary>
		/// <remarks>returns the content bytes for a database with the given name.</remarks>
		/// <param name="name">the name to be use for #openFile() or #openServer() calls</param>
		/// <returns>the content bytes</returns>
		public virtual byte[] Get(string name)
		{
			Db4objects.Db4o.IO.MemoryIoAdapter mia = (Db4objects.Db4o.IO.MemoryIoAdapter)_memoryFiles
				.Get(name);
			if (mia == null)
			{
				return null;
			}
			return mia._bytes;
		}

		/// <summary>
		/// configures the length a memory file should grow, if no more free slots
		/// are found within.
		/// </summary>
		/// <remarks>
		/// configures the length a memory file should grow, if no more free slots
		/// are found within. <br />
		/// <br />
		/// Specify a large value (100,000 or more) for best performance. Specify a
		/// small value (100) for the smallest memory consumption. The default
		/// setting is 10,000.
		/// </remarks>
		/// <param name="length">the length in bytes</param>
		public virtual void GrowBy(int length)
		{
			if (length < 1)
			{
				length = 1;
			}
			_growBy = length;
		}

		/// <summary>for internal processing only.</summary>
		/// <remarks>for internal processing only.</remarks>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Close()
		{
		}

		// do nothing
		public override void Delete(string path)
		{
			_memoryFiles.Remove(path);
		}

		/// <summary>for internal processing only.</summary>
		/// <remarks>for internal processing only.</remarks>
		public override bool Exists(string path)
		{
			Db4objects.Db4o.IO.MemoryIoAdapter mia = (Db4objects.Db4o.IO.MemoryIoAdapter)_memoryFiles
				.Get(path);
			if (mia == null)
			{
				return false;
			}
			return mia._length > 0;
		}

		/// <summary>for internal processing only.</summary>
		/// <remarks>for internal processing only.</remarks>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override long GetLength()
		{
			return _length;
		}

		/// <summary>for internal processing only.</summary>
		/// <remarks>for internal processing only.</remarks>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override IoAdapter Open(string path, bool lockFile, long initialLength, bool
			 readOnly)
		{
			Db4objects.Db4o.IO.MemoryIoAdapter mia = (Db4objects.Db4o.IO.MemoryIoAdapter)_memoryFiles
				.Get(path);
			if (mia == null)
			{
				mia = new Db4objects.Db4o.IO.MemoryIoAdapter(this, (int)initialLength);
				_memoryFiles.Put(path, mia);
			}
			return mia;
		}

		/// <summary>for internal processing only.</summary>
		/// <remarks>for internal processing only.</remarks>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override int Read(byte[] bytes, int length)
		{
			System.Array.Copy(_bytes, _seekPos, bytes, 0, length);
			_seekPos += length;
			return length;
		}

		/// <summary>for internal processing only.</summary>
		/// <remarks>for internal processing only.</remarks>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Seek(long pos)
		{
			_seekPos = (int)pos;
		}

		/// <summary>for internal processing only.</summary>
		/// <remarks>for internal processing only.</remarks>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Sync()
		{
		}

		/// <summary>for internal processing only.</summary>
		/// <remarks>for internal processing only.</remarks>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Write(byte[] buffer, int length)
		{
			if (_seekPos + length > _bytes.Length)
			{
				int growBy = _growBy;
				int missing = _seekPos + length - _bytes.Length;
				if (missing > growBy)
				{
					growBy = missing;
				}
				byte[] temp = new byte[_bytes.Length + growBy];
				System.Array.Copy(_bytes, 0, temp, 0, _length);
				_bytes = temp;
			}
			System.Array.Copy(buffer, 0, _bytes, _seekPos, length);
			_seekPos += length;
			if (_seekPos > _length)
			{
				_length = _seekPos;
			}
		}
	}
}
