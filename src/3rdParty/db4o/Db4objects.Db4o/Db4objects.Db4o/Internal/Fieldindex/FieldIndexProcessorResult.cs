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
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Fieldindex;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Fieldindex
{
	public class FieldIndexProcessorResult
	{
		public static readonly Db4objects.Db4o.Internal.Fieldindex.FieldIndexProcessorResult
			 NoIndexFound = new Db4objects.Db4o.Internal.Fieldindex.FieldIndexProcessorResult
			(null);

		public static readonly Db4objects.Db4o.Internal.Fieldindex.FieldIndexProcessorResult
			 FoundIndexButNoMatch = new Db4objects.Db4o.Internal.Fieldindex.FieldIndexProcessorResult
			(null);

		private readonly IIndexedNode _indexedNode;

		public FieldIndexProcessorResult(IIndexedNode indexedNode)
		{
			_indexedNode = indexedNode;
		}

		public virtual Tree ToQCandidate(QCandidates candidates)
		{
			return TreeInt.ToQCandidate(ToTreeInt(), candidates);
		}

		public virtual TreeInt ToTreeInt()
		{
			if (FoundMatch())
			{
				return _indexedNode.ToTreeInt();
			}
			return null;
		}

		public virtual bool FoundMatch()
		{
			return FoundIndex() && !NoMatch();
		}

		public virtual bool FoundIndex()
		{
			return this != NoIndexFound;
		}

		public virtual bool NoMatch()
		{
			return this == FoundIndexButNoMatch;
		}

		public virtual IEnumerator IterateIDs()
		{
			return new _MappingIterator_46(_indexedNode.GetEnumerator());
		}

		private sealed class _MappingIterator_46 : MappingIterator
		{
			public _MappingIterator_46(IEnumerator baseArg1) : base(baseArg1)
			{
			}

			protected override object Map(object current)
			{
				IFieldIndexKey composite = (IFieldIndexKey)current;
				return composite.ParentID();
			}
		}
	}
}
