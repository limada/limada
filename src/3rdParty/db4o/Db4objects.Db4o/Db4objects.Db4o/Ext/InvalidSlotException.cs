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
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Ext
{
	/// <summary>
	/// db4o-specific exception.<br /><br />
	/// This exception is thrown when db4o reads slot
	/// information which is not valid (length or address).
	/// </summary>
	/// <remarks>
	/// db4o-specific exception.<br /><br />
	/// This exception is thrown when db4o reads slot
	/// information which is not valid (length or address).
	/// </remarks>
	[System.Serializable]
	public class InvalidSlotException : Db4oRecoverableException
	{
		/// <summary>Constructor allowing to specify a detailed message.</summary>
		/// <remarks>Constructor allowing to specify a detailed message.</remarks>
		/// <param name="msg">message</param>
		public InvalidSlotException(string msg) : base(msg)
		{
		}

		/// <summary>Constructor allowing to specify the address, length and id.</summary>
		/// <remarks>Constructor allowing to specify the address, length and id.</remarks>
		/// <param name="address">offending address</param>
		/// <param name="length">offending length</param>
		/// <param name="id">id where the address and length were read.</param>
		public InvalidSlotException(int address, int length, int id) : base("address: " +
			 address + ", length : " + length + ", id : " + id)
		{
		}
	}
}
