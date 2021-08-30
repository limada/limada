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
namespace Db4objects.Db4o.Filestats
{
	/// <exclude></exclude>
	public sealed class FileUsageStatsUtil
	{
		private static readonly string Padding = "                    ";

		private FileUsageStatsUtil()
		{
		}

		public static string FormatLine(string label, long amount)
		{
			return PadLeft(label, 20) + ": " + PadLeft(amount.ToString(), 12) + "\n";
		}

		private static string PadLeft(string val, int len)
		{
			return Sharpen.Runtime.Substring(Padding, 0, len - val.Length) + val;
		}
	}
}
#endif // !SILVERLIGHT
