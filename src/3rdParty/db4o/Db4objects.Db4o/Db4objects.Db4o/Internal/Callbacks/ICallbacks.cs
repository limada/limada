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
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Internal.Callbacks
{
	public interface ICallbacks
	{
		bool ObjectCanNew(Transaction transaction, object obj);

		bool ObjectCanActivate(Transaction transaction, object obj);

		bool ObjectCanUpdate(Transaction transaction, IObjectInfo objectInfo);

		bool ObjectCanDelete(Transaction transaction, IObjectInfo objectInfo);

		bool ObjectCanDeactivate(Transaction transaction, IObjectInfo objectInfo);

		void ObjectOnActivate(Transaction transaction, IObjectInfo obj);

		void ObjectOnNew(Transaction transaction, IObjectInfo obj);

		void ObjectOnUpdate(Transaction transaction, IObjectInfo obj);

		void ObjectOnDelete(Transaction transaction, IObjectInfo obj);

		void ObjectOnDeactivate(Transaction transaction, IObjectInfo obj);

		void ObjectOnInstantiate(Transaction transaction, IObjectInfo obj);

		void QueryOnStarted(Transaction transaction, IQuery query);

		void QueryOnFinished(Transaction transaction, IQuery query);

		bool CaresAboutCommitting();

		bool CaresAboutCommitted();

		void ClassOnRegistered(ClassMetadata clazz);

		void CommitOnStarted(Transaction transaction, CallbackObjectInfoCollections objectInfoCollections
			);

		void CommitOnCompleted(Transaction transaction, CallbackObjectInfoCollections objectInfoCollections
			, bool isOwnCommit);

		bool CaresAboutDeleting();

		bool CaresAboutDeleted();

		void OpenOnFinished(IObjectContainer container);

		void CloseOnStarted(IObjectContainer container);
	}
}
