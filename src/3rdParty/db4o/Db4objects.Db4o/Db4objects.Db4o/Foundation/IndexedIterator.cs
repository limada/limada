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
using System.Collections;

namespace Db4objects.Db4o.Foundation
{
	/// <summary>
	/// Basic functionality for implementing iterators for
	/// fixed length structures whose elements can be efficiently
	/// accessed by a numeric index.
	/// </summary>
	/// <remarks>
	/// Basic functionality for implementing iterators for
	/// fixed length structures whose elements can be efficiently
	/// accessed by a numeric index.
	/// </remarks>
	public abstract class IndexedIterator : IEnumerator
	{
		private readonly int _length;

		private int _next;

		public IndexedIterator(int length)
		{
			_length = length;
			_next = -1;
		}

		public virtual bool MoveNext()
		{
			if (_next < LastIndex())
			{
				++_next;
				return true;
			}
			// force exception on unexpected call to current
			_next = _length;
			return false;
		}

		public virtual object Current
		{
			get
			{
				return Get(_next);
			}
		}

		public virtual void Reset()
		{
			_next = -1;
		}

		protected abstract object Get(int index);

		private int LastIndex()
		{
			return _length - 1;
		}
	}
}
