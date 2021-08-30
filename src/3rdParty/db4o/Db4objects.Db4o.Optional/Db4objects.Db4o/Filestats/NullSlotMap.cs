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
using System.Collections;
using Db4objects.Db4o.Filestats;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Filestats
{
	/// <exclude></exclude>
	public class NullSlotMap : ISlotMap
	{
		public virtual void Add(Slot slot)
		{
		}

		public virtual IList Merged()
		{
			return new ArrayList();
		}

		public virtual IList Gaps(long length)
		{
			return new ArrayList();
		}

		public override string ToString()
		{
			return string.Empty;
		}
	}
}
#endif // !SILVERLIGHT
