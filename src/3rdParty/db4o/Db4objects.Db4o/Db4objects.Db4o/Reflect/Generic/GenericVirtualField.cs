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
	public class GenericVirtualField : GenericField
	{
		public GenericVirtualField(string name) : base(name, null, false)
		{
		}

		public override object DeepClone(object obj)
		{
			return new Db4objects.Db4o.Reflect.Generic.GenericVirtualField(GetName());
		}

		public override object Get(object onObject)
		{
			return null;
		}

		public override IReflectClass GetFieldType()
		{
			return null;
		}

		public override bool IsPublic()
		{
			return false;
		}

		public override bool IsStatic()
		{
			return true;
		}

		public override bool IsTransient()
		{
			return true;
		}

		public override void Set(object onObject, object value)
		{
		}
		// do nothing
	}
}
