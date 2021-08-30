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
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class CallbackObjectInfoCollections
	{
		public readonly IObjectInfoCollection added;

		public readonly IObjectInfoCollection updated;

		public readonly IObjectInfoCollection deleted;

		public static readonly Db4objects.Db4o.Internal.CallbackObjectInfoCollections Emtpy
			 = Empty();

		public CallbackObjectInfoCollections(IObjectInfoCollection added_, IObjectInfoCollection
			 updated_, IObjectInfoCollection deleted_)
		{
			added = added_;
			updated = updated_;
			deleted = deleted_;
		}

		private static Db4objects.Db4o.Internal.CallbackObjectInfoCollections Empty()
		{
			return new Db4objects.Db4o.Internal.CallbackObjectInfoCollections(ObjectInfoCollectionImpl
				.Empty, ObjectInfoCollectionImpl.Empty, ObjectInfoCollectionImpl.Empty);
		}
	}
}
