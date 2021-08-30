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
	public class LazyObjectReference : IObjectInfo
	{
		private readonly Transaction _transaction;

		private readonly int _id;

		public LazyObjectReference(Transaction transaction, int id)
		{
			_transaction = transaction;
			_id = id;
		}

		public virtual long GetInternalID()
		{
			return _id;
		}

		public virtual object GetObject()
		{
			lock (ContainerLock())
			{
				return Reference().GetObject();
			}
		}

		public virtual Db4oUUID GetUUID()
		{
			lock (ContainerLock())
			{
				return Reference().GetUUID();
			}
		}

		public virtual long GetVersion()
		{
			return GetCommitTimestamp();
		}

		public virtual long GetCommitTimestamp()
		{
			lock (ContainerLock())
			{
				return Reference().GetCommitTimestamp();
			}
		}

		public virtual ObjectReference Reference()
		{
			HardObjectReference hardRef = _transaction.Container().GetHardObjectReferenceById
				(_transaction, _id);
			return hardRef._reference;
		}

		private object ContainerLock()
		{
			_transaction.Container().CheckClosed();
			return _transaction.Container().Lock();
		}
	}
}
