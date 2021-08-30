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
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Sharpen.IO;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MReadBlob : MsgBlob, IServerSideMessage
	{
		/// <exception cref="System.IO.IOException"></exception>
		public override void ProcessClient(Socket4Adapter sock)
		{
			Msg message = Msg.ReadMessage(MessageDispatcher(), Transaction(), sock);
			if (message.Equals(Msg.Length))
			{
				try
				{
					_currentByte = 0;
					_length = message.PayLoad().ReadInt();
					_blob.GetStatusFrom(this);
					_blob.SetStatus(Status.Processing);
					Copy(sock, this._blob.GetClientOutputStream(), _length, true);
					message = Msg.ReadMessage(MessageDispatcher(), Transaction(), sock);
					if (message.Equals(Msg.Ok))
					{
						this._blob.SetStatus(Status.Completed);
					}
					else
					{
						this._blob.SetStatus(Status.Error);
					}
				}
				catch (Exception)
				{
				}
			}
			else
			{
				if (message.Equals(Msg.Error))
				{
					this._blob.SetStatus(Status.Error);
				}
			}
		}

		public virtual void ProcessAtServer()
		{
			try
			{
				BlobImpl blobImpl = this.ServerGetBlobImpl();
				if (blobImpl != null)
				{
					blobImpl.SetTrans(Transaction());
					Sharpen.IO.File file = blobImpl.ServerFile(null, false);
					int length = (int)file.Length();
					Socket4Adapter sock = ServerMessageDispatcher().Socket();
					Msg.Length.GetWriterForInt(Transaction(), length).Write(sock);
					FileInputStream fin = new FileInputStream(file);
					Copy(fin, sock, false);
					sock.Flush();
					Msg.Ok.Write(sock);
				}
			}
			catch (Exception)
			{
				Write(Msg.Error);
			}
		}
	}
}
