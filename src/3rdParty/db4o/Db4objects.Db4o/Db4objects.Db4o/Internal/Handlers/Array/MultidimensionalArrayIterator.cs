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
using System.Collections;
using Db4objects.Db4o.Internal.Handlers.Array;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal.Handlers.Array
{
	/// <exclude></exclude>
	public class MultidimensionalArrayIterator : IEnumerator
	{
		private readonly IReflectArray _reflectArray;

		private readonly object[] _array;

		private int _currentElement;

		private IEnumerator _delegate;

		public MultidimensionalArrayIterator(IReflectArray reflectArray, object[] array)
		{
			_reflectArray = reflectArray;
			_array = array;
			Reset();
		}

		public virtual object Current
		{
			get
			{
				if (_delegate == null)
				{
					return _array[_currentElement];
				}
				return _delegate.Current;
			}
		}

		public virtual bool MoveNext()
		{
			if (_delegate != null)
			{
				if (_delegate.MoveNext())
				{
					return true;
				}
				_delegate = null;
			}
			_currentElement++;
			if (_currentElement >= _array.Length)
			{
				return false;
			}
			object obj = _array[_currentElement];
			Type clazz = obj.GetType();
			if (clazz.IsArray)
			{
				if (clazz.GetElementType().IsArray)
				{
					_delegate = new Db4objects.Db4o.Internal.Handlers.Array.MultidimensionalArrayIterator
						(_reflectArray, (object[])obj);
				}
				else
				{
					_delegate = new ReflectArrayIterator(_reflectArray, obj);
				}
				return MoveNext();
			}
			return true;
		}

		public virtual void Reset()
		{
			_currentElement = -1;
			_delegate = null;
		}
	}
}
