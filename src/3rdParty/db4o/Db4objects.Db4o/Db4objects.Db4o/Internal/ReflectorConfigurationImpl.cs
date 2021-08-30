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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal
{
	public class ReflectorConfigurationImpl : IReflectorConfiguration
	{
		private Config4Impl _config;

		public ReflectorConfigurationImpl(Config4Impl config)
		{
			_config = config;
		}

		public virtual bool TestConstructors()
		{
			return _config.TestConstructors();
		}

		public virtual bool CallConstructor(IReflectClass clazz)
		{
			TernaryBool specialized = CallConstructorSpecialized(clazz);
			if (!specialized.IsUnspecified())
			{
				return specialized.DefiniteYes();
			}
			return _config.CallConstructors().DefiniteYes();
		}

		private TernaryBool CallConstructorSpecialized(IReflectClass clazz)
		{
			Config4Class clazzConfig = _config.ConfigClass(clazz.GetName());
			if (clazzConfig != null)
			{
				TernaryBool res = clazzConfig.CallConstructor();
				if (!res.IsUnspecified())
				{
					return res;
				}
			}
			if (Platform4.IsEnum(_config.Reflector(), clazz))
			{
				return TernaryBool.No;
			}
			IReflectClass ancestor = clazz.GetSuperclass();
			if (ancestor != null)
			{
				return CallConstructorSpecialized(ancestor);
			}
			return TernaryBool.Unspecified;
		}
	}
}
