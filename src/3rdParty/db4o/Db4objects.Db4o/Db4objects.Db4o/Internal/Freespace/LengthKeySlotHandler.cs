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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Freespace
{
	/// <exclude></exclude>
	public class LengthKeySlotHandler : SlotHandler
	{
		public virtual int CompareTo(object obj)
		{
			return _current.CompareByLength((Slot)obj);
		}

		public override IPreparedComparison PrepareComparison(IContext context, object slot
			)
		{
			Slot sourceSlot = (Slot)slot;
			return new _IPreparedComparison_21(sourceSlot);
		}

		private sealed class _IPreparedComparison_21 : IPreparedComparison
		{
			public _IPreparedComparison_21(Slot sourceSlot)
			{
				this.sourceSlot = sourceSlot;
			}

			public int CompareTo(object obj)
			{
				Slot targetSlot = (Slot)obj;
				// FIXME: The comparison method in #compareByLength is the wrong way around.
				// Fix there and here after other references are fixed.
				return -sourceSlot.CompareByLength(targetSlot);
			}

			private readonly Slot sourceSlot;
		}
	}
}
