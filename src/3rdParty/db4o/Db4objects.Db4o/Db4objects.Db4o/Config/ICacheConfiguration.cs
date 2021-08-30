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
namespace Db4objects.Db4o.Config
{
	/// <summary>Interface to configure the cache configurations.</summary>
	/// <remarks>Interface to configure the cache configurations.</remarks>
	public interface ICacheConfiguration
	{
		/// <summary>
		/// configures the size of the slot cache to hold a number of
		/// slots in the cache.
		/// </summary>
		/// <remarks>
		/// configures the size of the slot cache to hold a number of
		/// slots in the cache.
		/// </remarks>
		/// <value>the number of slots</value>
		[System.ObsoleteAttribute(@"since 7.14 BTrees have their own LRU cache now.")]
		int SlotCacheSize
		{
			set;
		}
	}
}
