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
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.CS.Internal
{
	/// <exclude></exclude>
	public class LazyClientIdIterator : IIntIterator4
	{
		private readonly LazyClientQueryResult _queryResult;

		private int _current;

		private int[] _ids;

		private readonly int _batchSize;

		private int _available;

		public LazyClientIdIterator(LazyClientQueryResult queryResult)
		{
			_queryResult = queryResult;
			_batchSize = queryResult.Config().PrefetchObjectCount();
			_ids = new int[_batchSize];
			_current = -1;
		}

		public virtual int CurrentInt()
		{
			if (_current < 0)
			{
				throw new InvalidOperationException();
			}
			return _ids[_current];
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
			if (_available < 0)
			{
				return false;
			}
			if (_available == 0)
			{
				_queryResult.FetchIDs(_batchSize);
				_available--;
				_current = 0;
				return (_available > 0);
			}
			_current++;
			_available--;
			return true;
		}

		public virtual void Reset()
		{
			_queryResult.Reset();
			_available = 0;
			_current = -1;
		}

		public virtual void LoadFromIdReader(IEnumerator ids)
		{
			int count = 0;
			while (ids.MoveNext())
			{
				_ids[count++] = ((int)ids.Current);
			}
			if (count > 0)
			{
				_available = count;
				return;
			}
			_available = -1;
		}
	}
}
