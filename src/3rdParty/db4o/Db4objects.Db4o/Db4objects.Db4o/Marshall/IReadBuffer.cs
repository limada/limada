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
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Marshall
{
	/// <summary>
	/// a buffer interface with methods to read and to position
	/// the read pointer in the buffer.
	/// </summary>
	/// <remarks>
	/// a buffer interface with methods to read and to position
	/// the read pointer in the buffer.
	/// </remarks>
	public interface IReadBuffer
	{
		/// <summary>returns the current offset in the buffer</summary>
		/// <returns>the offset</returns>
		int Offset();

		BitMap4 ReadBitMap(int bitCount);

		/// <summary>reads a byte from the buffer.</summary>
		/// <remarks>reads a byte from the buffer.</remarks>
		/// <returns>the byte</returns>
		byte ReadByte();

		/// <summary>reads an array of bytes from the buffer.</summary>
		/// <remarks>
		/// reads an array of bytes from the buffer.
		/// The length of the array that is passed as a parameter specifies the
		/// number of bytes that are to be read. The passed bytes buffer parameter
		/// is directly filled.
		/// </remarks>
		/// <param name="bytes">the byte array to read the bytes into.</param>
		void ReadBytes(byte[] bytes);

		/// <summary>reads an int from the buffer.</summary>
		/// <remarks>reads an int from the buffer.</remarks>
		/// <returns>the int</returns>
		int ReadInt();

		/// <summary>reads a long from the buffer.</summary>
		/// <remarks>reads a long from the buffer.</remarks>
		/// <returns>the long</returns>
		long ReadLong();

		/// <summary>positions the read pointer at the specified position</summary>
		/// <param name="offset">the desired position in the buffer</param>
		void Seek(int offset);
	}
}
