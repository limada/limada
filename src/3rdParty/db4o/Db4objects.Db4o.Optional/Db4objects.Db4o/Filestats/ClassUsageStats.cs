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
using System.Text;
using Db4objects.Db4o.Filestats;

namespace Db4objects.Db4o.Filestats
{
	/// <summary>Statistics for the byte usage for a single class (instances, indices, etc.) in a db4o database file.
	/// 	</summary>
	/// <remarks>Statistics for the byte usage for a single class (instances, indices, etc.) in a db4o database file.
	/// 	</remarks>
	/// <exclude></exclude>
	public class ClassUsageStats
	{
		private readonly string _className;

		private readonly long _slotUsage;

		private readonly long _classIndexUsage;

		private readonly long _fieldIndexUsage;

		private readonly long _miscUsage;

		internal ClassUsageStats(string className, long slotSpace, long classIndexUsage, 
			long fieldIndexUsage, long miscUsage)
		{
			_className = className;
			_slotUsage = slotSpace;
			_classIndexUsage = classIndexUsage;
			_fieldIndexUsage = fieldIndexUsage;
			_miscUsage = miscUsage;
		}

		/// <returns>the name of the persistent class</returns>
		public virtual string ClassName()
		{
			return _className;
		}

		/// <returns>number of bytes used slots containing the actual class instances</returns>
		public virtual long SlotUsage()
		{
			return _slotUsage;
		}

		/// <returns>number of bytes used for the index of class instances</returns>
		public virtual long ClassIndexUsage()
		{
			return _classIndexUsage;
		}

		/// <returns>number of bytes used for field indexes, if any</returns>
		public virtual long FieldIndexUsage()
		{
			return _fieldIndexUsage;
		}

		/// <returns>
		/// number of bytes used for features that are specific to this class (ex.: the BTree encapsulated within a
		/// <see cref="Db4objects.Db4o.Internal.Collections.BigSet{E}">Db4objects.Db4o.Internal.Collections.BigSet&lt;E&gt;
		/// 	</see>
		/// instance)
		/// </returns>
		public virtual long MiscUsage()
		{
			return _miscUsage;
		}

		/// <returns>aggregated byte usage for this persistent class</returns>
		public virtual long TotalUsage()
		{
			return _slotUsage + _classIndexUsage + _fieldIndexUsage + _miscUsage;
		}

		public override string ToString()
		{
			StringBuilder str = new StringBuilder();
			ToString(str);
			return str.ToString();
		}

		internal virtual void ToString(StringBuilder str)
		{
			str.Append(ClassName()).Append("\n");
			str.Append(FileUsageStatsUtil.FormatLine("Slots", SlotUsage()));
			str.Append(FileUsageStatsUtil.FormatLine("Class index", ClassIndexUsage()));
			str.Append(FileUsageStatsUtil.FormatLine("Field indices", FieldIndexUsage()));
			if (MiscUsage() > 0)
			{
				str.Append(FileUsageStatsUtil.FormatLine("Misc", MiscUsage()));
			}
			str.Append(FileUsageStatsUtil.FormatLine("Total", TotalUsage()));
		}
	}
}
#endif // !SILVERLIGHT
