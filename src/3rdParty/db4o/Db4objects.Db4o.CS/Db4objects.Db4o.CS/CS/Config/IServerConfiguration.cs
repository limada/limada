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
using Db4objects.Db4o.Config;

namespace Db4objects.Db4o.CS.Config
{
	/// <summary>Configuration interface for db4o servers.</summary>
	/// <remarks>Configuration interface for db4o servers.</remarks>
	/// <since>7.5</since>
	public interface IServerConfiguration : IFileConfigurationProvider, INetworkingConfigurationProvider
		, ICommonConfigurationProvider, ICacheConfigurationProvider, IIdSystemConfigurationProvider
	{
		/// <summary>
		/// adds ConfigurationItems to be applied when
		/// an ObjectServer is opened.
		/// </summary>
		/// <remarks>
		/// adds ConfigurationItems to be applied when
		/// an ObjectServer is opened.
		/// </remarks>
		/// <param name="configItem">
		/// the
		/// <see cref="IServerConfigurationItem">IServerConfigurationItem</see>
		/// </param>
		/// <since>7.12</since>
		void AddConfigurationItem(IServerConfigurationItem configItem);

		/// <summary>configures the timeout of the server side socket.</summary>
		/// <remarks>
		/// configures the timeout of the server side socket. <br />
		/// <br />
		/// The server side handler waits for messages to arrive from the client.
		/// If no more messages arrive for the duration configured in this
		/// setting, the client will be disconnected.
		/// <br />
		/// Clients send PING messages to the server at an interval of
		/// Math.min(timeoutClientSocket(), timeoutServerSocket()) / 2
		/// and the server will respond to keep connections alive.
		/// <br />
		/// Decrease this setting if you want clients to disconnect faster.
		/// <br />
		/// Increase this setting if you have a large number of clients and long
		/// running queries and you are getting disconnected clients that you
		/// would like to wait even longer for a response from the server.
		/// <br />
		/// Default value: 600000ms (10 minutes)<br />
		/// <br />
		/// It is recommended to use the same values for
		/// <see cref="#timeoutClientSocket(int)">#timeoutClientSocket(int)</see>
		/// and
		/// <see cref="TimeoutServerSocket(int)">TimeoutServerSocket(int)</see>
		/// .
		/// <br />
		/// This setting can be used on both client and server.<br /><br />
		/// </remarks>
		/// <value>time in milliseconds</value>
		int TimeoutServerSocket
		{
			set;
		}
	}
}
