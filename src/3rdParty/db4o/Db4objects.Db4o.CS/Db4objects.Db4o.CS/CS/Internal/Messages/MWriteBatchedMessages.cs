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
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MWriteBatchedMessages : MsgD, IServerSideMessage
	{
		public void ProcessAtServer()
		{
			IServerMessageDispatcher dispatcher = (IServerMessageDispatcher)MessageDispatcher
				();
			int count = ReadInt();
			Transaction ta = Transaction();
			for (int i = 0; i < count; i++)
			{
				StatefulBuffer writer = _payLoad.ReadStatefulBuffer();
				int messageId = writer.ReadInt();
				Msg message = Msg.GetMessage(messageId);
				Msg clonedMessage = message.PublicClone();
				clonedMessage.SetMessageDispatcher(MessageDispatcher());
				clonedMessage.SetTransaction(ta);
				if (clonedMessage is MsgD)
				{
					MsgD msgd = (MsgD)clonedMessage;
					msgd.PayLoad(writer);
					if (msgd.PayLoad() != null)
					{
						msgd.PayLoad().IncrementOffset(Const4.IntLength);
						Transaction t = CheckParentTransaction(ta, msgd.PayLoad());
						msgd.SetTransaction(t);
						dispatcher.ProcessMessage(msgd);
					}
				}
				else
				{
					dispatcher.ProcessMessage(clonedMessage);
				}
			}
		}
	}
}
