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
	/// <summary>
	/// Storage adapter that does not pass flush calls
	/// on to its delegate.
	/// </summary>
	/// <remarks>
	/// Storage adapter that does not pass flush calls
	/// on to its delegate.
	/// You can use this
	/// <see cref="IStorage">IStorage</see>
	/// for improved db4o
	/// speed at the risk of corrupted database files in
	/// case of system failure.
	/// </remarks>
	public class NonFlushingStorage : StorageDecorator
	{
		public NonFlushingStorage(IStorage storage) : base(storage)
		{
		}

		protected override IBin Decorate(BinConfiguration config, IBin storage)
		{
			return new NonFlushingStorage.NonFlushingBin(storage);
		}

		private class NonFlushingBin : BinDecorator
		{
			public NonFlushingBin(IBin storage) : base(storage)
			{
			}

			public override void Sync()
			{
			}

			public override void Sync(IRunnable runnable)
			{
				runnable.Run();
			}
		}
	}
}
