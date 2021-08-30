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
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public sealed class MReadSlot : MsgD, IMessageWithResponse
	{
		public sealed override ByteArrayBuffer GetByteLoad()
		{
			int address = _payLoad.ReadInt();
			int length = _payLoad.Length() - (Const4.IntLength);
			Slot slot = new Slot(address, length);
			_payLoad.RemoveFirstBytes(Const4.IntLength);
			_payLoad.UseSlot(slot);
			return this._payLoad;
		}

		public sealed override MsgD GetWriter(StatefulBuffer bytes)
		{
			MsgD message = GetWriterForLength(bytes.Transaction(), bytes.Length() + Const4.IntLength
				);
			message._payLoad.WriteInt(bytes.GetAddress());
			message._payLoad.Append(bytes._buffer);
			return message;
		}

		public Msg ReplyFromServer()
		{
			int address = ReadInt();
			int length = ReadInt();
			lock (ContainerLock())
			{
				StatefulBuffer bytes = new StatefulBuffer(this.Transaction(), address, length);
				try
				{
					Container().ReadBytes(bytes._buffer, address, length);
					return GetWriter(bytes);
				}
				catch (Exception)
				{
					// TODO: not nicely handled on the client side yet
					return Msg.Null;
				}
			}
		}
	}
}
