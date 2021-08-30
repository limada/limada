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
	/// <exclude></exclude>
	public class IntIterator4Impl : IFixedSizeIntIterator4
	{
		private readonly int _count;

		private int[] _content;

		private int _current;

		public IntIterator4Impl(int[] content, int count)
		{
			_content = content;
			_count = count;
			Reset();
		}

		public virtual int CurrentInt()
		{
			if (_content == null || _current == _count)
			{
				throw new InvalidOperationException();
			}
			return _content[_current];
		}

		public virtual object Current
		{
			get
			{
				return CurrentInt();
			}
		}

		public virtual bool MoveNext()
		{
			if (_current < _count - 1)
			{
				_current++;
				return true;
			}
			_content = null;
			return false;
		}

		public virtual void Reset()
		{
			_current = -1;
		}

		public virtual int Size()
		{
			return _count;
		}
	}
}
