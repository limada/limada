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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.References;
using Db4objects.Db4o.Monitoring.Internal;

namespace Db4objects.Db4o.Monitoring
{
    public class ReferenceSystemMonitoringSupport : IConfigurationItem
    {
        private class ReferenceSystemListener : IReferenceSystemListener
        {
            private PerformanceCounter _performanceCounter;

            public ReferenceSystemListener(PerformanceCounter counter)
            {
                _performanceCounter = counter;
				_performanceCounter.Disposed += delegate { _performanceCounter = null; };
			}

            public void NotifyReferenceCountChanged(int changedBy)
            {
				if (null != _performanceCounter)
				{
					_performanceCounter.IncrementBy(changedBy);
				}
            }
        }

        private class MonitoringSupportReferenceSystemFactory : IReferenceSystemFactory, IDeepClone
        {
			internal MonitoringSupportReferenceSystemFactory()
			{
			}

			public IReferenceSystem NewReferenceSystem(IInternalObjectContainer container)
            {
            	PerformanceCounter counter = ObjectsInReferenceSystemCounterFor(container);
            	return new MonitoringReferenceSystem(new ReferenceSystemListener(counter));
            }
			
			public object DeepClone(object context)
			{
				return new MonitoringSupportReferenceSystemFactory(this);
			}

        	private PerformanceCounter ObjectsInReferenceSystemCounterFor(IObjectContainer container)
        	{
        		if (_objectsCounter == null)
        		{
        			_objectsCounter = Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.ObjectReferenceCount, container, false);
        			IEventRegistry eventRegistry = EventRegistryFactory.ForObjectContainer(container);
        			eventRegistry.Closing += delegate
					{
						_objectsCounter.RemoveInstance();
        				_objectsCounter.Dispose();
        			};
        		}

        		return _objectsCounter;
        	}

        	private MonitoringSupportReferenceSystemFactory(MonitoringSupportReferenceSystemFactory factory)
        	{
        		_objectsCounter = factory._objectsCounter;
        	}

			private PerformanceCounter _objectsCounter;
		}


        public void Prepare(IConfiguration configuration)
        {
            ((Config4Impl)configuration).ReferenceSystemFactory(new MonitoringSupportReferenceSystemFactory());
        }

        public void Apply(IInternalObjectContainer container)
        {

        }
    }
}

#endif
