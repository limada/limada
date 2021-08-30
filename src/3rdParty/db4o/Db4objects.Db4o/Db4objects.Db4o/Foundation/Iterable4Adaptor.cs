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

namespace Db4objects.Db4o.Foundation
{
	/// <summary>
	/// Adapts Iterable4/Iterator4 iteration model (moveNext, current) to the old db4o
	/// and jdk model (hasNext, next).
	/// </summary>
	/// <remarks>
	/// Adapts Iterable4/Iterator4 iteration model (moveNext, current) to the old db4o
	/// and jdk model (hasNext, next).
	/// </remarks>
	/// <exclude></exclude>
	public class Iterable4Adaptor
	{
		private static readonly object EofMarker = new object();

		private static readonly object MoveNextMarker = new object();

		private readonly IEnumerable _delegate;

		private IEnumerator _iterator;

		private object _current = MoveNextMarker;

		public Iterable4Adaptor(IEnumerable delegate_)
		{
			_delegate = delegate_;
		}

		public virtual bool HasNext()
		{
			if (_current == MoveNextMarker)
			{
				return MoveNext();
			}
			return _current != EofMarker;
		}

		public virtual object Next()
		{
			if (!HasNext())
			{
				throw new InvalidOperationException();
			}
			object returnValue = _current;
			_current = MoveNextMarker;
			return returnValue;
		}

		protected virtual bool MoveNext()
		{
			if (null == _iterator)
			{
				_iterator = _delegate.GetEnumerator();
			}
			if (_iterator.MoveNext())
			{
				_current = _iterator.Current;
				return true;
			}
			_current = EofMarker;
			return false;
		}

		public virtual void Reset()
		{
			_iterator = null;
			_current = MoveNextMarker;
		}
	}
}
