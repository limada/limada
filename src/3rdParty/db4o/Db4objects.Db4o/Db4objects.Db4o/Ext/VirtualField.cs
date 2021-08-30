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

namespace Db4objects.Db4o.Ext
{
	/// <summary>intended for future virtual fields on classes.</summary>
	/// <remarks>
	/// intended for future virtual fields on classes. Currently only
	/// the constant for the virtual version field is found here.
	/// </remarks>
	/// <exclude></exclude>
	public class VirtualField
	{
		/// <summary>
		/// the field name of the virtual version field, to be used
		/// for querying.
		/// </summary>
		/// <remarks>
		/// the field name of the virtual version field, to be used
		/// for querying.
		/// </remarks>
		public static readonly string Version = Const4.VirtualFieldPrefix + "version";

		public static readonly string CommitTimestamp = Const4.VirtualFieldPrefix + "commitTimestamp";
	}
}
