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
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public sealed class MCommit : Msg, IMessageWithResponse
	{
		private CallbackObjectInfoCollections committedInfo = null;

		public Msg ReplyFromServer()
		{
			IServerMessageDispatcher dispatcher = ServerMessageDispatcher();
			lock (ContainerLock())
			{
				ServerTransaction().Commit(dispatcher);
				committedInfo = dispatcher.CommittedInfo();
			}
			return Msg.Ok;
		}

		public override void PostProcessAtServer()
		{
			try
			{
				if (committedInfo != null)
				{
					AddCommittedInfoMsg(committedInfo, ServerTransaction());
				}
			}
			catch (Exception exc)
			{
				Sharpen.Runtime.PrintStackTrace(exc);
			}
		}

		private void AddCommittedInfoMsg(CallbackObjectInfoCollections committedInfo, LocalTransaction
			 serverTransaction)
		{
			lock (ContainerLock())
			{
				Msg.CommittedInfo.SetTransaction(serverTransaction);
				MCommittedInfo message = Msg.CommittedInfo.Encode(committedInfo, ServerMessageDispatcher
					().DispatcherID());
				message.SetMessageDispatcher(ServerMessageDispatcher());
				ServerMessageDispatcher().Server().AddCommittedInfoMsg(message);
			}
		}
	}
}
