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
using System.Diagnostics;

#if !SILVERLIGHT

namespace Db4objects.Db4o.Linq.Caching
{
	public class TracingCacheDecorator<TKey, TValue> : ICache4<TKey, TValue>
	{
		private readonly ICache4<TKey, TValue> _delegate;

		public TracingCacheDecorator(ICache4<TKey, TValue> @delegate)
		{
			_delegate = @delegate;
		}

		public TValue Produce(TKey key, Func<TKey, TValue> producer)
		{
			var hit = true;
			var result = _delegate.Produce(key, delegate(TKey newKey)
			                                    {
			                                    	hit = false;
			                                    	return producer(newKey);
			                                    });
			TraceCacheHitMiss(key, hit);
			return result;
		}

		private void TraceCacheHitMiss(TKey key, bool hit)
		{
			if (hit)
				Trace.WriteLine("Cache hit: " + key);
			else
				Trace.WriteLine("Cache miss: " + key);
		}
	}
}

#endif
