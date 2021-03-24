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
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Monitoring
{
	public class Db4oPerformanceCounters
	{
		public static readonly string CategoryName = "Db4o";

		public static void Install()
		{
			if (CategoryExists())
			{
				return;
			}

			CounterCreationData[] creationData = CreationDataFor(PerformanceCounterSpec.All());
			CounterCreationDataCollection collection = new CounterCreationDataCollection(creationData);

			PerformanceCounterCategory.Create(CategoryName, "Db4o Performance Counters", PerformanceCounterCategoryType.MultiInstance, collection);
		}

		private static CounterCreationData[] CreationDataFor(PerformanceCounterSpec[] performanceCounterSpecs)
		{
			CounterCreationData[] creationData = new CounterCreationData[performanceCounterSpecs.Length];
			for(int i = 0; i < performanceCounterSpecs.Length; i++)
			{
				creationData[i] = performanceCounterSpecs[i].CounterCreationData();
			}
			return creationData;
		}

		public static void ReInstall()
		{
			if (CategoryExists()) DeleteCategory();
			Install();
		}

		public static PerformanceCounter CounterFor(PerformanceCounterSpec spec, bool readOnly)
		{
			return CounterFor(spec, My<IObjectContainer>.Instance, readOnly);
		}

		internal static PerformanceCounter CounterFor(PerformanceCounterSpec spec, IObjectContainer container, bool readOnly)
		{
			return NewDb4oCounter(spec.Id, container.ToString(), readOnly);
		}

		public static PerformanceCounter CounterFor(PerformanceCounterSpec spec, IObjectContainer container)
		{
			return NewDb4oCounter(spec.Id, container.ToString(), true);
		}

		protected static PerformanceCounter NewDb4oCounter(string counterName, string instanceName, bool readOnly)
		{
			Install();

			PerformanceCounter counter = new PerformanceCounter(CategoryName, counterName, instanceName, readOnly);

			if (readOnly) return counter;

			counter.RawValue = 0;
			return counter;
		}

		private static bool CategoryExists()
		{
			return PerformanceCounterCategory.Exists(CategoryName);
		}

		private static void DeleteCategory()
		{
			PerformanceCounterCategory.Delete(CategoryName);
		}
	}
}

#endif