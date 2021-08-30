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
using Db4objects.Db4o.Internal.Fileheader;
using Sharpen.Lang;

namespace Db4objects.Db4o.Internal.Fileheader
{
	/// <exclude></exclude>
	public abstract class TimerFileLock : IRunnable
	{
		public static TimerFileLock ForFile(LocalObjectContainer file)
		{
			return new TimerFileLockDisabled();
		}

		public abstract void CheckHeaderLock();

		public abstract void CheckOpenTime();

		public abstract bool LockFile();

		public abstract long OpenTime();

		public abstract void SetAddresses(int baseAddress, int openTimeOffset, int accessTimeOffset
			);

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public abstract void Start();

		public abstract void WriteHeaderLock();

		public abstract void WriteOpenTime();

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public abstract void Close();

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public abstract void CheckIfOtherSessionAlive(LocalObjectContainer container, int
			 address, int offset, long lastAccessTime);

		public abstract void Run();
	}
}
