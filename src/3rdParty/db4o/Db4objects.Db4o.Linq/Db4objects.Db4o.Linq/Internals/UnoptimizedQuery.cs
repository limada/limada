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
using System.Collections.Generic;
using System.Linq;

namespace Db4objects.Db4o.Linq.Internals
{
	internal class UnoptimizedQuery<T> : IDb4oLinqQueryInternal<T>
	{
		private IEnumerable<T> _result;

		public UnoptimizedQuery(IEnumerable<T> result)
		{
			if (result == null)
				throw new ArgumentNullException("result");

			_result = result;
		}

		public IEnumerable<T> Result
		{
			get { return _result; }
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _result.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#region IDb4oLinqQueryInternal<T> Members

		public IEnumerable<T> UnoptimizedThenBy<TKey>(Func<T, TKey> function)
		{
			return ((IOrderedEnumerable<T>)_result).ThenBy(function);
		}

		public IEnumerable<T> UnoptimizedThenByDescending<TKey>(Func<T, TKey> function)
		{
			return ((IOrderedEnumerable<T>)_result).ThenByDescending(function);
		}

		public IEnumerable<T> UnoptimizedWhere(Func<T, bool> func)
		{
			return _result.Where(func);
		}

		#endregion
	}
}
