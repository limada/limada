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
	/// This exception is thrown when the supplied object ID
	/// is incorrect (outside the scope of the database IDs).
	/// </summary>
	/// <remarks>
	/// db4o-specific exception.<br /><br />
	/// This exception is thrown when the supplied object ID
	/// is incorrect (outside the scope of the database IDs).
	/// </remarks>
	/// <seealso cref="IExtObjectContainer.Bind(object, long)">IExtObjectContainer.Bind(object, long)
	/// 	</seealso>
	/// <seealso cref="IExtObjectContainer.GetByID(long)">IExtObjectContainer.GetByID(long)
	/// 	</seealso>
	[System.Serializable]
	public class InvalidIDException : Db4oRecoverableException
	{
		/// <summary>Constructor allowing to specify the exception cause</summary>
		/// <param name="cause">cause exception</param>
		public InvalidIDException(Exception cause) : base(cause)
		{
		}

		/// <summary>Constructor allowing to specify the offending id</summary>
		/// <param name="id">the offending id</param>
		public InvalidIDException(int id) : base("id: " + id)
		{
		}
	}
}
