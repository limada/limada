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
using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.Monitoring
{
    /// <summary>
    /// Publishes performance counters for bytes read and written.
    /// </summary>
    public class MonitoredStorage : StorageDecorator
    {
        public MonitoredStorage(IStorage storage) : base(storage)
        {   
        }

        protected override IBin Decorate(BinConfiguration config, IBin bin)
        {
            return new MonitoredBin(bin);
        }

        internal class MonitoredBin : BinDecorator
        {
            private readonly PerformanceCounter _bytesWrittenCounter;
            private readonly PerformanceCounter _bytesReadCounter;

            public MonitoredBin(IBin bin) : base(bin)
            {
                _bytesWrittenCounter = Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.BytesWrittenPerSec, false);
                _bytesReadCounter = Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.BytesReadPerSec, false);
            }

            public override void Write(long position, byte[] bytes, int bytesToWrite)
            {
                base.Write(position, bytes, bytesToWrite);
                _bytesWrittenCounter.IncrementBy(bytesToWrite);
            }

            public override int Read(long position, byte[] buffer, int bytesToRead)
            {   
                int bytesRead = base.Read(position, buffer, bytesToRead);
                _bytesReadCounter.IncrementBy(bytesRead);
                return bytesRead;
            }

            public override int SyncRead(long position, byte[] bytes, int bytesToRead)
            {
                int bytesRead = base.SyncRead(position, bytes, bytesToRead);
                _bytesReadCounter.IncrementBy(bytesRead);
                return bytesRead;
            }

            public override void Close()
            {
                base.Close();

				_bytesReadCounter.RemoveInstance();

            	_bytesReadCounter.Dispose();
            	_bytesWrittenCounter.Dispose();
            }
        }
    }
}
#endif