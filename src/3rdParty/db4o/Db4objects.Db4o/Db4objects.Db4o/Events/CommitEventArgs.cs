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
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Events
{
	/// <summary>Arguments for commit time related events.</summary>
	/// <remarks>Arguments for commit time related events.</remarks>
	/// <seealso cref="IEventRegistry">IEventRegistry</seealso>
	public class CommitEventArgs : TransactionalEventArgs
	{
		private readonly CallbackObjectInfoCollections _collections;

		private readonly bool _isOwnCommit;

		public CommitEventArgs(Transaction transaction, CallbackObjectInfoCollections collections
			, bool isOwnCommit) : base(transaction)
		{
			_collections = collections;
			_isOwnCommit = isOwnCommit;
		}

		/// <summary>Returns a iteration</summary>
		public virtual IObjectInfoCollection Added
		{
			get
			{
				return _collections.added;
			}
		}

		public virtual IObjectInfoCollection Deleted
		{
			get
			{
				return _collections.deleted;
			}
		}

		public virtual IObjectInfoCollection Updated
		{
			get
			{
				return _collections.updated;
			}
		}

		public virtual bool IsOwnCommit()
		{
			return _isOwnCommit;
		}
	}
}
