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
	public class FrozenObjectInfo : IObjectInfo
	{
		private readonly Db4oDatabase _sourceDatabase;

		private readonly long _uuidLongPart;

		private readonly long _id;

		private readonly long _commitTimestamp;

		private readonly object _object;

		public FrozenObjectInfo(object @object, long id, Db4oDatabase sourceDatabase, long
			 uuidLongPart, long commitTimestamp)
		{
			_sourceDatabase = sourceDatabase;
			_uuidLongPart = uuidLongPart;
			_id = id;
			_commitTimestamp = commitTimestamp;
			_object = @object;
		}

		private FrozenObjectInfo(ObjectReference @ref, VirtualAttributes virtualAttributes
			) : this(@ref == null ? null : @ref.GetObject(), @ref == null ? -1 : @ref.GetID(
			), virtualAttributes == null ? null : virtualAttributes.i_database, virtualAttributes
			 == null ? -1 : virtualAttributes.i_uuid, virtualAttributes == null ? 0 : virtualAttributes
			.i_version)
		{
		}

		public FrozenObjectInfo(Transaction trans, ObjectReference @ref, bool committed) : 
			this(@ref, IsInstantiatedReference(@ref) ? @ref.VirtualAttributes(trans, committed
			) : null)
		{
		}

		private static bool IsInstantiatedReference(ObjectReference @ref)
		{
			return @ref != null && @ref.GetObject() != null;
		}

		public virtual long GetInternalID()
		{
			return _id;
		}

		public virtual object GetObject()
		{
			return _object;
		}

		public virtual Db4oUUID GetUUID()
		{
			if (_sourceDatabase == null)
			{
				return null;
			}
			return new Db4oUUID(_uuidLongPart, _sourceDatabase.GetSignature());
		}

		public virtual long GetVersion()
		{
			return GetCommitTimestamp();
		}

		public virtual long GetCommitTimestamp()
		{
			return _commitTimestamp;
		}

		public virtual long SourceDatabaseId(Transaction trans)
		{
			if (_sourceDatabase == null)
			{
				return -1;
			}
			return _sourceDatabase.GetID(trans);
		}

		public virtual long UuidLongPart()
		{
			return _uuidLongPart;
		}
	}
}
