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
using Db4objects.Db4o.Foundation;
using Sharpen.Lang;

namespace Db4objects.Db4o.CS.Internal
{
	internal class BlobProcessor : IRunnable
	{
		private ClientObjectContainer stream;

		private IQueue4 queue = new NonblockingQueue();

		private bool terminated = false;

		internal BlobProcessor(ClientObjectContainer aStream)
		{
			stream = aStream;
		}

		internal virtual void Add(MsgBlob msg)
		{
			lock (queue)
			{
				queue.Add(msg);
			}
		}

		internal virtual bool IsTerminated()
		{
			lock (this)
			{
				return terminated;
			}
		}

		public virtual void Run()
		{
			try
			{
				Socket4Adapter socket = stream.CreateParallelSocket();
				MsgBlob msg = null;
				// no blobLock synchronisation here, since our first msg is valid
				lock (queue)
				{
					msg = (MsgBlob)queue.Next();
				}
				while (msg != null)
				{
					msg.Write(socket);
					msg.ProcessClient(socket);
					lock (stream._blobLock)
					{
						lock (queue)
						{
							msg = (MsgBlob)queue.Next();
						}
						if (msg == null)
						{
							terminated = true;
							Msg.CloseSocket.Write(socket);
							try
							{
								socket.Close();
							}
							catch (Exception)
							{
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				Sharpen.Runtime.PrintStackTrace(e);
			}
		}
	}
}
