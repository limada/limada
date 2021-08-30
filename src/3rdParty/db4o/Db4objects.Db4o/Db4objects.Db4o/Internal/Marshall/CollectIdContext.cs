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
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public class CollectIdContext : ObjectHeaderContext
	{
		private readonly IdObjectCollector _collector;

		public CollectIdContext(Transaction transaction, IdObjectCollector collector, ObjectHeader
			 oh, IReadBuffer buffer) : base(transaction, buffer, oh)
		{
			_collector = collector;
		}

		public CollectIdContext(Transaction transaction, ObjectHeader oh, IReadBuffer buffer
			) : this(transaction, new IdObjectCollector(), oh, buffer)
		{
		}

		public static Db4objects.Db4o.Internal.Marshall.CollectIdContext ForID(Transaction
			 transaction, int id)
		{
			return ForID(transaction, new IdObjectCollector(), id);
		}

		public static Db4objects.Db4o.Internal.Marshall.CollectIdContext ForID(Transaction
			 transaction, IdObjectCollector collector, int id)
		{
			StatefulBuffer reader = transaction.Container().ReadStatefulBufferById(transaction
				, id);
			if (reader == null)
			{
				return null;
			}
			ObjectHeader oh = new ObjectHeader(transaction.Container(), reader);
			return new Db4objects.Db4o.Internal.Marshall.CollectIdContext(transaction, collector
				, oh, reader);
		}

		public virtual void AddId()
		{
			int id = ReadInt();
			if (id <= 0)
			{
				return;
			}
			AddId(id);
		}

		private void AddId(int id)
		{
			_collector.AddId(id);
		}

		public override Db4objects.Db4o.Internal.ClassMetadata ClassMetadata()
		{
			return _objectHeader.ClassMetadata();
		}

		public virtual TreeInt Ids()
		{
			return _collector.Ids();
		}

		public virtual void ReadID(IReadsObjectIds objectIDHandler)
		{
			ObjectID objectID = objectIDHandler.ReadObjectID(this);
			if (objectID.IsValid())
			{
				AddId(objectID._id);
			}
		}

		public virtual IdObjectCollector Collector()
		{
			return _collector;
		}
	}
}
