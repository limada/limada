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
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Ext
{
	/// <summary>
	/// db4o-specific exception.<br /><br />
	/// this Exception is thrown during any of the db4o open calls
	/// if the database file is locked by another process.
	/// </summary>
	/// <remarks>
	/// db4o-specific exception.<br /><br />
	/// this Exception is thrown during any of the db4o open calls
	/// if the database file is locked by another process.
	/// </remarks>
	/// <seealso cref="Db4objects.Db4o.Db4oFactory.OpenFile(string)">Db4objects.Db4o.Db4oFactory.OpenFile(string)
	/// 	</seealso>
	[System.Serializable]
	public class DatabaseFileLockedException : Db4oFatalException
	{
		/// <summary>Constructor with a database description message</summary>
		/// <param name="databaseDescription">message, which can help to identify the database
		/// 	</param>
		public DatabaseFileLockedException(string databaseDescription) : base(databaseDescription
			)
		{
		}

		/// <summary>Constructor with a database description and cause exception</summary>
		/// <param name="databaseDescription">database description</param>
		/// <param name="cause">previous exception caused DatabaseFileLockedException</param>
		public DatabaseFileLockedException(string databaseDescription, Exception cause) : 
			base(databaseDescription, cause)
		{
		}
	}
}
