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
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class HandlerVersionRegistry
	{
		private readonly HandlerRegistry _registry;

		private readonly Hashtable4 _versions = new Hashtable4();

		public HandlerVersionRegistry(HandlerRegistry registry)
		{
			_registry = registry;
		}

		public virtual void Put(ITypeHandler4 handler, int version, ITypeHandler4 replacement
			)
		{
			_versions.Put(new HandlerVersionRegistry.HandlerVersionKey(this, handler, version
				), replacement);
		}

		public virtual ITypeHandler4 CorrectHandlerVersion(ITypeHandler4 originalHandler, 
			int version)
		{
			if (version >= HandlerRegistry.HandlerVersion)
			{
				return originalHandler;
			}
			if (originalHandler == null)
			{
				return null;
			}
			// HandlerVersionKey with null key will throw NPE.
			ITypeHandler4 replacement = (ITypeHandler4)_versions.Get(new HandlerVersionRegistry.HandlerVersionKey
				(this, GenericTemplate(originalHandler), version));
			if (replacement == null)
			{
				return CorrectHandlerVersion(originalHandler, version + 1);
			}
			if (replacement is IVersionedTypeHandler)
			{
				return (ITypeHandler4)((IVersionedTypeHandler)replacement).DeepClone(new TypeHandlerCloneContext
					(_registry, originalHandler, version));
			}
			return replacement;
		}

		private ITypeHandler4 GenericTemplate(ITypeHandler4 handler)
		{
			if (handler is IVersionedTypeHandler)
			{
				return ((IVersionedTypeHandler)handler).UnversionedTemplate();
			}
			return handler;
		}

		private class HandlerVersionKey
		{
			private readonly ITypeHandler4 _handler;

			private readonly int _version;

			public HandlerVersionKey(HandlerVersionRegistry _enclosing, ITypeHandler4 handler
				, int version)
			{
				this._enclosing = _enclosing;
				this._handler = handler;
				this._version = version;
			}

			public override int GetHashCode()
			{
				return this._handler.GetHashCode() + this._version * 4271;
			}

			public override bool Equals(object obj)
			{
				HandlerVersionRegistry.HandlerVersionKey other = (HandlerVersionRegistry.HandlerVersionKey
					)obj;
				return this._handler.Equals(other._handler) && this._version == other._version;
			}

			private readonly HandlerVersionRegistry _enclosing;
		}
	}
}
