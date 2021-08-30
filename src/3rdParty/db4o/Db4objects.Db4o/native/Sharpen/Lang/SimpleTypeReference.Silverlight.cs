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
#if SILVERLIGHT

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Resources;

#endif

namespace Sharpen.Lang
{
	public partial class SimpleTypeReference
	{
#if SILVERLIGHT
		private Assembly ResolveAssemblySilverlight()
		{
			if (!_assemblyCache.Contains(_assemblyName.Name))
			{
				Assembly assembly = IsInUIThread() 
				                    	? LoadAssembly(_assemblyName.Name) 
				                    	: LoadAssemblyInUIThread(_assemblyName.Name);
				
				_assemblyCache[_assemblyName.Name] = assembly;
				return assembly;
			}

			return _assemblyCache[_assemblyName.Name];
		}

		private static bool IsInUIThread()
		{
			return Deployment.Current.CheckAccess();
		}

		private static Assembly LoadAssemblyInUIThread(string assemblyName)
		{
			Assembly assembly = null;
			using (EventWaitHandle wait = new ManualResetEvent(false))
			{
				Deployment.Current.Dispatcher.BeginInvoke(delegate
				                                          	{
				                                          		assembly = LoadAssembly(assemblyName);
				                                          		wait.Set();
				                                          	});

				wait.WaitOne();
			}
			return assembly;
		}

		private static Assembly LoadAssembly(string assemblyName)
		{
			StreamResourceInfo resourceInfo = Application.GetResourceStream(AssemblyUriFor(assemblyName));
			return new AssemblyPart().Load(resourceInfo.Stream);
		}

		private static Uri AssemblyUriFor(string assemblyName)
		{
			return new Uri(assemblyName + ".dll", UriKind.Relative);
		}

		private static readonly AssemblyCache _assemblyCache = new AssemblyCache(typeof(Type).Assembly);

	}

	sealed class AssemblyCache
	{
		public AssemblyCache(params Assembly[] preCachedAssemblies)
		{
			foreach (var assembly in preCachedAssemblies)
			{
				_cache[new AssemblyName(assembly.FullName).Name] = assembly;
			}
		}

		public Assembly this[string assemblyName]
		{
			get { return _cache[assemblyName]; }
			set { _cache[assemblyName] = value; }
		}

		public bool Contains(string assemblyName)
		{
			return _cache.ContainsKey(assemblyName);
		}
		
		private IDictionary<string, Assembly> _cache = new Dictionary<string, Assembly>();
#endif
	}
}
