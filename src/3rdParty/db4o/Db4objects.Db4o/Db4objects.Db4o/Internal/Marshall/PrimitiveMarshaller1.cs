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
	public class PrimitiveMarshaller1 : PrimitiveMarshaller
	{
		public override bool UseNormalClassRead()
		{
			return false;
		}

		public override DateTime ReadDate(ByteArrayBuffer bytes)
		{
			return new DateTime(bytes.ReadLong());
		}

		public override object ReadInteger(ByteArrayBuffer bytes)
		{
			return bytes.ReadInt();
		}

		public override object ReadFloat(ByteArrayBuffer bytes)
		{
			return PrimitiveMarshaller0.UnmarshallFloat(bytes);
		}

		public override object ReadDouble(ByteArrayBuffer buffer)
		{
			return PrimitiveMarshaller0.UnmarshalDouble(buffer);
		}

		public override object ReadLong(ByteArrayBuffer buffer)
		{
			return buffer.ReadLong();
		}

		public override object ReadShort(ByteArrayBuffer buffer)
		{
			return PrimitiveMarshaller0.UnmarshallShort(buffer);
		}
	}
}
