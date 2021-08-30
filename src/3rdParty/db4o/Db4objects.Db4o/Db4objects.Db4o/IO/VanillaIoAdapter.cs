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
using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.IO
{
	/// <summary>base class for IoAdapters that delegate to other IoAdapters (decorator pattern)
	/// 	</summary>
	[System.ObsoleteAttribute(@"use  /  instead.")]
	public abstract class VanillaIoAdapter : IoAdapter
	{
		protected IoAdapter _delegate;

		public VanillaIoAdapter(IoAdapter delegateAdapter)
		{
			_delegate = delegateAdapter;
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		protected VanillaIoAdapter(IoAdapter delegateAdapter, string path, bool lockFile, 
			long initialLength, bool readOnly) : this(delegateAdapter.Open(path, lockFile, initialLength
			, readOnly))
		{
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Close()
		{
			_delegate.Close();
		}

		public override void Delete(string path)
		{
			_delegate.Delete(path);
		}

		public override bool Exists(string path)
		{
			return _delegate.Exists(path);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override long GetLength()
		{
			return _delegate.GetLength();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override int Read(byte[] bytes, int length)
		{
			return _delegate.Read(bytes, length);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Seek(long pos)
		{
			_delegate.Seek(pos);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Sync()
		{
			_delegate.Sync();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Write(byte[] buffer, int length)
		{
			_delegate.Write(buffer, length);
		}
	}
}
