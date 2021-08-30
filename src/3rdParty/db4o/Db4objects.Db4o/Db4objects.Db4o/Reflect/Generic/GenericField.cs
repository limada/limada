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
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;

namespace Db4objects.Db4o.Reflect.Generic
{
	/// <exclude></exclude>
	public class GenericField : IReflectField, IDeepClone
	{
		private readonly string _name;

		private readonly GenericClass _type;

		private readonly bool _primitive;

		private int _index = -1;

		public GenericField(string name, IReflectClass clazz, bool primitive)
		{
			_name = name;
			_type = (GenericClass)clazz;
			_primitive = primitive;
		}

		public virtual object DeepClone(object obj)
		{
			IReflector reflector = (IReflector)obj;
			IReflectClass newReflectClass = null;
			if (_type != null)
			{
				newReflectClass = reflector.ForName(_type.GetName());
			}
			return new Db4objects.Db4o.Reflect.Generic.GenericField(_name, newReflectClass, _primitive
				);
		}

		public virtual object Get(object onObject)
		{
			//TODO Consider: Do we need to check that onObject is an instance of the DataClass this field is a member of? 
			return ((GenericObject)onObject).Get(_index);
		}

		public virtual string GetName()
		{
			return _name;
		}

		public virtual IReflectClass GetFieldType()
		{
			return _type;
		}

		public virtual bool IsPublic()
		{
			return true;
		}

		public virtual bool IsPrimitive()
		{
			return _primitive;
		}

		public virtual bool IsStatic()
		{
			//FIXME Consider static fields.
			return false;
		}

		public virtual bool IsTransient()
		{
			return false;
		}

		public virtual void Set(object onObject, object value)
		{
			// FIXME: Consider enabling type checking.
			// The following will fail with arrays.
			// if (!_type.isInstance(value)) throw new RuntimeException(); //TODO Consider: is this checking really necessary?
			((GenericObject)onObject).Set(_index, value);
		}

		internal virtual void SetIndex(int index)
		{
			_index = index;
		}

		public virtual object IndexEntry(object orig)
		{
			return orig;
		}

		public virtual IReflectClass IndexType()
		{
			return GetFieldType();
		}
	}
}
