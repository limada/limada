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
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal
{
	/// <summary>Prefetchs multiples objects at once (in a single message).</summary>
	/// <remarks>Prefetchs multiples objects at once (in a single message).</remarks>
	/// <exclude></exclude>
	public class SingleMessagePrefetchingStrategy : IPrefetchingStrategy
	{
		public static readonly IPrefetchingStrategy Instance = new Db4objects.Db4o.CS.Internal.SingleMessagePrefetchingStrategy
			();

		private SingleMessagePrefetchingStrategy()
		{
		}

		public virtual int PrefetchObjects(ClientObjectContainer container, Transaction trans
			, IIntIterator4 ids, object[] prefetched, int prefetchCount)
		{
			int count = 0;
			IList idsToGet = new ArrayList();
			while (count < prefetchCount)
			{
				if (!ids.MoveNext())
				{
					break;
				}
				int id = ids.CurrentInt();
				if (id > 0)
				{
					object obj = trans.ObjectForIdFromCache(id);
					if (obj != null)
					{
						prefetched[count] = obj;
					}
					else
					{
						idsToGet.Add(Pair.Of(id, count));
					}
					count++;
				}
			}
			if (idsToGet.Count > 0)
			{
				ByteArrayBuffer[] buffers = container.ReadObjectSlots(trans, IdArrayFor(idsToGet)
					);
				for (int i = 0; i < buffers.Length; i++)
				{
					Pair pair = ((Pair)idsToGet[i]);
					int id = (((int)pair.first));
					int position = (((int)pair.second));
					object obj = trans.ObjectForIdFromCache(id);
					if (obj != null)
					{
						prefetched[position] = obj;
					}
					else
					{
						prefetched[position] = new ObjectReference(id).ReadPrefetch(trans, buffers[i], Const4
							.AddToIdTree);
					}
				}
			}
			return count;
		}

		private int[] IdArrayFor(IList idsToGet)
		{
			int[] idArray = new int[idsToGet.Count];
			for (int i = 0; i < idArray.Length; ++i)
			{
				idArray[i] = (((int)((Pair)idsToGet[i]).first));
			}
			return idArray;
		}
	}
}
