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
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.CS.Internal.Objectexchange;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MReadMultipleObjects : MsgD, IMessageWithResponse
	{
		public Msg ReplyFromServer()
		{
			int prefetchDepth = ReadInt();
			int prefetchCount = ReadInt();
			IIntIterator4 ids = new _FixedSizeIntIterator4Base_14(this, prefetchCount);
			ByteArrayBuffer buffer = MarshallObjects(prefetchDepth, prefetchCount, ids);
			return Msg.ReadMultipleObjects.GetWriterForBuffer(Transaction(), buffer);
		}

		private sealed class _FixedSizeIntIterator4Base_14 : FixedSizeIntIterator4Base
		{
			public _FixedSizeIntIterator4Base_14(MReadMultipleObjects _enclosing, int baseArg1
				) : base(baseArg1)
			{
				this._enclosing = _enclosing;
			}

			protected override int NextInt()
			{
				return this._enclosing.ReadInt();
			}

			private readonly MReadMultipleObjects _enclosing;
		}

		private ByteArrayBuffer MarshallObjects(int prefetchDepth, int prefetchCount, IIntIterator4
			 ids)
		{
			lock (ContainerLock())
			{
				IObjectExchangeStrategy strategy = ObjectExchangeStrategyFactory.ForConfig(new ObjectExchangeConfiguration
					(prefetchDepth, prefetchCount));
				return strategy.Marshall((LocalTransaction)Transaction(), ids, prefetchCount);
			}
		}
	}
}
