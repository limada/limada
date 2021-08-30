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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public abstract class TypeHandlerConfiguration
	{
		protected readonly Config4Impl _config;

		private ITypeHandler4 _listTypeHandler;

		private ITypeHandler4 _mapTypeHandler;

		public abstract void Apply();

		public TypeHandlerConfiguration(Config4Impl config)
		{
			_config = config;
		}

		protected virtual void ListTypeHandler(ITypeHandler4 listTypeHandler)
		{
			_listTypeHandler = listTypeHandler;
		}

		protected virtual void MapTypeHandler(ITypeHandler4 mapTypehandler)
		{
			_mapTypeHandler = mapTypehandler;
		}

		protected virtual void RegisterCollection(Type clazz)
		{
			RegisterListTypeHandlerFor(clazz);
		}

		protected virtual void RegisterMap(Type clazz)
		{
			RegisterMapTypeHandlerFor(clazz);
		}

		protected virtual void IgnoreFieldsOn(Type clazz)
		{
			_config.RegisterTypeHandler(new SingleClassTypeHandlerPredicate(clazz), IgnoreFieldsTypeHandler
				.Instance);
		}

		private void RegisterListTypeHandlerFor(Type clazz)
		{
			RegisterTypeHandlerFor(clazz, _listTypeHandler);
		}

		private void RegisterMapTypeHandlerFor(Type clazz)
		{
			RegisterTypeHandlerFor(clazz, _mapTypeHandler);
		}

		protected virtual void RegisterTypeHandlerFor(Type clazz, ITypeHandler4 typeHandler
			)
		{
			_config.RegisterTypeHandler(new SingleClassTypeHandlerPredicate(clazz), typeHandler
				);
		}
	}
}
