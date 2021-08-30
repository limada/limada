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
using Db4objects.Db4o.CS.Caching;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Objectexchange;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Objectexchange
{
	public class DeferredObjectExchangeStrategy : IObjectExchangeStrategy
	{
		public static readonly IObjectExchangeStrategy Instance = new DeferredObjectExchangeStrategy
			();

		public virtual ByteArrayBuffer Marshall(LocalTransaction transaction, IIntIterator4
			 ids, int count)
		{
			ByteArrayBuffer buffer = new ByteArrayBuffer(Const4.IntLength + count * Const4.IntLength
				);
			int sizeOffset = buffer.Offset();
			buffer.WriteInt(0);
			int written = 0;
			while (count > 0 && ids.MoveNext())
			{
				buffer.WriteInt(ids.CurrentInt());
				++written;
				--count;
			}
			buffer.Seek(sizeOffset);
			buffer.WriteInt(written);
			return buffer;
		}

		public virtual IFixedSizeIntIterator4 Unmarshall(ClientTransaction transaction, IClientSlotCache
			 slotCache, ByteArrayBuffer reader)
		{
			int size = reader.ReadInt();
			return new _IFixedSizeIntIterator4_34(size, reader);
		}

		private sealed class _IFixedSizeIntIterator4_34 : IFixedSizeIntIterator4
		{
			public _IFixedSizeIntIterator4_34(int size, ByteArrayBuffer reader)
			{
				this.size = size;
				this.reader = reader;
				this._available = size;
			}

			internal int _current;

			internal int _available;

			public int Size()
			{
				return size;
			}

			public int CurrentInt()
			{
				return this._current;
			}

			public object Current
			{
				get
				{
					return this._current;
				}
			}

			public bool MoveNext()
			{
				if (this._available > 0)
				{
					this._current = reader.ReadInt();
					--this._available;
					return true;
				}
				return false;
			}

			public void Reset()
			{
				throw new NotSupportedException();
			}

			private readonly int size;

			private readonly ByteArrayBuffer reader;
		}
	}
}
