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
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Slots
{
	/// <exclude></exclude>
	public class Pointer4
	{
		public readonly int _id;

		public readonly Slot _slot;

		public Pointer4(int id, Slot slot)
		{
			_id = id;
			_slot = slot;
		}

		public virtual int Address()
		{
			return _slot.Address();
		}

		public virtual int Id()
		{
			return _id;
		}

		public virtual int Length()
		{
			return _slot.Length();
		}
	}
}
