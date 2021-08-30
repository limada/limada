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
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;

namespace Db4objects.Db4o.Reflect.Generic
{
	/// <exclude></exclude>
	public class GenericClassBuilder : IReflectClassBuilder
	{
		private GenericReflector _reflector;

		private IReflector _delegate;

		public GenericClassBuilder(GenericReflector reflector, IReflector delegate_) : base
			()
		{
			_reflector = reflector;
			_delegate = delegate_;
		}

		public virtual IReflectClass CreateClass(string name, IReflectClass superClass, int
			 fieldCount)
		{
			IReflectClass nativeClass = _delegate.ForName(name);
			GenericClass clazz = new GenericClass(_reflector, nativeClass, name, (GenericClass
				)superClass);
			clazz.SetDeclaredFieldCount(fieldCount);
			return clazz;
		}

		public virtual IReflectField CreateField(IReflectClass parentType, string fieldName
			, IReflectClass fieldType, bool isVirtual, bool isPrimitive, bool isArray, bool 
			isNArray)
		{
			if (isVirtual)
			{
				return new GenericVirtualField(fieldName);
			}
			return new GenericField(fieldName, fieldType, isPrimitive);
		}

		public virtual void InitFields(IReflectClass clazz, IReflectField[] fields)
		{
			((GenericClass)clazz).InitFields((GenericField[])fields);
		}

		public virtual IReflectField[] FieldArray(int length)
		{
			return new GenericField[length];
		}
	}
}
