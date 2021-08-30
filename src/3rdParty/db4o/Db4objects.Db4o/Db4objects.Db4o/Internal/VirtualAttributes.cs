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
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class VirtualAttributes : IShallowClone
	{
		public Db4oDatabase i_database;

		public long i_version;

		public long i_uuid;

		// FIXME: should be named "uuidLongPart" or even better "creationTime" 
		public virtual object ShallowClone()
		{
			VirtualAttributes va = new VirtualAttributes();
			va.i_database = i_database;
			va.i_version = i_version;
			va.i_uuid = i_uuid;
			return va;
		}

		internal virtual bool SuppliesUUID()
		{
			return i_database != null && i_uuid != 0;
		}
	}
}
