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
#if !CF_3_5

using System.Linq;

namespace Db4objects.Db4o.Linq
{
	/// <summary>
	/// IDb4oLinqQueryable is the query type of Linq to db4o when working with an API requiring
	/// a LINQ provider implementing <see cref="System.Linq.IQueryable">IQueryable</see>.
	/// <typeparam name="T">The type of the objects that are queried from the database.</typeparam>
	/// </summary>
	public interface IDb4oLinqQueryable<T> : IDb4oLinqQueryable, IOrderedQueryable<T>
	{
	}

	public interface IDb4oLinqQueryable : IOrderedQueryable
	{
		IDb4oLinqQuery GetQuery();
	}
}

#endif