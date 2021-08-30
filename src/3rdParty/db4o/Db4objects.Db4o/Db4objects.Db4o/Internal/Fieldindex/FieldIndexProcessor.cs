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
using Db4objects.Db4o.Internal.Fieldindex;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Fieldindex
{
	public class FieldIndexProcessor
	{
		private readonly QCandidates _candidates;

		public FieldIndexProcessor(QCandidates candidates)
		{
			_candidates = candidates;
		}

		public virtual FieldIndexProcessorResult Run()
		{
			IIndexedNode bestIndex = SelectBestIndex();
			if (null == bestIndex)
			{
				return FieldIndexProcessorResult.NoIndexFound;
			}
			if (bestIndex.ResultSize() > 0)
			{
				IIndexedNode resolved = ResolveFully(bestIndex);
				if (null == resolved)
				{
					return FieldIndexProcessorResult.NoIndexFound;
				}
				resolved.MarkAsBestIndex();
				return new FieldIndexProcessorResult(resolved);
			}
			return FieldIndexProcessorResult.FoundIndexButNoMatch;
		}

		private IIndexedNode ResolveFully(IIndexedNode bestIndex)
		{
			if (null == bestIndex)
			{
				return null;
			}
			if (bestIndex.IsResolved())
			{
				return bestIndex;
			}
			return ResolveFully(bestIndex.Resolve());
		}

		public virtual IIndexedNode SelectBestIndex()
		{
			IEnumerator i = CollectIndexedNodes();
			if (!i.MoveNext())
			{
				return null;
			}
			IIndexedNode best = (IIndexedNode)i.Current;
			while (i.MoveNext())
			{
				IIndexedNode leaf = (IIndexedNode)i.Current;
				if (leaf.ResultSize() < best.ResultSize())
				{
					best = leaf;
				}
			}
			return best;
		}

		public virtual IEnumerator CollectIndexedNodes()
		{
			return new IndexedNodeCollector(_candidates).GetNodes();
		}
	}
}
