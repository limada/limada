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

namespace Db4objects.Db4o.Internal.Config
{
	/// <exclude></exclude>
	public class IdSystemConfigurationImpl : IIdSystemConfiguration
	{
		private readonly Config4Impl _config;

		public IdSystemConfigurationImpl(Config4Impl config)
		{
			_config = config;
		}

		public virtual void UsePointerBasedSystem()
		{
			_config.UsePointerBasedIdSystem();
		}

		public virtual void UseStackedBTreeSystem()
		{
			_config.UseStackedBTreeIdSystem();
		}

		public virtual void UseInMemorySystem()
		{
			_config.UseInMemoryIdSystem();
		}

		public virtual void UseCustomSystem(IIdSystemFactory factory)
		{
			_config.UseCustomIdSystem(factory);
		}

		public virtual void UseSingleBTreeSystem()
		{
			_config.UseSingleBTreeIdSystem();
		}
	}
}
