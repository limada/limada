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
using Db4objects.Db4o.CS.Caching;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Objectexchange;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Objectexchange
{
	public class EagerObjectExchangeStrategy : IObjectExchangeStrategy
	{
		private ObjectExchangeConfiguration _config;

		public EagerObjectExchangeStrategy(ObjectExchangeConfiguration config)
		{
			_config = config;
		}

		public virtual ByteArrayBuffer Marshall(LocalTransaction transaction, IIntIterator4
			 ids, int maxCount)
		{
			return new EagerObjectWriter(_config, transaction).Write(ids, maxCount);
		}

		public virtual IFixedSizeIntIterator4 Unmarshall(ClientTransaction transaction, IClientSlotCache
			 slotCache, ByteArrayBuffer reader)
		{
			return new CacheContributingObjectReader(transaction, slotCache, reader).Iterator
				();
		}
	}
}
