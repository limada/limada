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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Events
{
	/// <summary>
	/// Arguments for
	/// <see cref="Db4objects.Db4o.Query.IQuery">Db4objects.Db4o.Query.IQuery</see>
	/// related events.
	/// </summary>
	/// <seealso cref="IEventRegistry">IEventRegistry</seealso>
	public class QueryEventArgs : TransactionalEventArgs
	{
		private IQuery _query;

		/// <summary>
		/// Creates a new instance for the specified
		/// <see cref="Db4objects.Db4o.Query.IQuery">Db4objects.Db4o.Query.IQuery</see>
		/// instance.
		/// </summary>
		public QueryEventArgs(Transaction transaction, IQuery q) : base(transaction)
		{
			_query = q;
		}

		/// <summary>
		/// The
		/// <see cref="Db4objects.Db4o.Query.IQuery">Db4objects.Db4o.Query.IQuery</see>
		/// which triggered the event.
		/// </summary>
		public virtual IQuery Query
		{
			get
			{
				return _query;
			}
		}
	}
}
