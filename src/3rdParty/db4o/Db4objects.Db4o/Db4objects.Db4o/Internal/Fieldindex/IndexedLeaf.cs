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
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Fieldindex;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Fieldindex
{
	/// <exclude></exclude>
	public class IndexedLeaf : IndexedNodeBase, IIndexedNodeWithRange
	{
		private readonly IBTreeRange _range;

		public IndexedLeaf(QConObject qcon) : base(qcon)
		{
			_range = Search();
		}

		private IBTreeRange Search()
		{
			IBTreeRange range = Search(Constraint().GetObject());
			QEBitmap bitmap = QEBitmap.ForQE(Constraint().Evaluator());
			if (bitmap.TakeGreater())
			{
				if (bitmap.TakeEqual())
				{
					return range.ExtendToLast();
				}
				IBTreeRange greater = range.Greater();
				if (bitmap.TakeSmaller())
				{
					return greater.Union(range.Smaller());
				}
				return greater;
			}
			if (bitmap.TakeSmaller())
			{
				if (bitmap.TakeEqual())
				{
					return range.ExtendToFirst();
				}
				return range.Smaller();
			}
			return range;
		}

		public override int ResultSize()
		{
			return _range.Size();
		}

		public override IEnumerator GetEnumerator()
		{
			return _range.Keys();
		}

		public virtual IBTreeRange GetRange()
		{
			return _range;
		}

		public override void MarkAsBestIndex()
		{
			_constraint.SetProcessedByIndex();
		}
	}
}
