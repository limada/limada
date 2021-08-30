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

namespace Db4objects.Db4o.Internal.Ids
{
	/// <exclude></exclude>
	public class IdSlotMapping
	{
		public int _id;

		public int _address;

		public int _length;

		public IdSlotMapping(int id, int address, int length)
		{
			// persistent and indexed in DatabaseIdMapping, don't change the name
			_id = id;
			_address = address;
			_length = length;
		}

		public IdSlotMapping(int id, Db4objects.Db4o.Internal.Slots.Slot slot) : this(id, 
			slot.Address(), slot.Length())
		{
		}

		public virtual Db4objects.Db4o.Internal.Slots.Slot Slot()
		{
			return new Db4objects.Db4o.Internal.Slots.Slot(_address, _length);
		}

		public virtual void Write(ByteArrayBuffer buffer)
		{
			buffer.WriteInt(_id);
			buffer.WriteInt(_address);
			buffer.WriteInt(_length);
		}

		public static Db4objects.Db4o.Internal.Ids.IdSlotMapping Read(ByteArrayBuffer buffer
			)
		{
			return new Db4objects.Db4o.Internal.Ids.IdSlotMapping(buffer.ReadInt(), buffer.ReadInt
				(), buffer.ReadInt());
		}

		public override string ToString()
		{
			return string.Empty + _id + ":" + _address + "," + _length;
		}
	}
}
