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
	/// <exclude></exclude>
	public class MObjectSetFetch : MObjectSet, IMessageWithResponse
	{
		public virtual Msg ReplyFromServer()
		{
			int queryResultID = ReadInt();
			int fetchSize = ReadInt();
			int fetchDepth = ReadInt();
			MsgD message = null;
			lock (ContainerLock())
			{
				IIntIterator4 idIterator = Stub(queryResultID).IdIterator();
				ByteArrayBuffer payload = ObjectExchangeStrategyFactory.ForConfig(new ObjectExchangeConfiguration
					(fetchDepth, fetchSize)).Marshall((LocalTransaction)Transaction(), idIterator, fetchSize
					);
				message = IdList.GetWriterForBuffer(Transaction(), payload);
			}
			return message;
		}
	}
}
