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

namespace Db4objects.Db4o.Reflect
{
	/// <exclude></exclude>
	public class ArrayInfo
	{
		private int _elementCount;

		private bool _primitive;

		private bool _nullable;

		private IReflectClass _reflectClass;

		public virtual int ElementCount()
		{
			return _elementCount;
		}

		public virtual void ElementCount(int count)
		{
			_elementCount = count;
		}

		public virtual bool Primitive()
		{
			return _primitive;
		}

		public virtual void Primitive(bool flag)
		{
			_primitive = flag;
		}

		public virtual bool Nullable()
		{
			return _nullable;
		}

		public virtual void Nullable(bool flag)
		{
			_nullable = flag;
		}

		public virtual IReflectClass ReflectClass()
		{
			return _reflectClass;
		}

		public virtual void ReflectClass(IReflectClass claxx)
		{
			_reflectClass = claxx;
		}
	}
}
