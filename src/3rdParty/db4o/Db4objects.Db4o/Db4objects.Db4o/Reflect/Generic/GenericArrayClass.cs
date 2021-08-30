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
	public class GenericArrayClass : GenericClass
	{
		public GenericArrayClass(GenericReflector reflector, IReflectClass delegateClass, 
			string name, GenericClass superclass) : base(reflector, delegateClass, name, superclass
			)
		{
		}

		public override IReflectClass GetComponentType()
		{
			return GetDelegate();
		}

		public override bool IsArray()
		{
			return true;
		}

		public override bool IsInstance(object candidate)
		{
			if (!(candidate is GenericArray))
			{
				return false;
			}
			return IsAssignableFrom(((GenericArray)candidate)._clazz);
		}

		public override string ToString(object obj)
		{
			if (_converter == null)
			{
				return "(GA) " + GetName();
			}
			return _converter.ToString((GenericArray)obj);
		}
	}
}
