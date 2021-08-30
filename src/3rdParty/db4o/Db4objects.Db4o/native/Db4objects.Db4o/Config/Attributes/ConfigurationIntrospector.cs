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
using System.Reflection;

using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Config.Attributes
{
	class ConfigurationIntrospector
	{
		private readonly Type _type;
		private Config4Class _classConfig;
		private readonly IConfiguration _config;

		public ConfigurationIntrospector(Type type, Config4Class classConfig, IConfiguration config)
		{
			if (null == type) throw new ArgumentNullException("type");
			if (null == config) throw new ArgumentNullException("config");
			_type = type;
			_classConfig = classConfig;
			_config = config;
		}

		public Type Type
		{
			get { return _type; }
		}

		public Config4Class ClassConfiguration
		{
			get
			{
				if (null == _classConfig)
				{
					_classConfig = (Config4Class)_config.ObjectClass(_type);
				}
				return _classConfig;
			}
		}

		public IConfiguration IConfiguration
		{
			get { return _config; }
		}		

		public void Apply()
		{
			Apply(_type);
			foreach (FieldInfo field in _type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
				Apply(field);
		}
		
		private void Apply(ICustomAttributeProvider provider)
		{
			foreach (object o in provider.GetCustomAttributes(false))
			{
				IDb4oAttribute attr = o as IDb4oAttribute;
				if (null == attr)
					continue;
				
				attr.Apply(provider, this);
			}
		}
	}
}
