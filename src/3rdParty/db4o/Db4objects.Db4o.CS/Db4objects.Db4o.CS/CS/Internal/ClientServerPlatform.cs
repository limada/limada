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
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query.Result;

namespace Db4objects.Db4o.CS.Internal
{
	/// <summary>Platform specific defaults.</summary>
	/// <remarks>Platform specific defaults.</remarks>
	public class ClientServerPlatform
	{
		/// <summary>
		/// The default
		/// <see cref="ClientQueryResultIterator">ClientQueryResultIterator</see>
		/// for this platform.
		/// </summary>
		/// <returns></returns>
		public static IEnumerator CreateClientQueryResultIterator(AbstractQueryResult result
			)
		{
			IQueryResultIteratorFactory factory = result.Config().QueryResultIteratorFactory(
				);
			if (null != factory)
			{
				return factory.NewInstance(result);
			}
			return new ClientQueryResultIterator(result);
		}
	}
}
