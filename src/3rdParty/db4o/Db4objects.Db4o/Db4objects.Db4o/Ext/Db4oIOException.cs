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
	/// This exception is thrown when a system IO exception
	/// is encounted by db4o process.
	/// </summary>
	/// <remarks>
	/// db4o-specific exception.<br /><br />
	/// This exception is thrown when a system IO exception
	/// is encounted by db4o process.
	/// </remarks>
	[System.Serializable]
	public class Db4oIOException : Db4oFatalException
	{
		/// <summary>Constructor.</summary>
		/// <remarks>Constructor.</remarks>
		public Db4oIOException() : base()
		{
		}

		public Db4oIOException(string message) : base(message)
		{
		}

		/// <summary>Constructor allowing to specify the causing exception</summary>
		/// <param name="cause">exception cause</param>
		public Db4oIOException(Exception cause) : base(cause.Message, cause)
		{
		}
	}
}
