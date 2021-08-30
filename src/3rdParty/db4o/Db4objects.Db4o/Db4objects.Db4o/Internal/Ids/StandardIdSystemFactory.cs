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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Ids;

namespace Db4objects.Db4o.Internal.Ids
{
	/// <exclude></exclude>
	public class StandardIdSystemFactory
	{
		public const byte Legacy = 0;

		public const byte PointerBased = 1;

		public const byte StackedBtree = 2;

		public const byte Default = StackedBtree;

		public const byte InMemory = 3;

		public const byte Custom = 4;

		public const byte SingleBtree = 5;

		public static IIdSystem NewInstance(LocalObjectContainer localContainer)
		{
			SystemData systemData = localContainer.SystemData();
			byte idSystemType = systemData.IdSystemType();
			switch (idSystemType)
			{
				case Legacy:
				{
					return new PointerBasedIdSystem(localContainer);
				}

				case PointerBased:
				{
					return new PointerBasedIdSystem(localContainer);
				}

				case StackedBtree:
				{
					InMemoryIdSystem inMemoryIdSystem = new InMemoryIdSystem(localContainer);
					BTreeIdSystem bTreeIdSystem = new BTreeIdSystem(localContainer, inMemoryIdSystem);
					systemData.FreespaceIdSystem(bTreeIdSystem.FreespaceIdSystem());
					return new BTreeIdSystem(localContainer, bTreeIdSystem);
				}

				case SingleBtree:
				{
					InMemoryIdSystem smallInMemoryIdSystem = new InMemoryIdSystem(localContainer);
					BTreeIdSystem smallBTreeIdSystem = new BTreeIdSystem(localContainer, smallInMemoryIdSystem
						);
					systemData.FreespaceIdSystem(smallBTreeIdSystem.FreespaceIdSystem());
					return smallBTreeIdSystem;
				}

				case InMemory:
				{
					return new InMemoryIdSystem(localContainer);
				}

				case Custom:
				{
					IIdSystemFactory customIdSystemFactory = localContainer.ConfigImpl.CustomIdSystemFactory
						();
					if (customIdSystemFactory == null)
					{
						throw new Db4oFatalException("Custom IdSystem configured but no factory was found. See IdSystemConfiguration#useCustomSystem()"
							);
					}
					return customIdSystemFactory.NewInstance(localContainer);
				}

				default:
				{
					return new PointerBasedIdSystem(localContainer);
					break;
				}
			}
		}
	}
}
