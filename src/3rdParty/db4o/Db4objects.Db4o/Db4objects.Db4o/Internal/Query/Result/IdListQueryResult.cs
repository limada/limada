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
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Classindex;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Internal.Query.Result;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal.Query.Result
{
	/// <exclude></exclude>
	public class IdListQueryResult : AbstractQueryResult, IVisitor4
	{
		private Tree _candidates;

		private bool _checkDuplicates;

		public IntArrayList _ids;

		public IdListQueryResult(Transaction trans, int initialSize) : base(trans)
		{
			_ids = new IntArrayList(initialSize);
		}

		public IdListQueryResult(Transaction trans) : this(trans, 0)
		{
		}

		public override IIntIterator4 IterateIDs()
		{
			return _ids.IntIterator();
		}

		public override object Get(int index)
		{
			lock (Lock())
			{
				return ActivatedObject(GetId(index));
			}
		}

		public override int GetId(int index)
		{
			if (index < 0 || index >= Size())
			{
				throw new Db4oRecoverableException(new IndexOutOfRangeException());
			}
			return _ids.Get(index);
		}

		public void CheckDuplicates()
		{
			_checkDuplicates = true;
		}

		public virtual void Visit(object a_tree)
		{
			QCandidate candidate = (QCandidate)a_tree;
			if (candidate.Include())
			{
				AddKeyCheckDuplicates(candidate._key);
			}
		}

		public virtual void AddKeyCheckDuplicates(int a_key)
		{
			if (_checkDuplicates)
			{
				TreeInt newNode = new TreeInt(a_key);
				_candidates = Tree.Add(_candidates, newNode);
				if (newNode._size == 0)
				{
					return;
				}
			}
			Add(a_key);
		}

		public override void Sort(IQueryComparator cmp)
		{
			Algorithms4.Sort(new _ISortable4_74(this, cmp));
		}

		private sealed class _ISortable4_74 : ISortable4
		{
			public _ISortable4_74(IdListQueryResult _enclosing, IQueryComparator cmp)
			{
				this._enclosing = _enclosing;
				this.cmp = cmp;
			}

			public void Swap(int leftIndex, int rightIndex)
			{
				this._enclosing._ids.Swap(leftIndex, rightIndex);
			}

			public int Size()
			{
				return this._enclosing.Size();
			}

			public int Compare(int leftIndex, int rightIndex)
			{
				return cmp.Compare(this._enclosing.Get(leftIndex), this._enclosing.Get(rightIndex
					));
			}

			private readonly IdListQueryResult _enclosing;

			private readonly IQueryComparator cmp;
		}

		public override void SortIds(IIntComparator cmp)
		{
			Algorithms4.Sort(new _ISortable4_88(this, cmp));
		}

		private sealed class _ISortable4_88 : ISortable4
		{
			public _ISortable4_88(IdListQueryResult _enclosing, IIntComparator cmp)
			{
				this._enclosing = _enclosing;
				this.cmp = cmp;
			}

			public void Swap(int leftIndex, int rightIndex)
			{
				this._enclosing._ids.Swap(leftIndex, rightIndex);
			}

			public int Size()
			{
				return this._enclosing.Size();
			}

			public int Compare(int leftIndex, int rightIndex)
			{
				return cmp.Compare(this._enclosing._ids.Get(leftIndex), this._enclosing._ids.Get(
					rightIndex));
			}

			private readonly IdListQueryResult _enclosing;

			private readonly IIntComparator cmp;
		}

		public override void LoadFromClassIndex(ClassMetadata clazz)
		{
			IClassIndexStrategy index = clazz.Index();
			if (index is BTreeClassIndexStrategy)
			{
				BTree btree = ((BTreeClassIndexStrategy)index).Btree();
				_ids = new IntArrayList(btree.Size(Transaction()));
			}
			index.TraverseAll(_transaction, new _IVisitor4_107(this));
		}

		private sealed class _IVisitor4_107 : IVisitor4
		{
			public _IVisitor4_107(IdListQueryResult _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object a_object)
			{
				this._enclosing.Add(((int)a_object));
			}

			private readonly IdListQueryResult _enclosing;
		}

		public override void LoadFromQuery(QQuery query)
		{
			query.ExecuteLocal(this);
		}

		public override void LoadFromClassIndexes(ClassMetadataIterator iter)
		{
			// duplicates because of inheritance hierarchies
			ByRef duplicates = new ByRef();
			while (iter.MoveNext())
			{
				ClassMetadata classMetadata = iter.CurrentClass();
				if (classMetadata.GetName() != null)
				{
					IReflectClass claxx = classMetadata.ClassReflector();
					if (claxx == null || !(Stream()._handlers.IclassInternal.IsAssignableFrom(claxx)))
					{
						IClassIndexStrategy index = classMetadata.Index();
						index.TraverseAll(_transaction, new _IVisitor4_130(this, duplicates));
					}
				}
			}
		}

		private sealed class _IVisitor4_130 : IVisitor4
		{
			public _IVisitor4_130(IdListQueryResult _enclosing, ByRef duplicates)
			{
				this._enclosing = _enclosing;
				this.duplicates = duplicates;
			}

			public void Visit(object obj)
			{
				int id = ((int)obj);
				TreeInt newNode = new TreeInt(id);
				duplicates.value = Tree.Add(((Tree)duplicates.value), newNode);
				if (newNode.Size() != 0)
				{
					this._enclosing.Add(id);
				}
			}

			private readonly IdListQueryResult _enclosing;

			private readonly ByRef duplicates;
		}

		public override void LoadFromIdReader(IEnumerator ids)
		{
			while (ids.MoveNext())
			{
				Add(((int)ids.Current));
			}
		}

		public virtual void Add(int id)
		{
			_ids.Add(id);
		}

		public override int IndexOf(int id)
		{
			return _ids.IndexOf(id);
		}

		public override int Size()
		{
			return _ids.Size();
		}
	}
}
