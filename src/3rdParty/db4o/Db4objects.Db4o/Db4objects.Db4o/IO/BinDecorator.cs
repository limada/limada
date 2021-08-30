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
using Sharpen.Lang;

namespace Db4objects.Db4o.IO
{
	/// <summary>Wrapper baseclass for all classes that wrap Bin.</summary>
	/// <remarks>
	/// Wrapper baseclass for all classes that wrap Bin.
	/// Each class that adds functionality to a Bin must
	/// extend this class to allow db4o to access the
	/// delegate instance with
	/// <see cref="StorageDecorator.Decorate(BinConfiguration, IBin)">StorageDecorator.Decorate(BinConfiguration, IBin)
	/// 	</see>
	/// .
	/// </remarks>
	public class BinDecorator : IBin
	{
		protected readonly IBin _bin;

		/// <summary>Default constructor.</summary>
		/// <remarks>Default constructor.</remarks>
		/// <param name="bin">
		/// the
		/// <see cref="IBin">IBin</see>
		/// that is to be wrapped.
		/// </param>
		public BinDecorator(IBin bin)
		{
			_bin = bin;
		}

		/// <summary>
		/// closes the BinDecorator and the underlying
		/// <see cref="IBin">IBin</see>
		/// .
		/// </summary>
		public virtual void Close()
		{
			_bin.Close();
		}

		/// <seealso cref="IBin.Length()"></seealso>
		public virtual long Length()
		{
			return _bin.Length();
		}

		/// <seealso cref="IBin.Read(long, byte[], int)">IBin.Read(long, byte[], int)</seealso>
		public virtual int Read(long position, byte[] bytes, int bytesToRead)
		{
			return _bin.Read(position, bytes, bytesToRead);
		}

		/// <seealso cref="IBin.Sync()">IBin.Sync()</seealso>
		public virtual void Sync()
		{
			_bin.Sync();
		}

		/// <seealso cref="IBin.SyncRead(long, byte[], int)">IBin.SyncRead(long, byte[], int)
		/// 	</seealso>
		public virtual int SyncRead(long position, byte[] bytes, int bytesToRead)
		{
			return _bin.SyncRead(position, bytes, bytesToRead);
		}

		/// <seealso cref="IBin.Write(long, byte[], int)">IBin.Write(long, byte[], int)</seealso>
		public virtual void Write(long position, byte[] bytes, int bytesToWrite)
		{
			_bin.Write(position, bytes, bytesToWrite);
		}

		public virtual void Sync(IRunnable runnable)
		{
			_bin.Sync(runnable);
		}
	}
}
