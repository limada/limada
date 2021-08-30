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
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public abstract class AbstractBufferContext : IBufferContext, IHandlerVersionContext
	{
		private IReadBuffer _buffer;

		private readonly Db4objects.Db4o.Internal.Transaction _transaction;

		public AbstractBufferContext(Db4objects.Db4o.Internal.Transaction transaction, IReadBuffer
			 buffer)
		{
			_transaction = transaction;
			_buffer = buffer;
		}

		public virtual IReadBuffer Buffer(IReadBuffer buffer)
		{
			IReadBuffer temp = _buffer;
			_buffer = buffer;
			return temp;
		}

		public virtual IReadBuffer Buffer()
		{
			return _buffer;
		}

		public virtual byte ReadByte()
		{
			return _buffer.ReadByte();
		}

		public virtual void ReadBytes(byte[] bytes)
		{
			_buffer.ReadBytes(bytes);
		}

		public virtual int ReadInt()
		{
			return _buffer.ReadInt();
		}

		public virtual long ReadLong()
		{
			return _buffer.ReadLong();
		}

		public virtual int Offset()
		{
			return _buffer.Offset();
		}

		public virtual void Seek(int offset)
		{
			_buffer.Seek(offset);
		}

		public virtual ObjectContainerBase Container()
		{
			return _transaction.Container();
		}

		public virtual IObjectContainer ObjectContainer()
		{
			return Container();
		}

		public virtual Db4objects.Db4o.Internal.Transaction Transaction()
		{
			return _transaction;
		}

		public abstract int HandlerVersion();

		public virtual bool IsLegacyHandlerVersion()
		{
			return HandlerVersion() == 0;
		}

		public virtual BitMap4 ReadBitMap(int bitCount)
		{
			return _buffer.ReadBitMap(bitCount);
		}

		public virtual Db4objects.Db4o.Internal.Marshall.SlotFormat SlotFormat()
		{
			return Db4objects.Db4o.Internal.Marshall.SlotFormat.ForHandlerVersion(HandlerVersion
				());
		}
	}
}
