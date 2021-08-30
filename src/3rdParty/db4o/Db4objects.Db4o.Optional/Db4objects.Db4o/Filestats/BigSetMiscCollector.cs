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
#if !SILVERLIGHT
using Db4objects.Db4o.Filestats;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Filestats
{
	/// <exclude></exclude>
	internal class BigSetMiscCollector : IMiscCollector
	{
		public virtual long CollectFor(LocalObjectContainer db, int id, ISlotMap slotMap)
		{
			object bigSet = db.GetByID(id);
			db.Activate(bigSet, 1);
			BTree btree = (BTree)Reflection4.GetFieldValue(bigSet, "_bTree");
			return FileUsageStatsCollector.BTreeUsage(db, btree, slotMap);
		}
	}
}
#endif // !SILVERLIGHT
