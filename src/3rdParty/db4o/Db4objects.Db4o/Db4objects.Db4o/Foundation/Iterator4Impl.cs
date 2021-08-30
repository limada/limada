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
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class Iterator4Impl : IEnumerator
	{
		private readonly List4 _first;

		private List4 _next;

		private object _current;

		public Iterator4Impl(List4 first)
		{
			_first = first;
			_next = first;
			_current = Iterators.NoElement;
		}

		public virtual bool MoveNext()
		{
			if (_next == null)
			{
				_current = Iterators.NoElement;
				return false;
			}
			_current = ((object)_next._element);
			_next = ((List4)_next._next);
			return true;
		}

		public virtual object Current
		{
			get
			{
				if (Iterators.NoElement == _current)
				{
					throw new InvalidOperationException();
				}
				return (object)_current;
			}
		}

		public virtual void Reset()
		{
			_next = _first;
			_current = Iterators.NoElement;
		}
	}
}
