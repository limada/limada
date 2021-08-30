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
namespace Db4objects.Db4o.Config.Encoding
{
	/// <summary>
	/// encodes a String to a byte array and decodes a String
	/// from a part of a byte array
	/// </summary>
	public interface IStringEncoding
	{
		/// <summary>called when a string is to be encoded to a byte array.</summary>
		/// <remarks>called when a string is to be encoded to a byte array.</remarks>
		/// <param name="str">the string to encode</param>
		/// <returns>the encoded byte array</returns>
		byte[] Encode(string str);

		/// <summary>called when a byte array is to be decoded to a string.</summary>
		/// <remarks>called when a byte array is to be decoded to a string.</remarks>
		/// <param name="bytes">the byte array</param>
		/// <param name="start">the start offset in the byte array</param>
		/// <param name="length">the length of the encoded string in the byte array</param>
		/// <returns>the string</returns>
		string Decode(byte[] bytes, int start, int length);
	}
}
