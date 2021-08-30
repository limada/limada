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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Convert;

namespace Db4objects.Db4o.Internal.Convert.Conversions
{
	/// <exclude></exclude>
	public class ClassIndexesToBTrees_5_5 : Conversion
	{
		public const int Version = 5;

		public virtual void Convert(LocalObjectContainer container, int classIndexId, BTree
			 bTree)
		{
			Transaction trans = container.SystemTransaction();
			ByteArrayBuffer reader = container.ReadBufferById(trans, classIndexId);
			if (reader == null)
			{
				return;
			}
			int entries = reader.ReadInt();
			for (int i = 0; i < entries; i++)
			{
				bTree.Add(trans, reader.ReadInt());
			}
		}

		public override void Convert(ConversionStage.SystemUpStage stage)
		{
			// calling #storedClasses forces reading all classes
			// That's good enough to load them all and to call the
			// above convert method.
			stage.File().StoredClasses();
		}
	}
}
