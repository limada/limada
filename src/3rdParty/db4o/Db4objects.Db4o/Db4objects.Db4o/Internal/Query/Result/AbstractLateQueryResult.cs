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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Classindex;
using Db4objects.Db4o.Internal.Query.Result;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal.Query.Result
{
	/// <exclude></exclude>
	public abstract class AbstractLateQueryResult : AbstractQueryResult
	{
		protected IEnumerable _iterable;

		public AbstractLateQueryResult(Transaction transaction) : base(transaction)
		{
		}

		public override AbstractQueryResult SupportSize()
		{
			return ToIdTree();
		}

		public override AbstractQueryResult SupportSort()
		{
			return ToIdList();
		}

		public override AbstractQueryResult SupportElementAccess()
		{
			return ToIdList();
		}

		protected override int KnownSize()
		{
			return 0;
		}

		public override IIntIterator4 IterateIDs()
		{
			if (_iterable == null)
			{
				throw new InvalidOperationException();
			}
			return new IntIterator4Adaptor(_iterable);
		}

		public override AbstractQueryResult ToIdList()
		{
			return ToIdTree().ToIdList();
		}

		public virtual bool SkipClass(ClassMetadata classMetadata)
		{
			if (classMetadata.GetName() == null)
			{
				return true;
			}
			IReflectClass claxx = classMetadata.ClassReflector();
			if (Stream()._handlers.IclassInternal.IsAssignableFrom(claxx))
			{
				return true;
			}
			return false;
		}

		protected virtual IEnumerable ClassIndexesIterable(ClassMetadataIterator classCollectionIterator
			)
		{
			return Iterators.ConcatMap(Iterators.Iterable(classCollectionIterator), new _IFunction4_61
				(this));
		}

		private sealed class _IFunction4_61 : IFunction4
		{
			public _IFunction4_61(AbstractLateQueryResult _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Apply(object current)
			{
				ClassMetadata classMetadata = (ClassMetadata)current;
				if (this._enclosing.SkipClass(classMetadata))
				{
					return Iterators.Skip;
				}
				return this._enclosing.ClassIndexIterable(classMetadata);
			}

			private readonly AbstractLateQueryResult _enclosing;
		}

		protected virtual IEnumerable ClassIndexIterable(ClassMetadata clazz)
		{
			return new _IEnumerable_73(this, clazz);
		}

		private sealed class _IEnumerable_73 : IEnumerable
		{
			public _IEnumerable_73(AbstractLateQueryResult _enclosing, ClassMetadata clazz)
			{
				this._enclosing = _enclosing;
				this.clazz = clazz;
			}

			public IEnumerator GetEnumerator()
			{
				return this._enclosing.ClassIndexIterator(clazz);
			}

			private readonly AbstractLateQueryResult _enclosing;

			private readonly ClassMetadata clazz;
		}

		public virtual IEnumerator ClassIndexIterator(ClassMetadata clazz)
		{
			return BTreeClassIndexStrategy.Iterate(clazz, Transaction());
		}
	}
}
