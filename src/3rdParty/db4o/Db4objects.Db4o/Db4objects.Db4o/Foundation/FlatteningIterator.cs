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
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	public class FlatteningIterator : CompositeIterator4
	{
		private class IteratorStack
		{
			public readonly IEnumerator iterator;

			public readonly FlatteningIterator.IteratorStack next;

			public IteratorStack(IEnumerator iterator_, FlatteningIterator.IteratorStack next_
				)
			{
				iterator = iterator_;
				next = next_;
			}
		}

		private FlatteningIterator.IteratorStack _stack;

		public FlatteningIterator(IEnumerator iterators) : base(iterators)
		{
		}

		public override bool MoveNext()
		{
			if (null == _currentIterator)
			{
				if (null == _stack)
				{
					_currentIterator = _iterators;
				}
				else
				{
					_currentIterator = Pop();
				}
			}
			if (!_currentIterator.MoveNext())
			{
				if (_currentIterator == _iterators)
				{
					return false;
				}
				_currentIterator = null;
				return MoveNext();
			}
			object current = _currentIterator.Current;
			if (current is IEnumerator)
			{
				Push(_currentIterator);
				_currentIterator = NextIterator(current);
				return MoveNext();
			}
			return true;
		}

		private void Push(IEnumerator currentIterator)
		{
			_stack = new FlatteningIterator.IteratorStack(currentIterator, _stack);
		}

		private IEnumerator Pop()
		{
			IEnumerator iterator = _stack.iterator;
			_stack = _stack.next;
			return iterator;
		}
	}
}
