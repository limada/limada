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

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class SerializedGraph
	{
		public readonly int _id;

		public readonly byte[] _bytes;

		public SerializedGraph(int id, byte[] bytes)
		{
			_id = id;
			_bytes = bytes;
		}

		public virtual int Length()
		{
			return _bytes.Length;
		}

		public virtual int MarshalledLength()
		{
			return (Const4.IntLength * 2) + Length();
		}

		public virtual void Write(ByteArrayBuffer buffer)
		{
			buffer.WriteInt(_id);
			buffer.WriteInt(Length());
			buffer.Append(_bytes);
		}

		public static Db4objects.Db4o.Internal.SerializedGraph Read(ByteArrayBuffer buffer
			)
		{
			int id = buffer.ReadInt();
			int length = buffer.ReadInt();
			return new Db4objects.Db4o.Internal.SerializedGraph(id, buffer.ReadBytes(length));
		}
	}
}
