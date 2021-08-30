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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Config;

namespace Db4objects.Db4o.Internal.Config
{
	public class Db4oLegacyConfigurationBridge
	{
		public static IEmbeddedConfiguration AsEmbeddedConfiguration(IConfiguration legacy
			)
		{
			return new EmbeddedConfigurationImpl(legacy);
		}

		public static ICommonConfiguration AsCommonConfiguration(IConfiguration config)
		{
			return new CommonConfigurationImpl((Config4Impl)config);
		}

		public static Config4Impl AsLegacy(object config)
		{
			return ((ILegacyConfigurationProvider)config).Legacy();
		}

		public static IFileConfiguration AsFileConfiguration(IConfiguration config)
		{
			return new FileConfigurationImpl((Config4Impl)config);
		}

		public static IIdSystemConfiguration AsIdSystemConfiguration(IConfiguration config
			)
		{
			return new IdSystemConfigurationImpl((Config4Impl)config);
		}
	}
}
