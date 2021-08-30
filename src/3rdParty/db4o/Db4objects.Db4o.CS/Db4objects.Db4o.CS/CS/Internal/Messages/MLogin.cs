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
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	/// <exclude></exclude>
	public class MLogin : MsgD, IMessageWithResponse
	{
		public virtual Msg ReplyFromServer()
		{
			lock (ContainerLock())
			{
				string userName = ReadString();
				string password = ReadString();
				ObjectServerImpl server = ServerMessageDispatcher().Server();
				User found = server.GetUser(userName);
				if (found != null)
				{
					if (found.password.Equals(password))
					{
						ServerMessageDispatcher().SetDispatcherName(userName);
						LogMsg(32, userName);
						int blockSize = Container().BlockSize();
						int encrypt = Container()._handlers.i_encrypt ? 1 : 0;
						ServerMessageDispatcher().Login();
						return Msg.LoginOk.GetWriterForInts(Transaction(), new int[] { blockSize, encrypt
							, ServerMessageDispatcher().DispatcherID() });
					}
				}
			}
			return Msg.Failed;
		}
	}
}
