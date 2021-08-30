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

namespace Db4objects.Db4o.Internal.Caching
{
	/// <exclude></exclude>
	public interface ICache4 : IEnumerable
	{
		/// <summary>
		/// Retrieves the value associated to the
		/// <see cref="key">key</see>
		/// from the cache. If the value is not yet
		/// cached
		/// <see cref="producer">producer</see>
		/// will be called to produce it. If the cache needs to discard a value
		/// <see cref="finalizer">finalizer</see>
		/// will be given a chance to process it.
		/// </summary>
		/// <param name="key">the key for the value - must never change - cannot be null</param>
		/// <param name="producer">will be called if value not yet in the cache - can only be null when the value is found in the cache
		/// 	</param>
		/// <param name="finalizer">will be called if a page needs to be discarded - can be null
		/// 	</param>
		/// <returns>the cached value</returns>
		object Produce(object key, IFunction4 producer, IProcedure4 finalizer);
	}
}
