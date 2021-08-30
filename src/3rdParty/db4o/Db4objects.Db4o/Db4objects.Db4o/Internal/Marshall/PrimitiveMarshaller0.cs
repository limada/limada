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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	public class PrimitiveMarshaller0 : PrimitiveMarshaller
	{
		public override bool UseNormalClassRead()
		{
			return true;
		}

		public override DateTime ReadDate(ByteArrayBuffer bytes)
		{
			long value = bytes.ReadLong();
			if (value == long.MaxValue)
			{
				return MarshallingConstants0.NullDate;
			}
			return new DateTime(value);
		}

		public override object ReadInteger(ByteArrayBuffer bytes)
		{
			int value = bytes.ReadInt();
			if (value == int.MaxValue)
			{
				return null;
			}
			return value;
		}

		public override object ReadFloat(ByteArrayBuffer bytes)
		{
			float value = UnmarshallFloat(bytes);
			if (float.IsNaN(value))
			{
				return null;
			}
			return value;
		}

		public override object ReadDouble(ByteArrayBuffer buffer)
		{
			double value = UnmarshalDouble(buffer);
			if (double.IsNaN(value))
			{
				return null;
			}
			return value;
		}

		public override object ReadLong(ByteArrayBuffer buffer)
		{
			long value = buffer.ReadLong();
			if (value == long.MaxValue)
			{
				return null;
			}
			return value;
		}

		public override object ReadShort(ByteArrayBuffer buffer)
		{
			short value = UnmarshallShort(buffer);
			if (value == short.MaxValue)
			{
				return null;
			}
			return value;
		}

		public static double UnmarshalDouble(ByteArrayBuffer buffer)
		{
			return Platform4.LongToDouble(buffer.ReadLong());
		}

		public static float UnmarshallFloat(ByteArrayBuffer buffer)
		{
			return Sharpen.Runtime.IntBitsToFloat(buffer.ReadInt());
		}

		public static short UnmarshallShort(ByteArrayBuffer buffer)
		{
			int ret = 0;
			for (int i = 0; i < Const4.ShortBytes; i++)
			{
				ret = (ret << 8) + (buffer._buffer[buffer._offset++] & unchecked((int)(0xff)));
			}
			return (short)ret;
		}
	}
}
