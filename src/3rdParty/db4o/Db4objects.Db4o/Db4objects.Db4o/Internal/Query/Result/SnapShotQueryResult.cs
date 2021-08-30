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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Internal.Query.Result;

namespace Db4objects.Db4o.Internal.Query.Result
{
	/// <exclude></exclude>
	public class SnapShotQueryResult : AbstractLateQueryResult
	{
		public SnapShotQueryResult(Transaction transaction) : base(transaction)
		{
		}

		public override void LoadFromClassIndex(ClassMetadata clazz)
		{
			CreateSnapshot(ClassIndexIterable(clazz));
		}

		public override void LoadFromClassIndexes(ClassMetadataIterator classCollectionIterator
			)
		{
			CreateSnapshot(ClassIndexesIterable(classCollectionIterator));
		}

		public override void LoadFromQuery(QQuery query)
		{
			IEnumerator _iterator = query.ExecuteSnapshot();
			_iterable = new _IEnumerable_29(_iterator);
		}

		private sealed class _IEnumerable_29 : IEnumerable
		{
			public _IEnumerable_29(IEnumerator _iterator)
			{
				this._iterator = _iterator;
			}

			public IEnumerator GetEnumerator()
			{
				_iterator.Reset();
				return _iterator;
			}

			private readonly IEnumerator _iterator;
		}

		private void CreateSnapshot(IEnumerable iterable)
		{
			Tree ids = TreeInt.AddAll(null, new IntIterator4Adaptor(iterable));
			_iterable = new _IEnumerable_39(ids);
		}

		private sealed class _IEnumerable_39 : IEnumerable
		{
			public _IEnumerable_39(Tree ids)
			{
				this.ids = ids;
			}

			public IEnumerator GetEnumerator()
			{
				return new TreeKeyIterator(ids);
			}

			private readonly Tree ids;
		}
	}
}
