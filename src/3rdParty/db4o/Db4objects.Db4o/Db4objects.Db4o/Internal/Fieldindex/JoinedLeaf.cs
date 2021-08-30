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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Fieldindex;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Fieldindex
{
	public abstract class JoinedLeaf : IIndexedNodeWithRange
	{
		private readonly QCon _constraint;

		private readonly IIndexedNodeWithRange _leaf1;

		private readonly IBTreeRange _range;

		public JoinedLeaf(QCon constraint, IIndexedNodeWithRange leaf1, IBTreeRange range
			)
		{
			if (null == constraint || null == leaf1 || null == range)
			{
				throw new ArgumentNullException();
			}
			_constraint = constraint;
			_leaf1 = leaf1;
			_range = range;
		}

		public virtual QCon GetConstraint()
		{
			return _constraint;
		}

		public virtual IBTreeRange GetRange()
		{
			return _range;
		}

		public virtual IEnumerator GetEnumerator()
		{
			return _range.Keys();
		}

		public virtual TreeInt ToTreeInt()
		{
			return IndexedNodeBase.AddToTree(null, this);
		}

		public virtual BTree GetIndex()
		{
			return _leaf1.GetIndex();
		}

		public virtual bool IsResolved()
		{
			return _leaf1.IsResolved();
		}

		public virtual IIndexedNode Resolve()
		{
			return IndexedPath.NewParentPath(this, _constraint);
		}

		public virtual int ResultSize()
		{
			return _range.Size();
		}

		public virtual void MarkAsBestIndex()
		{
			_leaf1.MarkAsBestIndex();
			_constraint.SetProcessedByIndex();
		}
	}
}
