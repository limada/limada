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
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Internal.Btree
{
	public abstract class AbstractBTreeRangeIterator : IEnumerator
	{
		private readonly BTreeRangeSingle _range;

		private BTreePointer _cursor;

		private BTreePointer _current;

		public AbstractBTreeRangeIterator(BTreeRangeSingle range)
		{
			_range = range;
			_cursor = range.First();
		}

		public virtual bool MoveNext()
		{
			if (ReachedEnd(_cursor))
			{
				_current = null;
				return false;
			}
			_current = _cursor;
			_cursor = _cursor.Next();
			return true;
		}

		public virtual void Reset()
		{
			_cursor = _range.First();
		}

		protected virtual BTreePointer CurrentPointer()
		{
			if (null == _current)
			{
				throw new InvalidOperationException();
			}
			return _current;
		}

		private bool ReachedEnd(BTreePointer cursor)
		{
			if (cursor == null)
			{
				return true;
			}
			if (_range.End() == null)
			{
				return false;
			}
			return _range.End().Equals(cursor);
		}

		public abstract object Current
		{
			get;
		}
	}
}
