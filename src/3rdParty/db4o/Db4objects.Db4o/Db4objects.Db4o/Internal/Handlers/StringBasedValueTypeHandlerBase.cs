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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Handlers
{
	public abstract class StringBasedValueTypeHandlerBase : IValueTypeHandler, IBuiltinTypeHandler
		, IVariableLengthTypeHandler, IQueryableTypeHandler, IComparable4
	{
		public readonly Type _clazz;

		private IReflectClass _classReflector;

		public StringBasedValueTypeHandlerBase(Type clazz)
		{
			_clazz = clazz;
		}

		public virtual void Defragment(IDefragmentContext context)
		{
			StringHandler(context).Defragment(context);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Delete(IDeleteContext context)
		{
			StringHandler(context).Delete(context);
		}

		public virtual object Read(IReadContext context)
		{
			object read = StringHandler(context).Read(context);
			if (null == read)
			{
				return null;
			}
			return ConvertString((string)read);
		}

		public virtual void Write(IWriteContext context, object obj)
		{
			StringHandler(context).Write(context, ConvertObject((object)obj));
		}

		private Db4objects.Db4o.Internal.Handlers.StringHandler StringHandler(IContext context
			)
		{
			return Handlers(context)._stringHandler;
		}

		private HandlerRegistry Handlers(IContext context)
		{
			return ((IInternalObjectContainer)context.ObjectContainer()).Handlers;
		}

		public virtual IPreparedComparison PrepareComparison(IContext context, object obj
			)
		{
			return StringHandler(context).PrepareComparison(context, obj);
		}

		public virtual IReflectClass ClassReflector()
		{
			return _classReflector;
		}

		public virtual void RegisterReflector(IReflector reflector)
		{
			_classReflector = reflector.ForClass(_clazz);
		}

		public virtual bool DescendsIntoMembers()
		{
			return false;
		}

		protected abstract string ConvertObject(object obj);

		protected abstract object ConvertString(string str);
	}
}
