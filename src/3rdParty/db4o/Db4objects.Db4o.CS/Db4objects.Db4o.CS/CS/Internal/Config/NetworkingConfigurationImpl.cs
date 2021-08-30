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
using System.Collections;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Foundation;
using Db4objects.Db4o.CS.Internal.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Messaging;

namespace Db4objects.Db4o.CS.Internal.Config
{
	public class NetworkingConfigurationImpl : INetworkingConfiguration
	{
		protected readonly Config4Impl _config;

		internal NetworkingConfigurationImpl(Config4Impl config)
		{
			_config = config;
		}

		public virtual Config4Impl Config()
		{
			return _config;
		}

		public virtual bool BatchMessages
		{
			set
			{
				bool flag = value;
				_config.BatchMessages(flag);
			}
		}

		public virtual int MaxBatchQueueSize
		{
			set
			{
				int maxSize = value;
				_config.MaxBatchQueueSize(maxSize);
			}
		}

		public virtual bool SingleThreadedClient
		{
			set
			{
				bool flag = value;
				_config.SingleThreadedClient(flag);
			}
		}

		public virtual IMessageRecipient MessageRecipient
		{
			set
			{
				IMessageRecipient messageRecipient = value;
				_config.SetMessageRecipient(messageRecipient);
			}
		}

		public virtual IClientServerFactory ClientServerFactory
		{
			get
			{
				IClientServerFactory configuredFactory = ((IClientServerFactory)My(typeof(IClientServerFactory
					)));
				if (null == configuredFactory)
				{
					return new StandardClientServerFactory();
				}
				return configuredFactory;
			}
			set
			{
				IClientServerFactory factory = value;
				_config.EnvironmentContributions().Add(factory);
			}
		}

		public virtual ISocket4Factory SocketFactory
		{
			get
			{
				ISocket4Factory configuredFactory = ((ISocket4Factory)My(typeof(ISocket4Factory))
					);
				if (null == configuredFactory)
				{
					return new StandardSocket4Factory();
				}
				return configuredFactory;
			}
			set
			{
				ISocket4Factory factory = value;
				_config.EnvironmentContributions().Add(factory);
			}
		}

		private object My(Type type)
		{
			IList environmentContributions = _config.EnvironmentContributions();
			for (int i = environmentContributions.Count - 1; i >= 0; i--)
			{
				object o = environmentContributions[i];
				if (type.IsInstanceOfType(o))
				{
					return (object)o;
				}
			}
			return null;
		}
	}
}
