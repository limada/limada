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
	/// <exclude></exclude>
	public class DebugIoAdapter : VanillaIoAdapter
	{
		internal static int counter;

		private static readonly int[] RangeOfInterest = new int[] { 0, 20 };

		public DebugIoAdapter(IoAdapter delegateAdapter) : base(delegateAdapter)
		{
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		protected DebugIoAdapter(IoAdapter delegateAdapter, string path, bool lockFile, long
			 initialLength, bool readOnly) : base(delegateAdapter.Open(path, lockFile, initialLength
			, readOnly))
		{
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override IoAdapter Open(string path, bool lockFile, long initialLength, bool
			 readOnly)
		{
			return new Db4objects.Db4o.IO.DebugIoAdapter(new RandomAccessFileAdapter(), path, 
				lockFile, initialLength, readOnly);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Seek(long pos)
		{
			if (pos >= RangeOfInterest[0] && pos <= RangeOfInterest[1])
			{
				counter++;
				Sharpen.Runtime.Out.WriteLine("seek: " + pos + "  counter: " + counter);
			}
			base.Seek(pos);
		}
	}
}
