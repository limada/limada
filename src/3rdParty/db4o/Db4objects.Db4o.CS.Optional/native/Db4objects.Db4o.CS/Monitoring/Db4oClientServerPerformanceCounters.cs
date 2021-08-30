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
#if !CF && !SILVERLIGHT

using System.Diagnostics;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Monitoring;

namespace Db4objects.Db4o.CS.Monitoring
{
	public class Db4oClientServerPerformanceCounters : Db4oPerformanceCounters
	{
		public static PerformanceCounter CounterForNetworkingClientConnections(IObjectServer server)
		{
			PerformanceCounter clientConnections = NewDb4oCounter(PerformanceCounterSpec.NetClientConnections.Id, false);
			
			IObjectServerEvents serverEvents = (IObjectServerEvents) server;
			serverEvents.ClientConnected += delegate { clientConnections.Increment(); };
			serverEvents.ClientDisconnected += delegate { clientConnections.Decrement(); };

			return clientConnections;
		}

		/*
         * TODO: Remove 
         */

		private static PerformanceCounter NewDb4oCounter(string counterName, bool readOnly)
		{
			string instanceName = My<IObjectContainer>.Instance.ToString();
			return NewDb4oCounter(counterName, instanceName, readOnly);
		}
	}
}

#endif