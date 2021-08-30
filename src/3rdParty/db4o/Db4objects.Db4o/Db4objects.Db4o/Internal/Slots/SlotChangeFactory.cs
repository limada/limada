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
	public class SlotChangeFactory
	{
		private SlotChangeFactory()
		{
		}

		public virtual SlotChange NewInstance(int id)
		{
			return new SlotChange(id);
		}

		public static readonly Db4objects.Db4o.Internal.Slots.SlotChangeFactory UserObjects
			 = new Db4objects.Db4o.Internal.Slots.SlotChangeFactory();

		private sealed class _SlotChangeFactory_20 : Db4objects.Db4o.Internal.Slots.SlotChangeFactory
		{
			public _SlotChangeFactory_20()
			{
			}

			public override SlotChange NewInstance(int id)
			{
				return new SystemSlotChange(id);
			}
		}

		public static readonly Db4objects.Db4o.Internal.Slots.SlotChangeFactory SystemObjects
			 = new _SlotChangeFactory_20();

		private sealed class _SlotChangeFactory_26 : Db4objects.Db4o.Internal.Slots.SlotChangeFactory
		{
			public _SlotChangeFactory_26()
			{
			}

			public override SlotChange NewInstance(int id)
			{
				return new IdSystemSlotChange(id);
			}
		}

		public static readonly Db4objects.Db4o.Internal.Slots.SlotChangeFactory IdSystem = 
			new _SlotChangeFactory_26();

		private sealed class _SlotChangeFactory_32 : Db4objects.Db4o.Internal.Slots.SlotChangeFactory
		{
			public _SlotChangeFactory_32()
			{
			}

			public override SlotChange NewInstance(int id)
			{
				return new FreespaceSlotChange(id);
			}
		}

		public static readonly Db4objects.Db4o.Internal.Slots.SlotChangeFactory FreeSpace
			 = new _SlotChangeFactory_32();
	}
}
