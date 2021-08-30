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
	public class CompositeIterator4 : IEnumerator
	{
		protected readonly IEnumerator _iterators;

		protected IEnumerator _currentIterator;

		public CompositeIterator4(IEnumerator[] iterators) : this(new ArrayIterator4(iterators
			))
		{
		}

		public CompositeIterator4(IEnumerator iterators)
		{
			if (null == iterators)
			{
				throw new ArgumentNullException();
			}
			_iterators = iterators;
		}

		public virtual bool MoveNext()
		{
			if (null == _currentIterator)
			{
				if (!_iterators.MoveNext())
				{
					return false;
				}
				_currentIterator = NextIterator(_iterators.Current);
			}
			if (!_currentIterator.MoveNext())
			{
				_currentIterator = null;
				return MoveNext();
			}
			return true;
		}

		public virtual void Reset()
		{
			ResetIterators();
			_currentIterator = null;
			_iterators.Reset();
		}

		private void ResetIterators()
		{
			_iterators.Reset();
			while (_iterators.MoveNext())
			{
				NextIterator(_iterators.Current).Reset();
			}
		}

		public virtual IEnumerator CurrentIterator()
		{
			return _currentIterator;
		}

		public virtual object Current
		{
			get
			{
				return _currentIterator.Current;
			}
		}

		protected virtual IEnumerator NextIterator(object current)
		{
			return (IEnumerator)current;
		}
	}
}
