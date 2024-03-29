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
namespace Db4objects.Db4o.Marshall
{
	/// <summary>a reserved buffer within a write buffer.</summary>
	/// <remarks>
	/// a reserved buffer within a write buffer.
	/// The usecase this class was written for: A null bitmap should be at the
	/// beginning of a slot to allow lazy processing. During writing the content
	/// of the null bitmap is not yet fully known until all members are processed.
	/// With the Reservedbuffer the space in the slot can be occupied and writing
	/// can happen after all members are processed.
	/// </remarks>
	public interface IReservedBuffer
	{
		/// <summary>writes a byte array to the reserved buffer.</summary>
		/// <remarks>writes a byte array to the reserved buffer.</remarks>
		/// <param name="bytes">the byte array.</param>
		void WriteBytes(byte[] bytes);
	}
}
