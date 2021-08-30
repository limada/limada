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
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Foundation;
using Db4objects.Db4o.Messaging;

namespace Db4objects.Db4o.CS.Config
{
	/// <summary>
	/// Configuration interface for networking configuration settings.<br /><br />
	/// The network settings should be configured in exactly the same on the server and client.
	/// </summary>
	/// <remarks>
	/// Configuration interface for networking configuration settings.<br /><br />
	/// The network settings should be configured in exactly the same on the server and client.
	/// </remarks>
	/// <since>7.5</since>
	public interface INetworkingConfiguration
	{
		IClientServerFactory ClientServerFactory
		{
			get;
			set;
		}

		/// <summary>
		/// configures the client messaging system to be single threaded
		/// or multithreaded.
		/// </summary>
		/// <remarks>
		/// configures the client messaging system to be single threaded
		/// or multithreaded.
		/// <br /><br />Recommended settings:<br />
		/// - <code>true</code> for low resource systems.<br />
		/// - <code>false</code> for best asynchronous performance and fast
		/// GUI response.
		/// <br /><br />Default value:<br />
		/// - .NET Compact Framework: <code>true</code><br />
		/// - all other platforms: <code>false</code><br /><br />
		/// This setting can be used on both client and server.<br /><br />
		/// </remarks>
		/// <value>the desired setting</value>
		bool SingleThreadedClient
		{
			set;
		}

		/// <summary>Configures to batch messages between client and server.</summary>
		/// <remarks>
		/// Configures to batch messages between client and server. By default, batch
		/// mode is enabled.<br /><br />
		/// This setting can be used on both client and server.<br /><br />
		/// </remarks>
		/// <value>false, to turn message batching off.</value>
		bool BatchMessages
		{
			set;
		}

		/// <summary>Configures the maximum memory buffer size for batched message.</summary>
		/// <remarks>
		/// Configures the maximum memory buffer size for batched message. If the
		/// size of batched messages is greater than <code>maxSize</code>, batched
		/// messages will be sent to server.<br /><br />
		/// This setting can be used on both client and server.<br /><br />
		/// </remarks>
		/// <value></value>
		int MaxBatchQueueSize
		{
			set;
		}

		/// <summary>sets the MessageRecipient to receive Client Server messages.</summary>
		/// <remarks>
		/// sets the MessageRecipient to receive Client Server messages. <br />
		/// <br />
		/// This setting can be used on both client and server.<br /><br />
		/// </remarks>
		/// <value>the MessageRecipient to be used</value>
		IMessageRecipient MessageRecipient
		{
			set;
		}

		/// <since>7.11</since>
		/// <since>7.11</since>
		ISocket4Factory SocketFactory
		{
			get;
			set;
		}
	}
}
