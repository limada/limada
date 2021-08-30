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
using Db4objects.Db4o.Config.Encoding;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Encoding;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Encoding
{
	/// <exclude></exclude>
	public class DelegatingStringIO : LatinStringIO
	{
		private readonly IStringEncoding _encoding;

		public DelegatingStringIO(IStringEncoding encoding)
		{
			_encoding = encoding;
		}

		private string Decode(byte[] bytes, int start, int length)
		{
			return _encoding.Decode(bytes, start, length);
		}

		private byte[] Encode(string str)
		{
			return _encoding.Encode(str);
		}

		public override byte EncodingByte()
		{
			if (_encoding is BuiltInStringEncoding)
			{
				return BuiltInStringEncoding.EncodingByteForEncoding(_encoding);
			}
			return 0;
		}

		public override int Length(string str)
		{
			return Encode(str).Length + Const4.ObjectLength + Const4.IntLength;
		}

		public override string Read(IReadBuffer buffer, int length)
		{
			byte[] bytes = new byte[length];
			buffer.ReadBytes(bytes);
			return Decode(bytes, 0, bytes.Length);
		}

		public override string Read(byte[] bytes)
		{
			return Decode(bytes, 0, bytes.Length);
		}

		public override int ShortLength(string str)
		{
			return Encode(str).Length + Const4.IntLength;
		}

		public override void Write(IWriteBuffer buffer, string str)
		{
			buffer.WriteBytes(Encode(str));
		}

		public override byte[] Write(string str)
		{
			return Encode(str);
		}

		/// <summary>
		/// Note the different implementation when compared to LatinStringIO and UnicodeStringIO:
		/// Instead of writing the length of the string, UTF8StringIO writes the length of the
		/// byte array.
		/// </summary>
		/// <remarks>
		/// Note the different implementation when compared to LatinStringIO and UnicodeStringIO:
		/// Instead of writing the length of the string, UTF8StringIO writes the length of the
		/// byte array.
		/// </remarks>
		public override void WriteLengthAndString(IWriteBuffer buffer, string str)
		{
			if (str == null)
			{
				buffer.WriteInt(0);
				return;
			}
			byte[] bytes = Encode(str);
			buffer.WriteInt(bytes.Length);
			buffer.WriteBytes(bytes);
		}
	}
}
