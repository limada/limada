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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Freespace
{
	/// <exclude></exclude>
	public abstract class SlotHandler : IIndexable4
	{
		protected Slot _current;

		public virtual void DefragIndexEntry(DefragmentContextImpl context)
		{
			throw new NotImplementedException();
		}

		public virtual int LinkLength()
		{
			return Slot.MarshalledLength;
		}

		public virtual object ReadIndexEntry(IContext context, ByteArrayBuffer reader)
		{
			return new Slot(reader.ReadInt(), reader.ReadInt());
		}

		public virtual void WriteIndexEntry(IContext context, ByteArrayBuffer writer, object
			 obj)
		{
			Slot slot = (Slot)obj;
			writer.WriteInt(slot.Address());
			writer.WriteInt(slot.Length());
		}

		public abstract IPreparedComparison PrepareComparison(IContext arg1, object arg2);
	}
}
