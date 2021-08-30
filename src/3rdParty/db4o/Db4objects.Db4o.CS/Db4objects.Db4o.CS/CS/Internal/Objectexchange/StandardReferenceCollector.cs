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
using System;
using System.Collections;
using Db4objects.Db4o.CS.Internal.Objectexchange;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.CS.Internal.Objectexchange
{
	public class StandardReferenceCollector : IReferenceCollector
	{
		private Transaction _transaction;

		public StandardReferenceCollector(Transaction transaction)
		{
			_transaction = transaction;
		}

		public virtual IEnumerator ReferencesFrom(int id)
		{
			CollectIdContext context = CollectIdContext.ForID(_transaction, id);
			ClassMetadata classMetadata = context.ClassMetadata();
			if (null == classMetadata)
			{
				// most probably ClassMetadata reading
				return Iterators.EmptyIterator;
			}
			if (!classMetadata.HasIdentity())
			{
				throw new InvalidOperationException(classMetadata.ToString());
			}
			if (!Handlers4.IsCascading(classMetadata.TypeHandler()))
			{
				return Iterators.EmptyIterator;
			}
			classMetadata.CollectIDs(context);
			return new TreeKeyIterator(context.Ids());
		}
	}
}
