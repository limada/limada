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
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public sealed class MGetClasses : MsgD, IMessageWithResponse
	{
		public Msg ReplyFromServer()
		{
			lock (ContainerLock())
			{
				try
				{
					// Since every new Client reads the class
					// collection from the file, we have to 
					// make sure, it has been written.
					Container().ClassCollection().Write(Transaction());
				}
				catch (Exception)
				{
				}
			}
			MsgD message = Msg.GetClasses.GetWriterForLength(Transaction(), Const4.IntLength 
				+ 1);
			ByteArrayBuffer writer = message.PayLoad();
			writer.WriteInt(Container().ClassCollection().GetID());
			writer.WriteByte(Container().StringIO().EncodingByte());
			return message;
		}
	}
}
