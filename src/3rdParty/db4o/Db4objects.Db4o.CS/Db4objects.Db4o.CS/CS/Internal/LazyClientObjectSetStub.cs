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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Query.Result;

namespace Db4objects.Db4o.CS.Internal
{
	/// <exclude></exclude>
	public class LazyClientObjectSetStub
	{
		private readonly AbstractQueryResult _queryResult;

		private IIntIterator4 _idIterator;

		public LazyClientObjectSetStub(AbstractQueryResult queryResult, IIntIterator4 idIterator
			)
		{
			_queryResult = queryResult;
			_idIterator = idIterator;
		}

		public virtual IIntIterator4 IdIterator()
		{
			return _idIterator;
		}

		public virtual AbstractQueryResult QueryResult()
		{
			return _queryResult;
		}

		public virtual void Reset()
		{
			_idIterator = _queryResult.IterateIDs();
		}
	}
}
