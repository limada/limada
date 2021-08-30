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

using System;
using System.Diagnostics;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Monitoring
{
    public class ObjectLifecycleMonitoringSupport : IConfigurationItem
    {
        public void Prepare(IConfiguration configuration)
        {
        }

        public void Apply(IInternalObjectContainer container)
        {
            PerformanceCounter storedObjectsPerSec =
                Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.ObjectsStoredPerSec, false);
            PerformanceCounter activatedObjectsPerSec =
                Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.ObjectsActivatedPerSec, false);
            PerformanceCounter deactivatedObjectsPerSec =
                Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.ObjectsDeactivatedPerSec, false);
            IEventRegistry eventRegistry = EventRegistryFactory.ForObjectContainer(container);

            EventHandler<ObjectInfoEventArgs> eventHandler = delegate
                                       {
                                           storedObjectsPerSec.Increment();
                                       };
            eventRegistry.Created += eventHandler;
            eventRegistry.Updated += eventHandler;

            eventRegistry.Activated += delegate
                                           {
                                               activatedObjectsPerSec.Increment();
                                           };
            eventRegistry.Deactivated += delegate
                                             {
                                                 deactivatedObjectsPerSec.Increment();
                                             };

            eventRegistry.Closing += delegate
                                        {
                                            storedObjectsPerSec.Dispose();
                                            activatedObjectsPerSec.Dispose();
                                            deactivatedObjectsPerSec.Dispose();

                                            storedObjectsPerSec.RemoveInstance();
                                        };
            if (container.IsClient)
            {
                return;
            }

            PerformanceCounter deletedObjectsPerSec =
                Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.ObjectsDeletedPerSec, false);
            eventRegistry.Deleted += delegate
                                         {
                                             deletedObjectsPerSec.Increment();
                                         };

            eventRegistry.Closing += delegate
            {
                deletedObjectsPerSec.Dispose();
            };



        }
    }
}

#endif