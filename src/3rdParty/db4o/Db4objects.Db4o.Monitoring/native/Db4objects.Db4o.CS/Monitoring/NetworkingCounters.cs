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
using Db4objects.Db4o.Monitoring;

namespace Db4objects.Db4o.CS.Monitoring
{
	public class NetworkingCounters
	{
		internal PerformanceCounter BytesSent()
		{
			if (null == _bytesSent)
			{
                _bytesSent = Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.NetBytesSentPerSec, false);
			}

			return _bytesSent;
		}

		internal PerformanceCounter BytesReceived()
		{
			if (null == _bytesReceived)
			{
                _bytesReceived = Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.NetBytesReceivedPerSec, false);
			}

			return _bytesReceived;
		}

		internal PerformanceCounter MessagesSent()
		{
			if (null == _messagesSent)
			{
                _messagesSent = Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.NetMessagesSentPerSec, false);
			}

			return _messagesSent;
		}
		
		public void Close()
		{
			Dispose(_bytesSent);
			Dispose(_bytesReceived);
			Dispose(_messagesSent);
		}

		private static void Dispose(PerformanceCounter counter)
		{
			if (null != counter)
			{
				counter.RemoveInstance();
				counter.Dispose();
			}
		}

		private PerformanceCounter _bytesSent;
		private PerformanceCounter _bytesReceived;
		private PerformanceCounter _messagesSent;

	}
}

#endif