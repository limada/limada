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
using Db4objects.Db4o.Internal.Caching;

namespace Db4objects.Db4o.Internal.Caching
{
	/// <exclude></exclude>
	public class CacheFactory
	{
		public static ICache4 New2QCache(int size)
		{
			return new LRU2QCache(size);
		}

		public static ICache4 New2QLongCache(int size)
		{
			return new LRU2QLongCache(size);
		}

		public static ICache4 New2QXCache(int size)
		{
			return new LRU2QXCache(size);
		}

		public static IPurgeableCache4 NewLRUCache(int size)
		{
			return new LRUCache(size);
		}

		public static IPurgeableCache4 NewLRUIntCache(int size)
		{
			return new LRUIntCache(size);
		}

		public static IPurgeableCache4 NewLRULongCache(int size)
		{
			return new LRULongCache(size);
		}
	}
}
