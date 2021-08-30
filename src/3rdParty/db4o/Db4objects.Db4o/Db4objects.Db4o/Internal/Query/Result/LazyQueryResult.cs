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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Internal.Query.Result;

namespace Db4objects.Db4o.Internal.Query.Result
{
	/// <exclude></exclude>
	public class LazyQueryResult : AbstractLateQueryResult
	{
		public LazyQueryResult(Transaction trans) : base(trans)
		{
		}

		public override void LoadFromClassIndex(ClassMetadata clazz)
		{
			_iterable = ClassIndexIterable(clazz);
		}

		public override void LoadFromClassIndexes(ClassMetadataIterator classCollectionIterator
			)
		{
			_iterable = ClassIndexesIterable(classCollectionIterator);
		}

		public override void LoadFromQuery(QQuery query)
		{
			_iterable = new _IEnumerable_28(query);
		}

		private sealed class _IEnumerable_28 : IEnumerable
		{
			public _IEnumerable_28(QQuery query)
			{
				this.query = query;
			}

			public IEnumerator GetEnumerator()
			{
				return query.ExecuteLazy();
			}

			private readonly QQuery query;
		}
	}
}
