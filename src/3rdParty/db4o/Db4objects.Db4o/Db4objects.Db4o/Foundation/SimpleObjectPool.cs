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

namespace Db4objects.Db4o.Foundation
{
	public class SimpleObjectPool : IObjectPool
	{
		private readonly object[] _objects;

		private int _available;

		public SimpleObjectPool(object[] objects)
		{
			int length = objects.Length;
			_objects = new object[length];
			for (int i = 0; i < length; ++i)
			{
				_objects[length - i - 1] = objects[i];
			}
			_available = length;
		}

		public virtual object BorrowObject()
		{
			if (_available == 0)
			{
				throw new InvalidOperationException();
			}
			return (object)_objects[--_available];
		}

		public virtual void ReturnObject(object o)
		{
			if (_available == _objects.Length)
			{
				throw new InvalidOperationException();
			}
			_objects[_available++] = o;
		}
	}
}
