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
	/// <exclude></exclude>
	public class SynchronizedBin : BinDecorator
	{
		public SynchronizedBin(IBin bin) : base(bin)
		{
		}

		public override void Close()
		{
			lock (_bin)
			{
				base.Close();
			}
		}

		public override long Length()
		{
			lock (_bin)
			{
				return base.Length();
			}
		}

		public override int Read(long position, byte[] buffer, int bytesToRead)
		{
			lock (_bin)
			{
				return base.Read(position, buffer, bytesToRead);
			}
		}

		public override void Write(long position, byte[] bytes, int bytesToWrite)
		{
			lock (_bin)
			{
				base.Write(position, bytes, bytesToWrite);
			}
		}

		public override void Sync()
		{
			lock (_bin)
			{
				base.Sync();
			}
		}

		public override void Sync(IRunnable runnable)
		{
			lock (_bin)
			{
				base.Sync(runnable);
			}
		}
	}
}
