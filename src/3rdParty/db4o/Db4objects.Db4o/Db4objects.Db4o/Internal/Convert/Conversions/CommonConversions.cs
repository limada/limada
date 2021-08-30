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
using Db4objects.Db4o.Internal.Convert;
using Db4objects.Db4o.Internal.Convert.Conversions;

namespace Db4objects.Db4o.Internal.Convert.Conversions
{
	/// <exclude></exclude>
	public class CommonConversions
	{
		public static void Register(Converter converter)
		{
			converter.Register(ClassIndexesToBTrees_5_5.Version, new ClassIndexesToBTrees_5_5
				());
			converter.Register(FieldIndexesToBTrees_5_7.Version, new FieldIndexesToBTrees_5_7
				());
			converter.Register(ClassAspects_7_4.Version, new ClassAspects_7_4());
			converter.Register(ReindexNetDateTime_7_8.Version, new ReindexNetDateTime_7_8());
			converter.Register(DropEnumClassIndexes_7_10.Version, new DropEnumClassIndexes_7_10
				());
			converter.Register(DropGuidClassIndexes_7_12.Version, new DropGuidClassIndexes_7_12
				());
			converter.Register(DropDateTimeOffsetClassIndexes_7_12.Version, new DropDateTimeOffsetClassIndexes_7_12
				());
			converter.Register(VersionNumberToCommitTimestamp_8_0.Version, new VersionNumberToCommitTimestamp_8_0
				());
		}
	}
}
