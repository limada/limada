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
	public class HashSet4 : ISet4
	{
		private Hashtable4 _map;

		public HashSet4() : this(1)
		{
		}

		public HashSet4(int count)
		{
			_map = new Hashtable4(count);
		}

		public virtual bool Add(object obj)
		{
			if (_map.ContainsKey(obj))
			{
				return false;
			}
			_map.Put(obj, obj);
			return true;
		}

		public virtual void Clear()
		{
			_map.Clear();
		}

		public virtual bool Contains(object obj)
		{
			return _map.ContainsKey(obj);
		}

		public virtual bool IsEmpty()
		{
			return _map.Size() == 0;
		}

		public virtual IEnumerator GetEnumerator()
		{
			return _map.Values().GetEnumerator();
		}

		public virtual bool Remove(object obj)
		{
			return _map.Remove(obj) != null;
		}

		public virtual int Size()
		{
			return _map.Size();
		}

		public override string ToString()
		{
			return Iterators.Join(_map.Keys(), "{", "}", ", ");
		}
	}
}
