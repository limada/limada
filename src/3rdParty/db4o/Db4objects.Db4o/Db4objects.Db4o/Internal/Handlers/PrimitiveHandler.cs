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
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Handlers
{
	/// <exclude></exclude>
	public abstract class PrimitiveHandler : IValueTypeHandler, IIndexableTypeHandler
		, IBuiltinTypeHandler, IQueryableTypeHandler
	{
		protected IReflectClass _classReflector;

		private IReflectClass _primitiveClassReflector;

		private object _primitiveNull;

		public virtual object Coerce(IReflectClass claxx, object obj)
		{
			return IsAssignableFrom(claxx) ? obj : No4.Instance;
		}

		private bool IsAssignableFrom(IReflectClass claxx)
		{
			return ClassReflector().IsAssignableFrom(claxx) || PrimitiveClassReflector().IsAssignableFrom
				(claxx);
		}

		public abstract object DefaultValue();

		public virtual void Delete(IDeleteContext context)
		{
			context.Seek(context.Offset() + LinkLength());
		}

		public object IndexEntryToObject(IContext context, object indexEntry)
		{
			return indexEntry;
		}

		public abstract Type PrimitiveJavaClass();

		protected virtual Type JavaClass()
		{
			return Platform4.NullableTypeFor(PrimitiveJavaClass());
		}

		public virtual bool DescendsIntoMembers()
		{
			return false;
		}

		public virtual object PrimitiveNull()
		{
			if (_primitiveNull == null)
			{
				IReflectClass claxx = (_primitiveClassReflector == null ? _classReflector : _primitiveClassReflector
					);
				_primitiveNull = claxx.NullValue();
			}
			return _primitiveNull;
		}

		/// <param name="mf"></param>
		/// <param name="buffer"></param>
		/// <param name="redirect"></param>
		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		public virtual object Read(MarshallerFamily mf, StatefulBuffer buffer, bool redirect
			)
		{
			return Read1(buffer);
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		internal abstract object Read1(ByteArrayBuffer reader);

		public virtual object ReadIndexEntry(IContext context, ByteArrayBuffer buffer)
		{
			try
			{
				return Read1(buffer);
			}
			catch (CorruptionException)
			{
			}
			return null;
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		public object ReadIndexEntryFromObjectSlot(MarshallerFamily mf, StatefulBuffer statefulBuffer
			)
		{
			return Read(mf, statefulBuffer, true);
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual object ReadIndexEntry(IObjectIdContext context)
		{
			return Read(context);
		}

		public virtual IReflectClass ClassReflector()
		{
			return _classReflector;
		}

		public virtual IReflectClass PrimitiveClassReflector()
		{
			return _primitiveClassReflector;
		}

		public virtual void RegisterReflector(IReflector reflector)
		{
			_classReflector = reflector.ForClass(JavaClass());
			Type clazz = PrimitiveJavaClass();
			if (clazz != null)
			{
				_primitiveClassReflector = reflector.ForClass(clazz);
			}
		}

		public abstract void Write(object a_object, ByteArrayBuffer a_bytes);

		public virtual void WriteIndexEntry(IContext context, ByteArrayBuffer a_writer, object
			 a_object)
		{
			if (a_object == null)
			{
				a_object = PrimitiveNull();
			}
			Write(a_object, a_writer);
		}

		// redundant, only added to make Sun JDK 1.2's java happy :(
		public abstract int LinkLength();

		public void Defragment(IDefragmentContext context)
		{
			context.IncrementOffset(LinkLength());
		}

		public virtual void DefragIndexEntry(DefragmentContextImpl context)
		{
			try
			{
				Read1(context.SourceBuffer());
				Read1(context.TargetBuffer());
			}
			catch (CorruptionException)
			{
				Exceptions4.VirtualException();
			}
		}

		protected virtual Db4objects.Db4o.Internal.Marshall.PrimitiveMarshaller PrimitiveMarshaller
			()
		{
			return MarshallerFamily.Current()._primitive;
		}

		public virtual void Write(IWriteContext context, object obj)
		{
			throw new NotImplementedException();
		}

		public virtual object Read(IReadContext context)
		{
			throw new NotImplementedException();
		}

		public virtual object NullRepresentationInUntypedArrays()
		{
			return PrimitiveNull();
		}

		public virtual IPreparedComparison PrepareComparison(IContext context, object obj
			)
		{
			if (obj == null)
			{
				return Null.Instance;
			}
			return InternalPrepareComparison(obj);
		}

		public abstract IPreparedComparison InternalPrepareComparison(object obj);
	}
}
