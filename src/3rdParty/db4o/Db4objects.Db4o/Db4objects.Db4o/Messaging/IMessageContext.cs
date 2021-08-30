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
using Db4objects.Db4o.Messaging;

namespace Db4objects.Db4o.Messaging
{
	/// <summary>Additional message-related information.</summary>
	/// <remarks>Additional message-related information.</remarks>
	public interface IMessageContext
	{
		/// <summary>The container the message was dispatched to.</summary>
		/// <remarks>The container the message was dispatched to.</remarks>
		IObjectContainer Container
		{
			get;
		}

		/// <summary>The sender of the current message.</summary>
		/// <remarks>
		/// The sender of the current message.
		/// The reference can be used to send a reply to it.
		/// </remarks>
		IMessageSender Sender
		{
			get;
		}

		/// <summary>The transaction the current message has been sent with.</summary>
		/// <remarks>The transaction the current message has been sent with.</remarks>
		Db4objects.Db4o.Internal.Transaction Transaction
		{
			get;
		}
	}
}
