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
using Db4objects.Db4o.Internal;
using Sharpen.Lang;

namespace Db4objects.Db4o.CS.Internal
{
	/// <exclude></exclude>
	public class ClientHeartbeat : IRunnable
	{
		private SimpleTimer _timer;

		private readonly ClientObjectContainer _container;

		public ClientHeartbeat(ClientObjectContainer container)
		{
			_container = container;
			_timer = new SimpleTimer(this, Frequency(container.ConfigImpl));
		}

		private int Frequency(Config4Impl config)
		{
			return Math.Min(config.TimeoutClientSocket(), config.TimeoutServerSocket()) / 4;
		}

		public virtual void Run()
		{
			_container.WriteMessageToSocket(Msg.Ping);
		}

		public virtual void Start()
		{
			_container.ThreadPool().Start("db4o client heartbeat", _timer);
		}

		public virtual void Stop()
		{
			if (_timer == null)
			{
				return;
			}
			_timer.Stop();
			_timer = null;
		}
	}
}
