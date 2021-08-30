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

namespace Db4objects.Db4o.Internal
{
	/// <summary>
	/// db4o-specific exception.<br />
	/// <br />
	/// This exception is thrown when one of the db4o reflection methods fails.
	/// </summary>
	/// <remarks>
	/// db4o-specific exception.<br />
	/// <br />
	/// This exception is thrown when one of the db4o reflection methods fails.
	/// </remarks>
	/// <seealso cref="Db4objects.Db4o.Reflect">Db4objects.Db4o.Reflect</seealso>
	[System.Serializable]
	public class ReflectException : Db4oRecoverableException
	{
		public ReflectException(string msg, Exception cause) : base(msg, cause)
		{
		}

		/// <summary>Constructor with the cause exception</summary>
		/// <param name="cause">cause exception</param>
		public ReflectException(Exception cause) : base(cause)
		{
		}

		/// <summary>Constructor with message</summary>
		/// <param name="message">detailed explanation</param>
		public ReflectException(string message) : base(message)
		{
		}
	}
}
