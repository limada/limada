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
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Sharpen.Lang;

namespace Db4objects.Db4o.CS.Internal
{
	internal class ClientMessageDispatcherImpl : IRunnable, IClientMessageDispatcher
	{
		private ClientObjectContainer _container;

		private Socket4Adapter _socket;

		private readonly BlockingQueue _synchronousMessageQueue;

		private readonly BlockingQueue _asynchronousMessageQueue;

		private bool _isClosed;

		internal ClientMessageDispatcherImpl(ClientObjectContainer client, Socket4Adapter
			 a_socket, BlockingQueue synchronousMessageQueue, BlockingQueue asynchronousMessageQueue
			)
		{
			_container = client;
			_synchronousMessageQueue = synchronousMessageQueue;
			_asynchronousMessageQueue = asynchronousMessageQueue;
			_socket = a_socket;
		}

		public virtual bool IsMessageDispatcherAlive()
		{
			lock (this)
			{
				return !_isClosed;
			}
		}

		public virtual bool Close()
		{
			lock (this)
			{
				if (_isClosed)
				{
					return true;
				}
				_isClosed = true;
				if (_socket != null)
				{
					try
					{
						_socket.Close();
					}
					catch (Db4oIOException)
					{
					}
				}
				_synchronousMessageQueue.Stop();
				_asynchronousMessageQueue.Stop();
				return true;
			}
		}

		public virtual void Run()
		{
			MessageLoop();
			Close();
		}

		public virtual void MessageLoop()
		{
			while (IsMessageDispatcherAlive())
			{
				Msg message = null;
				try
				{
					message = Msg.ReadMessage(this, Transaction(), _socket);
				}
				catch (Db4oIOException exc)
				{
					if (DTrace.enabled)
					{
						DTrace.ClientMessageLoopException.Log(exc.ToString());
					}
					return;
				}
				if (message == null)
				{
					continue;
				}
				if (IsClientSideMessage(message))
				{
					_asynchronousMessageQueue.Add(message);
				}
				else
				{
					_synchronousMessageQueue.Add(message);
				}
			}
		}

		private bool IsClientSideMessage(Msg message)
		{
			return message is IClientSideMessage;
		}

		public virtual bool Write(Msg msg)
		{
			_container.Write(msg);
			return true;
		}

		private Db4objects.Db4o.Internal.Transaction Transaction()
		{
			return _container.Transaction;
		}
	}
}
