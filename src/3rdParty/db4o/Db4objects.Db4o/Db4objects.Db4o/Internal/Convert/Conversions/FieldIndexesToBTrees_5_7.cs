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
using Db4objects.Db4o.Internal.Convert;

namespace Db4objects.Db4o.Internal.Convert.Conversions
{
	/// <exclude></exclude>
	public class FieldIndexesToBTrees_5_7 : Conversion
	{
		public const int Version = 6;

		public override void Convert(ConversionStage.SystemUpStage stage)
		{
			stage.File().ClassCollection().WriteAllClasses();
			RebuildUUIDIndex(stage.File());
			FreeOldUUIDMetaIndex(stage.File());
		}

		private void RebuildUUIDIndex(LocalObjectContainer file)
		{
			UUIDFieldMetadata uuid = file.UUIDIndex();
			ClassMetadataIterator i = file.ClassCollection().Iterator();
			while (i.MoveNext())
			{
				ClassMetadata clazz = i.CurrentClass();
				if (clazz.GenerateUUIDs())
				{
					uuid.RebuildIndexForClass(file, clazz);
				}
			}
		}

		/// <param name="file"></param>
		private void FreeOldUUIDMetaIndex(LocalObjectContainer file)
		{
		}
		// updating removed here to allow removing MetaIndex class
	}
}
