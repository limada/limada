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
using Db4objects.Db4o;

namespace Db4objects.Db4o
{
	/// <summary>
	/// Represents a local ObjectContainer attached to a
	/// database file.
	/// </summary>
	/// <remarks>
	/// Represents a local ObjectContainer attached to a
	/// database file.
	/// </remarks>
	/// <since>7.10</since>
	public interface IEmbeddedObjectContainer : IObjectContainer
	{
		/// <summary>backs up a database file of an open ObjectContainer.</summary>
		/// <remarks>
		/// backs up a database file of an open ObjectContainer.
		/// <br /><br />While the backup is running, the ObjectContainer can continue to be
		/// used. Changes that are made while the backup is in progress, will be applied to
		/// the open ObjectContainer and to the backup.<br /><br />
		/// While the backup is running, the ObjectContainer should not be closed.<br /><br />
		/// If a file already exists at the specified path, it will be overwritten.<br /><br />
		/// The
		/// <see cref="Db4objects.Db4o.IO.IStorage">Db4objects.Db4o.IO.IStorage</see>
		/// used for backup is the one configured for this container.
		/// </remarks>
		/// <param name="path">a fully qualified path</param>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException">db4o database file was closed or failed to open.
		/// 	</exception>
		/// <exception cref="System.NotSupportedException">
		/// is thrown when the operation is not supported in current
		/// configuration/environment
		/// </exception>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException">I/O operation failed or was unexpectedly interrupted.
		/// 	</exception>
		void Backup(string path);
	}
}
