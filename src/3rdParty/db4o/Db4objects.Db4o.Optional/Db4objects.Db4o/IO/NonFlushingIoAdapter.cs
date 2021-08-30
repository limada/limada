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
	/// <summary>
	/// Delegating IoAdapter that does not pass on calls to sync
	/// data to the underlying device.
	/// </summary>
	/// <remarks>
	/// Delegating IoAdapter that does not pass on calls to sync
	/// data to the underlying device. <br /><br />
	/// This IoAdapter can be used to improve performance at the cost of a
	/// higher risk of database file corruption upon abnormal termination
	/// of a session against a database.<br /><br />
	/// An example of possible usage:<br />
	/// <code>
	/// RandomAccessFileAdapter randomAccessFileAdapter = new RandomAccessFileAdapter();<br />
	/// NonFlushingIoAdapter nonFlushingIoAdapter = new NonFlushingIoAdapter(randomAccessFileAdapter);<br />
	/// CachedIoAdapter cachedIoAdapter = new CachedIoAdapter(nonFlushingIoAdapter);<br />
	/// Configuration configuration = Db4o.newConfiguration();<br />
	/// configuration.io(cachedIoAdapter);<br />
	/// </code>
	/// <br /><br />
	/// db4o uses a resume-commit-on-crash strategy to ensure ACID transactions.
	/// When a transaction commits,<br />
	/// - (1) a list "pointers that are to be modified" is written to the database file,<br />
	/// - (2) the database file is switched into "in-commit" mode, <br />
	/// - (3) the pointers are actually modified in the database file,<br />
	/// - (4) the database file is switched to "not-in-commit" mode.<br />
	/// If the system is halted by a hardware or power failure <br />
	/// - before (2)<br />
	/// all objects will be available as before the commit<br />
	/// - between (2) and (4)
	/// the commit is restarted when the database file is opened the next time, all pointers
	/// will be read from the "pointers to be modified" list and all of them will be modified
	/// to the state they are intended to have after commit<br />
	/// - after (4)
	/// no work is necessary, the transaction is committed.
	/// <br /><br />
	/// In order for the above to be 100% failsafe, the order of writes to the
	/// storage medium has to be obeyed. On operating systems that use in-memory
	/// file caching, the OS cache may revert the order of writes to optimize
	/// file performance.<br /><br />
	/// db4o enforces the correct write order by calling
	/// <see cref="Sync()">Sync()</see>
	/// after every single one of the above steps during transaction
	/// commit. The calls to
	/// <see cref="Sync()">Sync()</see>
	/// have a high performance cost.
	/// By using this IoAdapter it is possible to omit these calls, at the cost
	/// of a risc of corrupted database files upon hardware-, power- or operating
	/// system failures.<br /><br />
	/// </remarks>
	public class NonFlushingIoAdapter : VanillaIoAdapter
	{
		public NonFlushingIoAdapter(IoAdapter delegateAdapter) : base(delegateAdapter)
		{
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		private NonFlushingIoAdapter(IoAdapter delegateAdapter, string path, bool lockFile
			, long initialLength, bool readOnly) : base(delegateAdapter, path, lockFile, initialLength
			, readOnly)
		{
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override IoAdapter Open(string path, bool lockFile, long initialLength, bool
			 readOnly)
		{
			return new Db4objects.Db4o.IO.NonFlushingIoAdapter(_delegate, path, lockFile, initialLength
				, readOnly);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Sync()
		{
		}
		// do nothing 
	}
}
