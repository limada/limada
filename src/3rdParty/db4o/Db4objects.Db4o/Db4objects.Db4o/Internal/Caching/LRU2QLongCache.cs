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
using Db4objects.Db4o.Internal.Caching;

namespace Db4objects.Db4o.Internal.Caching
{
	/// <exclude>
	/// Simplified version of the algorithm taken from here:
	/// http://citeseerx.ist.psu.edu/viewdoc/summary?doi=10.1.1.34.2641
	/// </exclude>
	internal class LRU2QLongCache : ICache4
	{
		private readonly CircularLongBuffer4 _am;

		private readonly CircularLongBuffer4 _a1;

		private readonly IDictionary _slots;

		private readonly int _maxSize;

		private readonly int _a1_threshold;

		internal LRU2QLongCache(int maxSize)
		{
			_maxSize = maxSize;
			_a1_threshold = _maxSize / 4;
			_am = new CircularLongBuffer4(_maxSize);
			_a1 = new CircularLongBuffer4(_maxSize);
			_slots = new Hashtable(maxSize);
		}

		public virtual object Produce(object key, IFunction4 producer, IProcedure4 finalizer
			)
		{
			if (_am.Remove((((long)key))))
			{
				_am.AddFirst((((long)key)));
				return _slots[((long)key)];
			}
			if (_a1.Remove((((long)key))))
			{
				_am.AddFirst((((long)key)));
				return _slots[((long)key)];
			}
			if (_slots.Count >= _maxSize)
			{
				DiscardPage(finalizer);
			}
			object value = producer.Apply(((long)key));
			_slots[((long)key)] = value;
			_a1.AddFirst((((long)key)));
			return value;
		}

		private void DiscardPage(IProcedure4 finalizer)
		{
			if (_a1.Size() >= _a1_threshold)
			{
				DiscardPageFrom(_a1, finalizer);
			}
			else
			{
				DiscardPageFrom(_am, finalizer);
			}
		}

		private void DiscardPageFrom(CircularLongBuffer4 list, IProcedure4 finalizer)
		{
			Discard(list.RemoveLast(), finalizer);
		}

		private void Discard(long key, IProcedure4 finalizer)
		{
			if (null != finalizer)
			{
				finalizer.Apply(_slots[key]);
			}
			Sharpen.Collections.Remove(_slots, key);
		}

		public override string ToString()
		{
			return "LRU2QCache(am=" + ToString(_am) + ", a1=" + ToString(_a1) + ")";
		}

		private string ToString(IEnumerable buffer)
		{
			return Iterators.ToString(buffer);
		}

		public virtual IEnumerator GetEnumerator()
		{
			return _slots.Values.GetEnumerator();
		}
	}
}
