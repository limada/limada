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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Encoding;
using Db4objects.Db4o.Marshall;
using Sharpen;

namespace Db4objects.Db4o.Internal.Encoding
{
	/// <exclude></exclude>
	public class LatinStringIO
	{
		public virtual byte[] Bytes(ByteArrayBuffer buffer)
		{
			int len = buffer.ReadInt();
			len = BytesPerChar() * len;
			byte[] res = new byte[len];
			System.Array.Copy(buffer._buffer, buffer._offset, res, 0, len);
			return res;
		}

		protected virtual int BytesPerChar()
		{
			return 1;
		}

		public virtual byte EncodingByte()
		{
			return BuiltInStringEncoding.EncodingByteForEncoding(new LatinStringEncoding());
		}

		public virtual int Length(string str)
		{
			return str.Length + Const4.ObjectLength + Const4.IntLength;
		}

		public virtual string Read(IReadBuffer buffer, int length)
		{
			char[] chars = new char[length];
			for (int ii = 0; ii < length; ii++)
			{
				chars[ii] = (char)(buffer.ReadByte() & unchecked((int)(0xff)));
			}
			return new string(chars, 0, length);
		}

		public virtual string Read(byte[] bytes)
		{
			char[] chars = new char[bytes.Length];
			for (int i = 0; i < bytes.Length; i++)
			{
				chars[i] = (char)(bytes[i] & unchecked((int)(0xff)));
			}
			return new string(chars, 0, bytes.Length);
		}

		public virtual string ReadLengthAndString(IReadBuffer buffer)
		{
			if(buffer==null)
                return string.Empty;
            int length = buffer.ReadInt();
			if (length == 0)
			{
				return string.Empty;
			}
			return Read(buffer, length);
		}

		public virtual int ShortLength(string str)
		{
			return str.Length + Const4.IntLength;
		}

		public virtual void Write(IWriteBuffer buffer, string str)
		{
			int length = str.Length;
			char[] chars = new char[length];
			Sharpen.Runtime.GetCharsForString(str, 0, length, chars, 0);
			for (int i = 0; i < length; i++)
			{
				buffer.WriteByte((byte)(chars[i] & unchecked((int)(0xff))));
			}
		}

		public virtual byte[] Write(string str)
		{
			int length = str.Length;
			char[] chars = new char[length];
			Sharpen.Runtime.GetCharsForString(str, 0, length, chars, 0);
			byte[] bytes = new byte[length];
			for (int i = 0; i < length; i++)
			{
				bytes[i] = (byte)(chars[i] & unchecked((int)(0xff)));
			}
			return bytes;
		}

		public virtual void WriteLengthAndString(IWriteBuffer buffer, string str)
		{
			if (str == null)
			{
				buffer.WriteInt(0);
				return;
			}
			buffer.WriteInt(str.Length);
			Write(buffer, str);
		}
	}
}
