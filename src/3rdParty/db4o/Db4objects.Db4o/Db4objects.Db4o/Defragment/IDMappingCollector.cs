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
using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Defragment
{
	public class IDMappingCollector
	{
		private const int IdBatchSize = 4096;

		private TreeInt _ids;

		internal virtual void CreateIDMapping(DefragmentServicesImpl context, int objectID
			, bool isClassID)
		{
			if (BatchFull())
			{
				Flush(context);
			}
			_ids = TreeInt.Add(_ids, (isClassID ? -objectID : objectID));
		}

		private bool BatchFull()
		{
			return _ids != null && _ids.Size() == IdBatchSize;
		}

		public virtual void Flush(DefragmentServicesImpl context)
		{
			if (_ids == null)
			{
				return;
			}
			IEnumerator idIter = new TreeKeyIterator(_ids);
			while (idIter.MoveNext())
			{
				int objectID = ((int)idIter.Current);
				bool isClassID = false;
				if (objectID < 0)
				{
					objectID = -objectID;
					isClassID = true;
				}
				// seen object ids don't come by here anymore - any other candidates?
				context.MapIDs(objectID, context.TargetNewId(), isClassID);
			}
			context.Mapping().Commit();
			_ids = null;
		}
	}
}
