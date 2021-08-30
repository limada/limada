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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Sharpen.Lang;

namespace Db4objects.Db4o.Foundation
{
	public partial class Environments
	{
		private static readonly DynamicVariable _current = DynamicVariable.NewInstance();

		public static object My(Type service)
		{
			IEnvironment environment = Current();
			if (null == environment)
			{
				throw new InvalidOperationException();
			}
			return environment.Provide(service);
		}

		private static IEnvironment Current()
		{
			return ((IEnvironment)_current.Value);
		}

		public static void RunWith(IEnvironment environment, IRunnable runnable)
		{
			_current.With(environment, runnable);
		}

		public static IEnvironment NewClosedEnvironment(object[] bindings)
		{
			return new _IEnvironment_32(bindings);
		}

		private sealed class _IEnvironment_32 : IEnvironment
		{
			public _IEnvironment_32(object[] bindings)
			{
				this.bindings = bindings;
			}

			public object Provide(Type service)
			{
				for (int bindingIndex = 0; bindingIndex < bindings.Length; ++bindingIndex)
				{
					object binding = bindings[bindingIndex];
					if (service.IsInstanceOfType(binding))
					{
						return (object)binding;
					}
				}
				return null;
			}

			private readonly object[] bindings;
		}

		public static IEnvironment NewCachingEnvironmentFor(IEnvironment environment)
		{
			return new _IEnvironment_48(environment);
		}

		private sealed class _IEnvironment_48 : IEnvironment
		{
			public _IEnvironment_48(IEnvironment environment)
			{
				this.environment = environment;
				this._bindings = new Hashtable();
			}

			private readonly IDictionary _bindings;

			public object Provide(Type service)
			{
				object existing = this._bindings[service];
				if (null != existing)
				{
					return (object)existing;
				}
				object binding = environment.Provide(service);
				if (null == binding)
				{
					return null;
				}
				this._bindings[service] = binding;
				return binding;
			}

			private readonly IEnvironment environment;
		}

		public static IEnvironment NewConventionBasedEnvironment(object[] bindings)
		{
			return NewCachingEnvironmentFor(Compose(new IEnvironment[] { NewClosedEnvironment
				(bindings), new Environments.ConventionBasedEnvironment() }));
		}

		public static IEnvironment NewConventionBasedEnvironment()
		{
			return NewCachingEnvironmentFor(new Environments.ConventionBasedEnvironment());
		}

		public static IEnvironment Compose(IEnvironment[] environments)
		{
			return new _IEnvironment_75(environments);
		}

		private sealed class _IEnvironment_75 : IEnvironment
		{
			public _IEnvironment_75(IEnvironment[] environments)
			{
				this.environments = environments;
			}

			public object Provide(Type service)
			{
				for (int eIndex = 0; eIndex < environments.Length; ++eIndex)
				{
					IEnvironment e = environments[eIndex];
					object binding = e.Provide(service);
					if (null != binding)
					{
						return binding;
					}
				}
				return null;
			}

			private readonly IEnvironment[] environments;
		}

		private sealed class ConventionBasedEnvironment : IEnvironment
		{
			public object Provide(Type service)
			{
				return Resolve(service);
			}

			/// <summary>
			/// Resolves a service interface to its default implementation using the
			/// db4o namespace convention:
			/// interface foo.bar.Baz
			/// default implementation foo.internal.bar.BazImpl
			/// </summary>
			/// <returns>the convention based type name for the requested service</returns>
			private object Resolve(Type service)
			{
				string className = DefaultImplementationFor(service);
				object binding = ReflectPlatform.CreateInstance(className);
				if (null == binding)
				{
					throw new ArgumentException("Cant find default implementation for " + service.ToString
						() + ": " + className);
				}
				return (object)binding;
			}
		}
		// ignore convention for internal types
	}
}
