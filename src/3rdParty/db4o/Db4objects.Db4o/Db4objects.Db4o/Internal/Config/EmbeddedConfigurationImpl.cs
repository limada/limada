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
using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Config;

namespace Db4objects.Db4o.Internal.Config
{
	public class EmbeddedConfigurationImpl : IEmbeddedConfiguration, ILegacyConfigurationProvider
	{
		private readonly Config4Impl _legacy;

		private IList _configItems;

		public EmbeddedConfigurationImpl(IConfiguration legacy)
		{
			_legacy = (Config4Impl)legacy;
		}

		public virtual ICacheConfiguration Cache
		{
			get
			{
				return new CacheConfigurationImpl(_legacy);
			}
		}

		public virtual IFileConfiguration File
		{
			get
			{
				return new FileConfigurationImpl(_legacy);
			}
		}

		public virtual ICommonConfiguration Common
		{
			get
			{
				return Db4oLegacyConfigurationBridge.AsCommonConfiguration(Legacy());
			}
		}

		public virtual Config4Impl Legacy()
		{
			return _legacy;
		}

		public virtual void AddConfigurationItem(IEmbeddedConfigurationItem configItem)
		{
			if (_configItems != null && _configItems.Contains(configItem))
			{
				return;
			}
			configItem.Prepare(this);
			if (_configItems == null)
			{
				_configItems = new ArrayList();
			}
			_configItems.Add(configItem);
		}

		public virtual void ApplyConfigurationItems(IEmbeddedObjectContainer container)
		{
			if (_configItems == null)
			{
				return;
			}
			for (IEnumerator configItemIter = _configItems.GetEnumerator(); configItemIter.MoveNext
				(); )
			{
				IEmbeddedConfigurationItem configItem = ((IEmbeddedConfigurationItem)configItemIter
					.Current);
				configItem.Apply(container);
			}
		}

		public virtual IIdSystemConfiguration IdSystem
		{
			get
			{
				return new IdSystemConfigurationImpl(_legacy);
			}
		}
	}
}
