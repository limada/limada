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
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Classindex;
using Db4objects.Db4o.Internal.References;

namespace Db4objects.Db4o.Internal.Classindex
{
	/// <exclude></exclude>
	public class BTreeClassIndexStrategy : AbstractClassIndexStrategy
	{
		private BTree _btreeIndex;

		public BTreeClassIndexStrategy(ClassMetadata classMetadata) : base(classMetadata)
		{
		}

		public virtual BTree Btree()
		{
			return _btreeIndex;
		}

		public override int EntryCount(Transaction ta)
		{
			return _btreeIndex != null ? _btreeIndex.Size(ta) : 0;
		}

		public override void Initialize(ObjectContainerBase stream)
		{
			CreateBTreeIndex(stream, 0);
		}

		public override void Purge()
		{
		}

		public override void Read(ObjectContainerBase stream, int indexID)
		{
			ReadBTreeIndex(stream, indexID);
		}

		public override int Write(Transaction trans)
		{
			if (_btreeIndex == null)
			{
				return 0;
			}
			_btreeIndex.Write(trans);
			return _btreeIndex.GetID();
		}

		public override void TraverseAll(Transaction ta, IVisitor4 command)
		{
			if (_btreeIndex != null)
			{
				_btreeIndex.TraverseKeys(ta, command);
			}
		}

		private void CreateBTreeIndex(ObjectContainerBase stream, int btreeID)
		{
			if (stream.IsClient)
			{
				return;
			}
			_btreeIndex = ((LocalObjectContainer)stream).CreateBTreeClassIndex(btreeID);
			_btreeIndex.SetRemoveListener(new _IVisitor4_61(this));
		}

		private sealed class _IVisitor4_61 : IVisitor4
		{
			public _IVisitor4_61(BTreeClassIndexStrategy _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object obj)
			{
				this._enclosing.RemoveId((TransactionContext)obj);
			}

			private readonly BTreeClassIndexStrategy _enclosing;
		}

		private void RemoveId(TransactionContext context)
		{
			IReferenceSystem referenceSystem = context._transaction.ReferenceSystem();
			ObjectReference reference = referenceSystem.ReferenceForId(((int)context._object)
				);
			if (reference != null)
			{
				referenceSystem.RemoveReference(reference);
			}
		}

		private void ReadBTreeIndex(ObjectContainerBase stream, int indexId)
		{
			if (!stream.IsClient && _btreeIndex == null)
			{
				CreateBTreeIndex(stream, indexId);
			}
		}

		protected override void InternalAdd(Transaction trans, int id)
		{
			_btreeIndex.Add(trans, id);
		}

		protected override void InternalRemove(Transaction ta, int id)
		{
			_btreeIndex.Remove(ta, id);
		}

		public override void DontDelete(Transaction transaction, int id)
		{
		}

		public override void DefragReference(ClassMetadata classMetadata, DefragmentContextImpl
			 context, int classIndexID)
		{
			int newID = -classIndexID;
			context.WriteInt(newID);
		}

		public override int Id()
		{
			return _btreeIndex.GetID();
		}

		public override IEnumerator AllSlotIDs(Transaction trans)
		{
			return _btreeIndex.AllNodeIds(trans);
		}

		public override void DefragIndex(DefragmentContextImpl context)
		{
			_btreeIndex.DefragIndex(context);
		}

		public static BTree Btree(ClassMetadata clazz)
		{
			IClassIndexStrategy index = clazz.Index();
			if (!(index is Db4objects.Db4o.Internal.Classindex.BTreeClassIndexStrategy))
			{
				throw new InvalidOperationException();
			}
			return ((Db4objects.Db4o.Internal.Classindex.BTreeClassIndexStrategy)index).Btree
				();
		}

		public static IEnumerator Iterate(ClassMetadata clazz, Transaction trans)
		{
			return Btree(clazz).AsRange(trans).Keys();
		}
	}
}
