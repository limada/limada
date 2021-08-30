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

namespace Db4objects.Db4o.Ext
{
	/// <summary>
	/// db4o exception wrapper: Exceptions occurring during internal processing
	/// will be proliferated to the client calling code encapsulated in an exception
	/// of this type.
	/// </summary>
	/// <remarks>
	/// db4o exception wrapper: Exceptions occurring during internal processing
	/// will be proliferated to the client calling code encapsulated in an exception
	/// of this type. The original exception, if any, is available through
	/// Db4oException#getCause().
	/// </remarks>
	[System.Serializable]
	public class Db4oException : Exception
	{
		/// <summary>Simple constructor</summary>
		public Db4oException() : this(null, null)
		{
		}

		/// <summary>Constructor with an exception message specified</summary>
		/// <param name="msg">exception message</param>
		public Db4oException(string msg) : this(msg, null)
		{
		}

		/// <summary>Constructor with an exception cause specified</summary>
		/// <param name="cause">exception cause</param>
		public Db4oException(Exception cause) : this(null, cause)
		{
		}

		/// <summary>
		/// Constructor with an exception message selected
		/// from the internal message collection.
		/// </summary>
		/// <remarks>
		/// Constructor with an exception message selected
		/// from the internal message collection.
		/// </remarks>
		/// <param name="messageConstant">internal db4o message number</param>
		public Db4oException(int messageConstant) : this(Db4objects.Db4o.Internal.Messages
			.Get(messageConstant))
		{
		}

		/// <summary>Constructor with an exception message and cause specified</summary>
		/// <param name="msg">exception message</param>
		/// <param name="cause">exception cause</param>
		public Db4oException(string msg, Exception cause) : base(msg, cause)
		{
		}
	}
}
