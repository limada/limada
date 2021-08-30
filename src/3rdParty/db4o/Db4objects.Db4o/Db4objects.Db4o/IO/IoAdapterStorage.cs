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
using Db4objects.Db4o.IO;
using Sharpen.Lang;

namespace Db4objects.Db4o.IO
{
	/// <exclude></exclude>
	public class IoAdapterStorage : IStorage
	{
		private readonly IoAdapter _io;

		public IoAdapterStorage(IoAdapter io)
		{
			_io = io;
		}

		public virtual bool Exists(string uri)
		{
			return _io.Exists(uri);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual IBin Open(BinConfiguration config)
		{
			IoAdapterStorage.IoAdapterBin bin = new IoAdapterStorage.IoAdapterBin(_io.Open(config
				.Uri(), config.LockFile(), config.InitialLength(), config.ReadOnly()));
			((IBlockSize)Environments.My(typeof(IBlockSize))).Register(bin);
			return bin;
		}

		internal class IoAdapterBin : IBin, IListener4
		{
			private readonly IoAdapter _io;

			public IoAdapterBin(IoAdapter io)
			{
				_io = io;
			}

			public virtual void Close()
			{
				_io.Close();
			}

			public virtual long Length()
			{
				return _io.GetLength();
			}

			public virtual int Read(long position, byte[] buffer, int bytesToRead)
			{
				_io.Seek(position);
				return _io.Read(buffer, bytesToRead);
			}

			public virtual void Sync()
			{
				_io.Sync();
			}

			public virtual int SyncRead(long position, byte[] bytes, int bytesToRead)
			{
				return Read(position, bytes, bytesToRead);
			}

			public virtual void Write(long position, byte[] bytes, int bytesToWrite)
			{
				_io.Seek(position);
				_io.Write(bytes, bytesToWrite);
			}

			public virtual void BlockSize(int blockSize)
			{
				_io.BlockSize(blockSize);
			}

			public virtual void OnEvent(object @event)
			{
				BlockSize((((int)@event)));
			}

			public virtual void Sync(IRunnable runnable)
			{
				Sync();
				runnable.Run();
				Sync();
			}
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Delete(string uri)
		{
			_io.Delete(uri);
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Rename(string oldUri, string newUri)
		{
			throw new NotImplementedException();
		}
	}
}
