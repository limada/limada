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
using Db4objects.Db4o;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Messaging;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public sealed class MUserMessage : MsgObject, IServerSideMessage, IClientSideMessage
	{
		public void ProcessAtServer()
		{
			ProcessUserMessage();
		}

		public bool ProcessAtClient()
		{
			return ProcessUserMessage();
		}

		private class MessageContextImpl : IMessageContext
		{
			public virtual IMessageSender Sender
			{
				get
				{
					return new _IMessageSender_22(this);
				}
			}

			private sealed class _IMessageSender_22 : IMessageSender
			{
				public _IMessageSender_22(MessageContextImpl _enclosing)
				{
					this._enclosing = _enclosing;
				}

				public void Send(object message)
				{
					this._enclosing._enclosing.ServerMessageDispatcher().Write(Msg.UserMessage.MarshallUserMessage
						(this._enclosing.Transaction, message));
				}

				private readonly MessageContextImpl _enclosing;
			}

			public virtual IObjectContainer Container
			{
				get
				{
					return this.Transaction.ObjectContainer();
				}
			}

			public virtual Db4objects.Db4o.Internal.Transaction Transaction
			{
				get
				{
					return this._enclosing.Transaction();
				}
			}

			internal MessageContextImpl(MUserMessage _enclosing)
			{
				this._enclosing = _enclosing;
			}

			private readonly MUserMessage _enclosing;
		}

		private bool ProcessUserMessage()
		{
			IMessageRecipient recipient = MessageRecipient();
			if (recipient == null)
			{
				return true;
			}
			try
			{
				recipient.ProcessMessage(new MUserMessage.MessageContextImpl(this), ReadUserMessage
					());
			}
			catch (Exception x)
			{
				// TODO: use MessageContext.sender() to send
				// error back to client
				Sharpen.Runtime.PrintStackTrace(x);
			}
			return true;
		}

		private object ReadUserMessage()
		{
			Unmarshall();
			return ((MUserMessage.UserMessagePayload)ReadObjectFromPayLoad()).message;
		}

		private IMessageRecipient MessageRecipient()
		{
			return Config().MessageRecipient();
		}

		public sealed class UserMessagePayload
		{
			public object message;

			public UserMessagePayload()
			{
			}

			public UserMessagePayload(object message_)
			{
				message = message_;
			}
		}

		public Msg MarshallUserMessage(Transaction transaction, object message)
		{
			return GetWriter(Serializer.Marshall(transaction, new MUserMessage.UserMessagePayload
				(message)));
		}
	}
}
