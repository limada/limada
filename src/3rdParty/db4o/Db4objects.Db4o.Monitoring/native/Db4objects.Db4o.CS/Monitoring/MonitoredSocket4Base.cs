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

using Db4objects.Db4o.CS.Foundation;

namespace Db4objects.Db4o.CS.Monitoring
{
	public abstract class MonitoredSocket4Base : Socket4Decorator
	{
		protected MonitoredSocket4Base(ISocket4 socket) : base(socket)
		{
		}

		public override void Write(byte[] bytes, int offset, int count)
		{
			base.Write(bytes, offset, count);
			
			Counters().BytesSent().IncrementBy(count);
			Counters().MessagesSent().Increment();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			int bytesReceived = base.Read(buffer, offset, count);
			Counters().BytesReceived().IncrementBy(bytesReceived);

			return bytesReceived;
		}

		protected abstract NetworkingCounters Counters();
	}
}

#endif